﻿using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal interface ICommandBuilderWithArgumentAndOption
    {
        string Build(string project, CommandInfo parameterInfo, CommandInfo parent, string nameSpace);
    }
}
