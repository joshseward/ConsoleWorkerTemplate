using ConsoleWorker.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleWorker
{
    public class WorkerA : Worker, IWorker
    {
        private readonly WorkerASetting _settings;

        public WorkerA(HttpClient client, WorkerASetting settings) : base(client, settings)
        {
            _settings = settings;
        }

        public async Task Start()
        {
            Console.WriteLine($"Starting {DateTime.UtcNow}");
            
        }
    }
}
