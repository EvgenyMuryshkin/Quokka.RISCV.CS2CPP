using Newtonsoft.Json;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Quokka.RISCV.Integration.Client
{
    public class RISCVIntegrationClientContext
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

        protected RISCVIntegrationClientContext Clone()
        {
            return JsonHelper.Clone(this);
        }

        public RISCVIntegrationClientContext TakeModifiedFiles()
        {
            var result = Clone();
            result.ResultFileRules.Add(new ModifiedFilesRule());

            return result;
        }

        public RISCVIntegrationClientContext TakeRegexMatchFiles(params string[] patterns)
        {
            var result = Clone();
            result.ResultFileRules.Add(new RegexMatchFilesRule() { Patterns = patterns?.ToList() });

            return result;
        }

        public RISCVIntegrationClientContext TakeExtensionMatchFiles(params string[] extensions)
        {
            var result = Clone();
            result.ResultFileRules.Add(new ExtensionMatchFilesRule() { Extensions = extensions?.ToList() });

            return result;
        }

        public RISCVIntegrationClientContext WithURL(string url)
        {
            var result = Clone();
            result.URL = url;

            return result;
        }

        public RISCVIntegrationClientContext WithOperations(params ToolchainOperation[] list)
        {
            var result = Clone();
            result.Operations = list.ToList();

            return result;
        }

        public RISCVIntegrationClientContext WithResultFileRules(List<FileRule> rules)
        {
            var result = Clone();
            result.ResultFileRules = rules.ToList();

            return result;
        }

        public RISCVIntegrationClientContext WithPort(int port)
        {
            var result = Clone();
            result.Port = port;

            return result;
        }

        public RISCVIntegrationClientContext WithExtensionClasses(ExtensionClasses extensionClasses)
        {
            var result = Clone();
            result.ExtensionClasses = extensionClasses;

            return result;
        }

        public RISCVIntegrationClientContext WithRootFolder(string rootFolder)
        {
            var result = Clone();
            result.RootFolder = rootFolder;

            return result;
        }

        public RISCVIntegrationClientContext WithSourceSnapshot(FSSnapshot snapshot)
        {
            var result = Clone();
            result.SourceSnapshot = snapshot;

            return result;
        }

        public RISCVIntegrationClientContext WithResultSnapshot(FSSnapshot snapshot)
        {
            var result = Clone();
            result.ResultSnapshot = snapshot;

            return result;
        }

        public RISCVIntegrationClientContext WithAllFiles()
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

        public RISCVIntegrationClientContext WithDockerImage(string dockerImage)
        {
            var result = Clone();

            result.DockerImage = dockerImage;

            return result;
        }
    }

    public class RISCVIntegrationClient
    {
        public static async Task<RISCVIntegrationClientContext> Run(RISCVIntegrationClientContext context)
        {
            using (var client = new HttpClient() )
            {
                var request = new InvokeRequest()
                {
                    ExtensionClasses = context.ExtensionClasses,
                    Source = context.SourceSnapshot,
                    Operations = context.Operations,
                    ResultRules = context.ResultFileRules
                };

                var requestPayload = JsonHelper.Serialize(request);

                var test = JsonHelper.Deserialize<InvokeRequest>(requestPayload);

                var requestContent = new StringContent(requestPayload, Encoding.UTF8, "application/json");
                var url = $"{context.URL}:{context.Port}/api/RISCV/Invoke";

                var response = await client.PostAsync(url, requestContent);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var exceptionPayload = await response.Content.ReadAsStringAsync();

                    throw new Exception($"Failed to post data to '{url}': {exceptionPayload}");
                }

                var responsePayload = await response.Content.ReadAsStringAsync();
                var invokeResponse = JsonHelper.Deserialize<InvokeResponse>(responsePayload);

                return context.WithResultSnapshot(invokeResponse.Result);
            }
        }
    }
}
