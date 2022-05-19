using System;
using System.Collections.Generic;
using System.Linq;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Models;
using UsaPopulation.Domain.Repositories;

namespace UsaPopulation.Domain.Interactors
{
    public class QueryLogInteractor
    {
        private readonly IQueryLogRepository _queryLogRepository;

        public QueryLogInteractor(IQueryLogRepository queryLogRepository)
        {
            _queryLogRepository = queryLogRepository;
        }

        public void AddQueryLog(DateTime receivedAt, string endpoint, string pathAndQuery)
        {
            _queryLogRepository.AddQueryLog(new QueryLog { DateTime = receivedAt, Endpoint = endpoint, PathAndQuery = pathAndQuery });
        }

        private static bool QueriesExist(int pageSize, int pageIndex, IQueryable<QueryLog> allQueries)
        {
            return allQueries.Count() >= (pageSize * pageIndex + pageSize);
        }

        public List<QueryLogOutputModel> FetchQueries(int pageSize, int pageIndex)
        {
            if (pageSize <= 0 || pageIndex < 0)
                throw new InvalidNumberException("Number was too low.");

            IQueryable<QueryLog> allQueries = _queryLogRepository.GetAllQueryLogs();
            List<QueryLogOutputModel> outputModels = new();

            if (QueriesExist(pageSize, pageIndex, allQueries))
            {
                IQueryable<QueryLog> queryPage = allQueries.OrderBy(q => q.DateTime).Skip(pageSize * pageIndex).Take(pageSize);

                foreach (QueryLog q in queryPage)
                {
                    outputModels.Add(new QueryLogOutputModel(q.DateTime, q.Endpoint, q.PathAndQuery));
                }
            }

            return outputModels;
        }
    }
}
