using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Localize.Strings
{
    public static class AddStringLocalizerExtension
    {
        public static void AddStringLocalizer(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<StringLocalizer>();
        }
    }

    internal class StringLocalizer
    {
        // Hint: This is a prototype to check if we are able to do a full automation of the localization process.
        //       
        //       This fixture allows you to localize all exception messages in all available language files.
        //       This code parse all the exception message strings out and create the keys and its default language "english" -> "en"
        //       into the language files.
        public async Task LocalizeAsync(IImmutableList<string> languages,
                                        string solutionPath)
        {
            var workspace = MSBuildWorkspace.Create();

            // Load the entire solution
            var solution = await workspace.OpenSolutionAsync(solutionPath);

            foreach (var project in solution.Projects)
            {
                if (project.IsNull())
                {
                    continue;
                }

                if (project.FilePath.IsNullOrWhiteSpace())
                {
                    continue;
                }

                if (project.Name.Contains(".Test", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var allTranslations = new Dictionary<string, Dictionary<string, string>>();
                var translationFolder = new DirectoryInfo(Path.Combine(new FileInfo(project.FilePath).Directory!.FullName, "Translations"));
                var languageFiles = languages.Select(l => new FileInfo(Path.Combine(translationFolder.FullName, $"{l}.json"))).ToList();
                var compilation = await project.GetCompilationAsync();

                if (compilation.IsNull())
                {
                    continue;
                }

                // Iterate over each document in the project
                foreach (var document in project.Documents)
                {
                    var syntaxTree = await document.GetSyntaxTreeAsync();

                    if (syntaxTree.IsNull())
                    {
                        continue;
                    }


                    var semanticModel = compilation.GetSemanticModel(syntaxTree);

                    var throwStatements = FindAllThrowStatements(syntaxTree);

                    foreach (var throwStatement in throwStatements)
                    {
                        var translatableStrings = GetTranslatableStrings(semanticModel, throwStatement).ToList();
                        foreach (var translatableString in translatableStrings)
                        {
                            foreach (var language in languages)
                            {
                                if (allTranslations.ContainsKey(language).IsFalse())
                                {
                                    allTranslations.Add(language, new Dictionary<string, string>());
                                }

                                allTranslations[language][translatableString.Key] = translatableString.Text;
                            }
                        }
                    }
                }

                // Write all project specific translations to the language files
                foreach (var languageFile in languageFiles)
                {
                    if (languageFile.Directory!.NotExists())
                    {
                        languageFile.Directory!.Create();
                    }

                    if (allTranslations.TryGetValue(languageFile.NameWithoutExtension(), out var translations))
                    {
                        await File.WriteAllTextAsync(languageFile.FullName, translations.ToJsonIntended());
                    }
                }
            }
        }

        private static IEnumerable<ThrowStatementSyntax> FindAllThrowStatements(SyntaxTree syntaxTree)
        {
            var root = syntaxTree.GetRoot();
            return root.DescendantNodes().OfType<ThrowStatementSyntax>()
                       .Where(ts => ts.Expression is ObjectCreationExpressionSyntax);
        }

        private static MethodDeclarationSyntax? GetContainingMethod(SyntaxNode? node)
        {
            while (node != null)
            {
                if (node is MethodDeclarationSyntax methodDeclaration)
                {
                    return methodDeclaration;
                }
                node = node.Parent;
            }
            return null;
        }

        private static ClassDeclarationSyntax? GetContainingClass(SyntaxNode? node)
        {
            while (node != null)
            {
                if (node is ClassDeclarationSyntax classDeclaration)
                {
                    return classDeclaration;
                }
                node = node.Parent;
            }
            return null;
        }

        private static IEnumerable<TextToTranslate> GetTranslatableStrings(SemanticModel semanticModel,
                                                                           ThrowStatementSyntax throwStatement)
        {
            // Get the method and class containing the throw statement
            var methodDeclaration = GetContainingMethod(throwStatement);
            var classDeclaration = GetContainingClass(throwStatement);

            if (methodDeclaration == null ||
                classDeclaration == null)
            {
                yield break;
            }

            // Get the full class name
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

            if (classSymbol.IsNull())
            {
                yield break;
            }

            string fullyQualifiedClassName = classSymbol.ToDisplayString();

            // Get the method name
            string methodName = methodDeclaration.Identifier.Text;

            // Get the exception type
            var objectCreation = throwStatement.Expression as ObjectCreationExpressionSyntax;
            if (objectCreation.IsNull())
            {
                yield break;
            }

            var constructorSymbol = semanticModel.GetSymbolInfo(objectCreation).Symbol as IMethodSymbol;
            if (constructorSymbol.IsNull())
            {
                yield break;
            }

            if (objectCreation.ArgumentList.IsNull())
            {
                yield break;
            }

            // Generate the text key for each parameter
            for (int i = 0; i < objectCreation.ArgumentList.Arguments.Count; i++)
            {
                var argument = objectCreation.ArgumentList.Arguments[i];

                if (constructorSymbol.Parameters.Length <= i)
                {
                    break;
                }

                var parameter = constructorSymbol.Parameters[i];

                // Check if the argument is a string literal expression
                if (argument.Expression is LiteralExpressionSyntax literalExpression &&
                    literalExpression.IsKind(SyntaxKind.StringLiteralExpression))
                {
                    // Get the parameter name (e.g., "message" for ArgumentException)
                    string parameterName = parameter.Name.FirstCharToUpper();

                    // Get the string value from the literal expression
                    var stringValue = literalExpression.Token.ValueText;

                    // Combine the fully qualified class name, method name, exception type, and parameter name, and text value itself
                    // Sample: 
                    // throw new ArgumentException("This is the exception message");
                    // MyAssembly.MyNamespace.MyClass.MyMethod.ArgumentException.Message.This is the exception message
                    var parameterKey = $"{fullyQualifiedClassName}.{methodName}.{constructorSymbol.ContainingType.Name}.{parameterName}.{stringValue}";

                    // Return the key and the string value as a tuple
                    yield return new(parameterKey, stringValue);
                }
            }
        }
    }

    internal record TextToTranslate(string Key,
                                    string Text);
}
