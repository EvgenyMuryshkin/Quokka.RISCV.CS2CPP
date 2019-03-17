using Newtonsoft.Json;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Tools;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Quokka.RISCV.Integration.Client
{
    public class RISCVIntegrationClient
    {
        public static async Task<RISCVIntegrationContext> Run(RISCVIntegrationContext context)
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
