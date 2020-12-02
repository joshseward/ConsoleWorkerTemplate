using ConsoleWorker.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleWorker
{
    public class WorkerB: Worker, IWorker
    {
        private readonly AppSettings _appSttings;

        public WorkerB(HttpClient client, AppSettings appSttings): base(client, appSttings)
        {
            _appSttings = appSttings;
        }

        public async Task Start()
        {
            Console.WriteLine($"Starting Run {DateTime.UtcNow}");

            Console.WriteLine($"End Run {DateTime.UtcNow}");
        }
    }
}
