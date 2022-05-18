using System.Collections.Generic;
using System.Linq;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Repositories;

namespace UsaPopulation.Domain.Tests.Unit.FakeRepositories
{
    internal class FakeQueryLogRepository : IQueryLogRepository
    {
        internal List<QueryLog> QueryLogs = new();

        public void AddQueryLog(QueryLog queryLog)
        {
            QueryLogs.Add(queryLog);
        }

        public IQueryable<QueryLog> GetAllQueryLogs()
        {
            return QueryLogs.AsQueryable();
        }
    }
}
