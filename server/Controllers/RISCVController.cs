using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;

namespace server.Controllers
{
    [Route("api/[controller]")]
    public class RISCVController : Controller
    {
        [HttpPost("[action]")]
        public ActionResult Invoke([FromBody] BaseRequest request)
        {
            try
            {
                switch (request)
                {
                    case InvokeRequest invoke:
                    {
                        var response = new InvokeResponse()
                        {
                            CorrelationId = invoke.CorrelationId
                        };

                        using (var toolchain = new Toolchain(invoke.CorrelationId))
                        {
                            toolchain.SaveSnapshot(invoke.Source);
                            toolchain.SetupRules(invoke.ResultRules);
                            toolchain.Invoke(invoke.Operations);

                            response.Result = toolchain.LoadSnapshot(invoke.ExtensionClasses);
                        }

                        return Ok(response);
                    }
                    default:
                        return BadRequest($"Unsupported request type: {request.GetType()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return BadRequest(ex.ToString());
            }
        }
    }
}
