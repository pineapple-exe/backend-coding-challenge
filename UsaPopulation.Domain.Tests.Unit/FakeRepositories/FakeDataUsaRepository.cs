using System.Collections.Generic;
using System.Threading.Tasks;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Repositories;

namespace UsaPopulation.Domain.Tests.Unit.FakeRepositories
{
    class FakeDataUsaRepository : IDataUsaRepository
    {
        public DataUsa DataUsa = new() { Data = new List<DataUsaState>() };

        public Task<DataUsa> Request(int? year)
        {
            if (year != null)
            {
                DataUsa.Data.RemoveAll(s => s.Year != year);
            }

            return Task.FromResult(DataUsa);
        }
    }
}
