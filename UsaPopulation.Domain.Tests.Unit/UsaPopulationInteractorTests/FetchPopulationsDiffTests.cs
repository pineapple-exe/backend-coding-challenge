using System.Collections.Generic;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Interactors;
using UsaPopulation.Domain.Models;
using UsaPopulation.Domain.Tests.Unit.FakeRepositories;
using Xunit;

namespace UsaPopulation.Domain.Tests.Unit.UsaPopulationInteractorTests
{
    public class FetchPopulationsDiffTests
    {
        [Fact]
        public async void FetchPopulationsDiff_GeneralRequest_CorrectDiff()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string stateA = "Missouri";
            const string stateB = "Mississippi";
            const int populationA = 10;
            const int populationB = 20;

            dataUsaRepository.DataUsa.Data = new List<DataUsaState>();
            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>
            {
                new DataUsaState() { State = stateA, Population = populationA, Year = 2022 },
                new DataUsaState() { State = stateB, Population = populationB, Year = 2022 },
                new DataUsaState() { State = "Massachusetts", Population = 30, Year = 2022 }
            });

            //Act
            List<PopulationsDiffOutputModel> outputModels = await interactor.FetchPopulationsDiff(stateA, stateB);

            //Assert
            Assert.Single(outputModels);
            Assert.Equal(populationA - populationB, outputModels[0].Diff);
        }

        [Fact]
        public async void FetchPopulationsDiff_SpecificRequest_CorrectYear()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string stateA = "Missouri";
            const string stateB = "Mississippi";
            const int year = 2022;

            dataUsaRepository.DataUsa.Data = new List<DataUsaState>();
            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>
            {
                new DataUsaState() { State = stateA, Population = 5, Year = 2021 },
                new DataUsaState() { State = stateB, Population = 10, Year = 2021 },

                new DataUsaState() { State = stateA, Population = 10, Year = year },
                new DataUsaState() { State = stateB, Population = 20, Year = year },

                new DataUsaState() { State = stateA, Population = 20, Year = 2023 },
                new DataUsaState() { State = stateB, Population = 40, Year = 2023 }
            });

            //Act
            List<PopulationsDiffOutputModel> outputModels = await interactor.FetchPopulationsDiff(stateA, stateB, year);

            //Assert
            Assert.Single(outputModels);
            Assert.Equal(year, outputModels[0].Year);
        }

        [Fact]
        public async void FetchPopulationsDiff_GeneralRequestAgainstMultiYearData_MultipleElements()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string stateA = "Missouri";
            const string stateB = "Mississippi";

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>
            {
                new DataUsaState() { State = stateA, Population = 5, Year = 2021 },
                new DataUsaState() { State = stateB, Population = 10, Year = 2021 },

                new DataUsaState() { State = stateA, Population = 10, Year = 2022 },
                new DataUsaState() { State = stateB, Population = 20, Year = 2022 },

                new DataUsaState() { State = stateA, Population = 20, Year = 2023 },
                new DataUsaState() { State = stateB, Population = 40, Year = 2023 }
            });

            //Act
            List<PopulationsDiffOutputModel> outputModels = await interactor.FetchPopulationsDiff(stateA, stateB);

            //Assert
            Assert.Equal(3, outputModels.Count);
        }

        [Fact]
        public async void FetchPopulationsDiff_NonExistingState_ThrowsException()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const string nonExistingState = "Texmex";

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>
            {
                new DataUsaState() { State = "Texas", Population = 5, Year = 2222 },
                new DataUsaState() { State = "Iowa", Population = 10, Year = 2222 }
            });

            //Act & Assert
            await Assert.ThrowsAsync<InvalidStateException>(async () => await interactor.FetchPopulationsDiff(nonExistingState, "Iowa"));
        }

        [Fact]
        public async void FetchPopulationsDiff_NonExistingYear_ThrowsException()
        {
            //Arrange
            FakeDataUsaRepository dataUsaRepository = new();
            UsaPopulationInteractor interactor = new(dataUsaRepository);

            const int nonExistingYear = 2020;

            dataUsaRepository.DataUsa.Data.AddRange(new List<DataUsaState>
            {
                new DataUsaState() { State = "Florida", Population = 1000, Year = 2021 },
                new DataUsaState() { State = "Oklahoma", Population = 1000, Year = 2022 }
            });

            //Act & Assert
            await Assert.ThrowsAsync<InvalidYearException>(async () => 
                await interactor.FetchPopulationsDiff("Florida", "Oklahoma", nonExistingYear));
        }
    }
}
