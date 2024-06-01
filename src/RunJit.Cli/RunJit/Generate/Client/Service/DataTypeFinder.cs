﻿using System.Collections.Immutable;
using System.Reflection;
using Extensions.Pack;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddDataTypeFinderExtension
    {
        internal static void AddDataTypeFinder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<DataTypeFinder>();
        }
    }

    internal record DeclarationToType(DeclarationBase Declaration, Type Type);

    public class DataTypeFinder
    {
        internal IImmutableList<DeclarationToType> FindDataType(MethodInfo methodInfo, IImmutableList<CSharpSyntaxTree> syntaxTrees)
        {
            var parameters = methodInfo.GetParameters().Select(p => p.ParameterType);
            var declaredTypes = parameters.Concat(methodInfo.ReturnType).ToImmutableList();
            var allGenericTypes = declaredTypes.SelectMany(declaredType => declaredType.GetAllGenericArguments()).ToImmutableList();
            var allDeclaredTypes = declaredTypes.Concat(allGenericTypes).ToImmutableList();

            var alreadyFound = new List<string>();
            var allTypesWithAllSubTypes = GetAllSubTypes(allDeclaredTypes, alreadyFound).ToImmutableList();

            var allModels = allTypesWithAllSubTypes.Select(type => syntaxTrees.FindDataType(type))
                                                   .Where(result => result.Declaration.IsNotNull())
                                                   .Select(result => new DeclarationToType(result.Declaration!, result.Type))
                                                   .ToImmutableList();

            return allModels;
        }

        internal IImmutableList<DeclarationToType> FindDataType(IImmutableList<string> fullqualifiedNames, 
                                                                IImmutableList<CSharpSyntaxTree> syntaxTrees,
                                                                IImmutableList<Type> reflectionTypes)
        {
            var alreadyFound = new List<string>();

            var types = reflectionTypes.Where(t => fullqualifiedNames.Contains(t.FullName)).ToImmutableList();
            
            var allTypesWithAllSubTypes = GetAllSubTypes(types, alreadyFound).ToImmutableList();

            var allModels = allTypesWithAllSubTypes.Select(type => syntaxTrees.FindDataType(type))
                                                   .Where(result => result.Declaration.IsNotNull())
                                                   .Select(result => new DeclarationToType(result.Declaration!, result.Type))
                                                   .ToImmutableList();

            return allModels;
        }
        
        private IEnumerable<Type> GetAllSubTypes(IImmutableList<Type> types, List<string> alreadyFound)
        {
            foreach (var type in types)
            {
                if (alreadyFound.Contains(type.FullName!))
                {
                    continue;
                }

                if (type.FullName.IsNull())
                {
                    continue;
                }

                // we are not interested in system types or any type from microsoft
                if (type.IsSystemType())
                {
                    continue;
                }

                if (type.FullName.Contains("Microsoft."))
                {
                    continue;
                }

                alreadyFound.Add(type.FullName!);

                var properties = type.GetProperties().Select(p => p.PropertyType).ToImmutableList();
                var genericArguments = properties.SelectMany(p => p.GetAllGenericArguments()).ToImmutableList();
                var allPropertyTypes = properties.Concat(genericArguments)
                                                 .Where(p => p.FullName.NotEqualsTo(type.FullName)).ToImmutableList();

                allPropertyTypes = type.BaseType.IsNotNull() ? allPropertyTypes.Add(type.BaseType) : allPropertyTypes;


                var subTypes = GetAllSubTypes(allPropertyTypes, alreadyFound).ToImmutableList();

                foreach (var subType in subTypes)
                {
                    alreadyFound.Add(subType.FullName!);
                    yield return subType;
                }

                yield return type;
            }
        }
    }
}
