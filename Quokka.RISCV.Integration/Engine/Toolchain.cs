using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.RuleHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quokka.RISCV.Integration.Engine
{
    public class Toolchain : IDisposable
    {
        Guid _correlationId;

        public string RootPath { get; private set; }

        private List<RuleHandler> _rules = new List<RuleHandler>();

        public Toolchain(Guid correlationId)
        {
            Console.WriteLine($"======================================================");
            Console.WriteLine($"Toolchain request {correlationId}");

            _correlationId = correlationId;
            RootPath = Path.Combine(Path.GetTempPath(), _correlationId.ToString());
            Directory.CreateDirectory(RootPath);
        }

        public void SaveSnapshot(FSSnapshot fsSnashot)
        {
            new FSManager(RootPath).SaveSnapshot(fsSnashot);
        }

        public FSSnapshot LoadSnapshot(ExtensionClasses classes)
        {
            return new FSManager(RootPath)
                .LoadSnapshot(
                    classes, 
                    _rules
                        .SelectMany(r => r.MatchingFiles())
                        .ToHashSet());
        }

        public void SetupRules(IEnumerable<FileRule> rules)
        {
            DisposeRules();

            _rules = new RulesManager(RootPath).CreateRules(rules);
        }

        public void Invoke(IEnumerable<ToolchainOperation> operations)
        {
            var current = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(RootPath);

                foreach (var cmd in operations)
                {
                    switch (cmd)
                    {
                        case CommandLineInfo commandLine:
                        {
                            Console.WriteLine($"Running {commandLine.Arguments}");

                            var psi = new ProcessStartInfo()
                            {
                                FileName = commandLine.FileName,
                                Arguments = commandLine.Arguments,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                            };

                            var process = new Process()
                            {
                                StartInfo = psi
                            };
                            process.Start();
                            string result = process.StandardOutput.ReadToEnd();
                            string errors = process.StandardError.ReadToEnd();

                            process.WaitForExit();

                            Console.WriteLine($"Completed with {process.ExitCode}");
                            Console.WriteLine($"Stdout: {result}");
                            Console.WriteLine($"Stderror {errors}");

                            if (process.ExitCode != 0)
                                throw new Exception(errors);

                        }   break;

                        case ResetRules rst:
                        {
                            _rules.ForEach(r => r.Reset());
                        }   break;
                    }
                }
            }
            finally
            {
                Directory.SetCurrentDirectory(current);
            }
        }

        #region IDisposable Support

        void DisposeRules()
        {
            if (_rules != null)
            {
                _rules.ForEach(r => r.Dispose());
            }

            _rules = null;
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeRules();
                    Directory.Delete(RootPath, true);
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        public static InvokeResponse Invoke(InvokeRequest request)
        {
            var response = new InvokeResponse()
            {
                CorrelationId = request.CorrelationId
            };

            using (var toolchain = new Toolchain(request.CorrelationId))
            {
                toolchain.SaveSnapshot(request.Source);
                toolchain.SetupRules(request.ResultRules);
                toolchain.Invoke(request.Operations);

                response.Result = toolchain.LoadSnapshot(request.ExtensionClasses);
            }

            return response;
        }
    }
}
