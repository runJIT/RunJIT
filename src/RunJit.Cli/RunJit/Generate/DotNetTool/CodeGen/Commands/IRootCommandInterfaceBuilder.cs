﻿using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal interface IRootCommandInterfaceBuilder
    {
        string Build(string project, CommandInfo parameterInfo, string nameSpace);
    }
}
