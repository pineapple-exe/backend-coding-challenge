using System.Collections.Generic;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Interactors;
using UsaPopulation.Domain.Models;
using UsaPopulation.Domain.Tests.Unit.FakeRepositories;
using Xunit;
using static UsaPopulation.Domain.Utils;

namespace UsaPopulation.Domain.Tests.Unit.UsaPopulationInteractorTests
{
    public class FetchBiggestOrSmallestTests
    {
        [Fact]
        public async void FetchBiggestOrSmallest_RequestingStateWithBiggestPopulation_ActuallyBiggest()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string hotName = "California";

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>()
            {
                new DataUsaState() { State = "Colorado", Population = 100 },
                new DataUsaState() { State = hotName, Population = 1000 },
                new DataUsaState() { State = "Connecticut", Population = 10 }
            });

            //Act
            StateOutputModel state = await interactor.FetchBiggestOrSmallest(SuperlativeSize.Biggest);

            //Assert
            Assert.Equal(hotName, state.Name);
        }
    }
}
