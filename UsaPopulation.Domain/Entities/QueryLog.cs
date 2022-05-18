using System;

namespace UsaPopulation.Domain.Entities
{
    public class QueryLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Endpoint { get; set; }
        public string PathAndQuery { get; set; }
    }
}
