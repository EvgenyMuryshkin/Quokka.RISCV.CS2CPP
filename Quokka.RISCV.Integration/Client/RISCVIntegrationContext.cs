using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quokka.RISCV.Integration.Client
{
    public class RISCVIntegrationContext
    {
        public ExtensionClasses ExtensionClasses = new ExtensionClasses();
        public string RootFolder;
        public FSSnapshot SourceSnapshot = new FSSnapshot();
        public FSSnapshot ResultSnapshot = new FSSnapshot();
        public string DockerImage;
        public string URL = "http://localhost";
        public int Port = 15000;
        public List<ToolchainOperation> Operations = new List<ToolchainOperation>();
        public List<FileRule> ResultFileRules = new List<FileRule>();

        protected RISCVIntegrationContext Clone()
        {
            return JsonHelper.Clone(this);
        }

        public RISCVIntegrationContext TakeModifiedFiles()
        {
            var result = Clone();
            result.ResultFileRules.Add(new ModifiedFilesRule());

            return result;
        }

        public RISCVIntegrationContext TakeRegexMatchFiles(params string[] patterns)
        {
            var result = Clone();
            result.ResultFileRules.Add(new RegexMatchFilesRule() { Patterns = patterns?.ToList() });

            return result;
        }

        public RISCVIntegrationContext TakeExtensionMatchFiles(params string[] extensions)
        {
            var result = Clone();
            result.ResultFileRules.Add(new ExtensionMatchFilesRule() { Extensions = extensions?.ToList() });

            return result;
        }

        public RISCVIntegrationContext WithURL(string url)
        {
            var result = Clone();
            result.URL = url;

            return result;
        }

        public RISCVIntegrationContext WithOperations(params ToolchainOperation[] list)
        {
            var result = Clone();
            result.Operations = list.ToList();

            return result;
        }

        public RISCVIntegrationContext WithResultFileRules(List<FileRule> rules)
        {
            var result = Clone();
            result.ResultFileRules = rules.ToList();

            return result;
        }

        public RISCVIntegrationContext WithPort(int port)
        {
            var result = Clone();
            result.Port = port;

            return result;
        }

        public RISCVIntegrationContext WithExtensionClasses(ExtensionClasses extensionClasses)
        {
            var result = Clone();
            result.ExtensionClasses = extensionClasses;

            return result;
        }

        public RISCVIntegrationContext WithRootFolder(string rootFolder)
        {
            var result = Clone();
            result.RootFolder = rootFolder;

            return result;
        }

        public RISCVIntegrationContext WithSourceSnapshot(FSSnapshot snapshot)
        {
            var result = Clone();
            result.SourceSnapshot = snapshot;

            return result;
        }

        public RISCVIntegrationContext WithResultSnapshot(FSSnapshot snapshot)
        {
            var result = Clone();
            result.ResultSnapshot = snapshot;

            return result;
        }

        public RISCVIntegrationContext WithAllRegisteredFiles()
        {
            var result = Clone();

            var fs = new FSManager(RootFolder);
            var allFiles = Directory
                .EnumerateFiles(RootFolder, "*.*", SearchOption.AllDirectories)
                .Where(file => ExtensionClasses.Contains(Path.GetExtension(file)))
                ;

            var snapshot = fs.LoadSnapshot(ExtensionClasses, allFiles);

            return result.WithSourceSnapshot(snapshot);
        }

        public RISCVIntegrationContext WithAllFiles()
        {
            var result = Clone();

            var fs = new FSManager(RootFolder);
            var allFiles = Directory
                .EnumerateFiles(RootFolder, "*.*", SearchOption.AllDirectories)
                ;

            var snapshot = fs.LoadSnapshot(ExtensionClasses, allFiles);

            return result.WithSourceSnapshot(snapshot);
        }

        public RISCVIntegrationContext WithDockerImage(string dockerImage)
        {
            var result = Clone();

            result.DockerImage = dockerImage;

            return result;
        }
    }
}
