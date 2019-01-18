using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet("[action]")]
        public bool IsAlive()
        {
            return true;
        }
    }
}
