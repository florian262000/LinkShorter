using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace LinkShorter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create config file if not exits
            if (!File.Exists("config.json"))
            {
                using (var outputFile = new StreamWriter("config.json"))
                {
                    outputFile.Write("{"+
                                    "\"database\": { "+
                                    "\"host\": \"DEFAULT\", " +
                                    "\"username\": \"DEFAULT\"," +
                                    "\"password\": \"DEFAULT\", " +
                                "\"name\": \"DEFAULT\" " +
                            "},"+
                            "\"urlbase\": \"DEFAULT\","+
                              "\"password_pepper\": \"DEFAULT\""+
                        "}");
                    
                }
                // run deploy script up

            }
            
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}