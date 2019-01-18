using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            Console.WriteLine($"PID: {Process.GetCurrentProcess().Id}");
            Console.WriteLine($"Path: {Environment.GetEnvironmentVariable("PATH")}");

            var port = args.FirstOrDefault() ?? "15000";

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://0.0.0.0:{port}/")
            ;
        }
    }
}
