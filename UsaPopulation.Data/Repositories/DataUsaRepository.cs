using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Repositories;

namespace UsaPopulation.Data.Repositories
{
    public class DataUsaRepository : IDataUsaRepository
    {
        private readonly IHttpClientFactory _factory;
        private readonly Uri _base = new("https://datausa.io/api/data?drilldowns=State&measures=Population");
        public DataUsaRepository(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<DataUsa> Request(int? year)
        {
            using HttpClient webWizard = _factory.CreateClient();
            string query = year == null ? "&year=" + year.ToString() : "";

            HttpResponseMessage response = await webWizard.SendAsync(new HttpRequestMessage(HttpMethod.Get, _base + query));
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DataUsa>(content);
        }

        public async Task ExtractState(string state)
        {

        }
    }
}
