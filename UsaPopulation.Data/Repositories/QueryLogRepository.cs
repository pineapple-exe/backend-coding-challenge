using System.Linq;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Repositories;

namespace UsaPopulation.Data.Repositories
{
    public class QueryLogRepository : IQueryLogRepository
    {
        private readonly UsaPopulationDbContext _context;

        public QueryLogRepository(UsaPopulationDbContext context)
        {
            _context = context;
        }

        public void AddQueryLog(QueryLog queryLog)
        {
            _context.Add(queryLog);
            _context.SaveChanges();
        }

        public IQueryable<QueryLog> GetAllQueryLogs()
        {
            return _context.QueryLogs;
        }
    }
}
