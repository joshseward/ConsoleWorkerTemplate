using ConsoleWorker.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleWorker
{
    public abstract class Worker
    {
        protected readonly AppSettings _appSettings;
        protected readonly HttpClient _client;

        public Worker(HttpClient client, AppSettings appSettings)
        {
            _client = client;
            _appSettings = appSettings;

            _client.DefaultRequestHeaders.Add("APIKEY", appSettings.ApiKey);
        }

        protected string HandleUrl(string url)
        {
            return new Uri(_appSettings.Api + url).ToString();
        }

        protected string GetJsonFile(string fileName)
        {
            using (StreamReader r = new StreamReader($"Json/{fileName}.json"))
            {
                return r.ReadToEnd();
            }
        }

        protected decimal GetHowManyToSend(int respondentCount, decimal percentage)
        {
            return Math.Round(Math.Abs(respondentCount / percentage * 100), 0);
        }

        protected ByteArrayContent GetByteArrayContent(string json)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.Add("Content-Type", "application/json");
            return byteContent;
        }       

        protected async Task<T> GetResultData<T>(HttpResponseMessage result)
        {
            var stringcontent = await result.Content.ReadAsStringAsync();

            return typeof(T) == typeof(string) ? (T)Convert.ChangeType(stringcontent, typeof(T)) :
                    JsonSerializer.Deserialize<T>(stringcontent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IgnoreNullValues = true });
        }
    }
}
