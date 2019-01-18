using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.DTO
{
    public abstract class ToolchainOperation
    {

    }

    public class ResetRules : ToolchainOperation
    {
    }

    public class CommandLineInfo : ToolchainOperation
    {
        public string FileName { get; set; }
        public string Arguments { get; set; }
    }

    public class CmdInvocation : CommandLineInfo
    {
        public CmdInvocation() { }

        public CmdInvocation(string arguments)
        {
            FileName = "cmd.exe";
            Arguments = $"/c {arguments}";
        }
    }

    public class BashInvocation : CommandLineInfo
    {
        public BashInvocation() { }

        public BashInvocation(string arguments)
        {
            FileName = "/bin/bash";
            Arguments = $"-c \"{arguments}\"";
        }
    }
}
