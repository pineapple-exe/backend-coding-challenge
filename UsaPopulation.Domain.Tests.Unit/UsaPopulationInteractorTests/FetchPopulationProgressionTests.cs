using System.Collections.Generic;
using System.Linq;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Interactors;
using UsaPopulation.Domain.Tests.Unit.FakeRepositories;
using Xunit;

namespace UsaPopulation.Domain.Tests.Unit.UsaPopulationInteractorTests
{
    public class FetchPopulationProgressionTests
    {
        [Fact]
        public async void FetchPopulationProgression_GeneralRequest_CorrectProgression()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string state = "California";
            const int populationFrom = 100;
            const int populationTo = 300;
            const int fromYear = 2020;
            const int toYear = 2022;

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>()
            {
                new DataUsaState() { State = state, Population = populationFrom, Year = fromYear },
                new DataUsaState() { State = state, Population = 200, Year = 2020 },
                new DataUsaState() { State = state, Population = populationTo, Year = toYear }
            });

            //Act
            var progressionOutputModels = await interactor.FetchPopulationProgression(state, fromYear, toYear);

            //Assert
            Assert.Single(progressionOutputModels);
            Assert.Equal(populationTo - populationFrom, progressionOutputModels[0].YearlyIncrement);
            Assert.Equal(populationTo - populationFrom, progressionOutputModels[0].PercentChange);
        }

        [Fact]
        public async void FetchPopulationProgression_ComplexRequest_MultipleElements()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string state = "California";
            const int populationFrom = 100;
            const int populationTo = 300;
            const int fromYear = 2020;
            const int toYear = 2022;

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>()
            {
                new DataUsaState() { State = state, Population = populationFrom, Year = fromYear },
                new DataUsaState() { State = state, Population = 200, Year = 2020 },
                new DataUsaState() { State = state, Population = populationTo, Year = toYear }
            });

            //Act
            var progressionOutputModels = await interactor.FetchPopulationProgression(state, fromYear, toYear, true);

            //Assert
            Assert.Equal(2, progressionOutputModels.Count);
        }

        [Fact]
        public async void FetchPopulationProgression_NonExistingStateName_ThrowsException()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string nonExistingState = "Sweet home";

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>()
            {
                new DataUsaState() { State = "Alabama", Population = 777, Year = 1993 },
                new DataUsaState() { State = "Alabama", Population = 888, Year = 1994 }
            });

            //Act & Assert
            await Assert.ThrowsAsync<InvalidStateException>(async () => await interactor.FetchPopulationProgression(nonExistingState, 1993, 1994));
        }

        [Fact]
        public async void FetchPopulationProgression_NonExistingYear_ThrowsException()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string perfectlyExistingState = "Alabama";
            const int nonExistingYear = 666;

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>()
            {
                new DataUsaState() { State = "Alabama", Population = 777, Year = 1993 },
                new DataUsaState() { State = "Alabama", Population = 888, Year = 1994 }
            });

            //Act & Assert
            await Assert.ThrowsAsync<InvalidYearException>(
                async () => await interactor.FetchPopulationProgression(perfectlyExistingState, nonExistingYear, 1994)
            );
        }

        [Fact]
        public async void FetchPopulationProgression_YearsInReversedOrder_ThrowsException()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const int firstYear = 1993;
            const int nextYear = 1994;

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>()
            {
                new DataUsaState() { State = "Alabama", Population = 777, Year = firstYear },
                new DataUsaState() { State = "Alabama", Population = 888, Year = nextYear }
            });

            //Act & Assert
            await Assert.ThrowsAsync<InvalidYearException>(async () => 
                await interactor.FetchPopulationProgression("Alabama", nextYear, firstYear));
        }
    }
}
