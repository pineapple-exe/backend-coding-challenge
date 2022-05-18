using System;

namespace UsaPopulation.Domain.Models
{
    public class QueryLogOutputModel
    {
        public DateTime DateTime { get; }
        public string Endpoint { get; }
        public string PathAndQuery { get; }

        public QueryLogOutputModel(DateTime dateTime, string endpoint, string pathAndQuery)
        {
            DateTime = dateTime;
            Endpoint = endpoint;
            PathAndQuery = pathAndQuery;
        }
    }
}
