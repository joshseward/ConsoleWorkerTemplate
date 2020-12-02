using ConsoleWorker.Configuration;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleWorker
{
    [Command(Name = "ConsoleWorker", Description = "The application description")]
    [HelpOption("-?")]
    public class Program
    {
        public static async Task Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "WorkerA = 1, WorkerB = 2")]
        private WorkerType WorkerType { get; }

        [Option("-T|Test", Description = "json")]
        private string Test { get; }

        private async Task OnExecute()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var serviceProvider = new ServiceCollection();
            
            switch (WorkerType)
            {                
                case WorkerType.WorkerA:
                    serviceProvider.AddTransient<IWorker, WorkerA>();
                    serviceProvider.AddTransient<WorkerASetting>(x =>  new WorkerASetting() 
                    {
                        Api = config.GetValue<string>("ApiKey"),
                        ApiKey = config.GetValue<string>("QuotaApi"),
                        TestSetting = config.GetValue<int>("WorkerASetting:TestSetting"),
                    });
                    break;
                case WorkerType.WorkerB:
                default:
                    serviceProvider.AddTransient<IWorker, WorkerB>();
                    
                    serviceProvider.AddTransient<AppSettings>(x => new AppSettings()
                    {
                        Api = config.GetValue<string>("ApiKey"),
                        ApiKey = config.GetValue<string>("QuotaApi")
                    });
                    break;
            }

            var appSettings = new AppSettings()
            {
                ApiKey = config.GetValue<string>("ApiKey"),
                Api = config.GetValue<string>("Api"),
            };            

            serviceProvider.AddTransient<AppSettings>(x => { return appSettings; });
            serviceProvider.AddHttpClient();

            var builtServiceProvider = serviceProvider.BuildServiceProvider();

            var worker = builtServiceProvider.GetService<IWorker>();
            await worker.Start();
        }        
    }
}
