using System;
using System.Linq;
using UsaPopulation.Domain.Entities;

namespace UsaPopulation.Domain.Repositories
{
    public interface IQueryLogRepository
    {
        void AddQueryLog(QueryLog queryLog);
        IQueryable<QueryLog> GetAllQueryLogs();
    }
}
