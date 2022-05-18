using System.Collections.Generic;
using System.Threading.Tasks;
using UsaPopulation.Domain.Entities;

namespace UsaPopulation.Domain.Repositories
{
    public interface IDataUsaRepository
    {
        Task<DataUsa> Request(int? year);
        Task ExtractState(string state);
    }
}
