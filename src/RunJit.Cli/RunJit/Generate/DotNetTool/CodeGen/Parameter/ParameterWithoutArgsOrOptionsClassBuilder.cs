﻿using Argument.Check;
using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class ParameterWithoutArgsOrOptionsClassBuilder : IParameterSpecificClassBuilder
    {
        private const string Template =
            @"
namespace $namespace$
{    
    internal sealed class $command-name$Parameters
    {
        public $command-name$Parameters()
        {
        }
    }
}";

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var newTemplate = Template.Replace("$projectName$", projectName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$command-name$", parameterInfo.NormalizedName);

            return newTemplate;
        }

        public bool IsThisBuilderFor(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return ObjectExtensions.IsNull((object?)parameterInfo.Argument) && parameterInfo.Options.IsNullOrEmpty();
        }
    }
}
