using System;
using UsaPopulation.Domain.Interactors;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Tests.Unit.FakeRepositories;
using Xunit;
using System.Collections.Generic;
using UsaPopulation.Domain.Models;

namespace UsaPopulation.Domain.Tests.Unit
{
    public class QueryLogInteractorTests
    {
        [Fact]
        public void AddQueryLog_AddingQueryLog_WasAdded()
        {
            //Arrange
            FakeQueryLogRepository queryLogRepository = new();
            QueryLogInteractor queryLogInteractor = new(queryLogRepository);
            DateTime dateTime = DateTime.Now;
            const string endpoint = "populationsDiff";
            const string pathAndQuery = "/UsaPopulation/populationsDiff?stateA=Montana&stateB=Texas&year=2014";

            //Act
            queryLogInteractor.AddQueryLog(dateTime, endpoint, pathAndQuery);

            //Assert
            Assert.Single(queryLogRepository.QueryLogs);
            Assert.Equal(dateTime, queryLogRepository.QueryLogs[0].DateTime);
            Assert.Equal(endpoint, queryLogRepository.QueryLogs[0].Endpoint);
            Assert.Equal(pathAndQuery, queryLogRepository.QueryLogs[0].PathAndQuery);
        }

        [Fact]
        public void FetchQueries_NormalDemand_ReturnCorrectSet()
        {
            //Arrange
            FakeQueryLogRepository queryLogRepository = new();
            QueryLogInteractor queryLogInteractor = new(queryLogRepository);
            const int pageSize = 2;
            const int pageIndex = 1;
            const string hotEndpoint = "C";
            const string hotEndpointAlso = "D";

            queryLogRepository.QueryLogs.AddRange(new List<QueryLog>()
            {
                new QueryLog() { Endpoint = "A" },
                new QueryLog() { Endpoint = "B" },
                new QueryLog() { Endpoint = hotEndpoint },
                new QueryLog() { Endpoint = hotEndpointAlso }
            });

            //Act
            List<QueryLogOutputModel> outputModels = queryLogInteractor.FetchQueries(pageSize, pageIndex);

            //Assert
            Assert.Equal(2, outputModels.Count);
            Assert.Equal(hotEndpoint, outputModels[0].Endpoint);
            Assert.Equal(hotEndpointAlso, outputModels[1].Endpoint);
        }

        [Fact]
        public void FetchQueries_NegativeNumber_ThrowsException()
        {
            //Arrange
            FakeQueryLogRepository queryLogRepository = new();
            QueryLogInteractor queryLogInteractor = new(queryLogRepository);
            const int pageSize = -1;
            const int pageIndex = 1;

            //Act & Assert
            Assert.Throws<InvalidNumberException>(() => queryLogInteractor.FetchQueries(pageSize, pageIndex));
        }

        [Fact]
        public void FetchQueries_DemandingTooLargeSet_ReturnEmptySet()
        {
            //Arrange
            FakeQueryLogRepository queryLogRepository = new();
            QueryLogInteractor queryLogInteractor = new(queryLogRepository);
            const int pageSize = 5;
            const int pageIndex = 1;

            queryLogRepository.QueryLogs.AddRange(new List<QueryLog>() { new QueryLog(), new QueryLog(), new QueryLog(), new QueryLog() });

            //Act
            List<QueryLogOutputModel> outputModels = queryLogInteractor.FetchQueries(pageSize, pageIndex);

            //Assert
            Assert.Empty(outputModels);
        }
    }
}
