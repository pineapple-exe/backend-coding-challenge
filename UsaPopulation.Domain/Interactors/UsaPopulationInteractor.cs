using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain.Models;
using UsaPopulation.Domain.Repositories;
using static UsaPopulation.Domain.Utils;

namespace UsaPopulation.Domain.Interactors
{
    public class UsaPopulationInteractor
    {
        private readonly IDataUsaRepository _dataUsaRepository;

        public UsaPopulationInteractor(IDataUsaRepository dataUsaRepository)
        {
            _dataUsaRepository = dataUsaRepository;
        }

        private async Task<bool> StateExists(string state)
        {
            if (state == null) return false;

            DataUsa dataUsa = await _dataUsaRepository.Request(null);
            IEnumerable<string> existingStates = dataUsa.Data.Select(s => s.State.ToLower()).Distinct();

            return existingStates.Contains(state.ToLower());
        }

        private async Task<bool> YearExists(int? year)
        {
            DataUsa dataUsa = await _dataUsaRepository.Request(null);
            IEnumerable<int> existingYears = dataUsa.Data.Select(s => s.Year).Distinct();

            return existingYears.Any(y => y == year);
        }

        public async Task<List<PopulationsDiffOutputModel>> FetchPopulationsDiff(string stateA, string stateB, int? year = null)
        {
            if (!(await StateExists(stateA) && await StateExists(stateB)))
                throw new InvalidStateException("Invalid name of state.");

            if (year != null) { 
                if (!await YearExists(year))
                    throw new InvalidYearException("Year does not exist.");
            }

            DataUsa dataUsa = await _dataUsaRepository.Request(year);
            List<PopulationsDiffOutputModel> populationsDiffOutputModels = new();
            IEnumerable<int> everyYear = dataUsa.Data.Select(s => s.Year).Distinct();

            foreach (int y in everyYear)
            {
                DataUsaState specificYearStateA = dataUsa.Data.First(s => s.State.ToLower() == stateA.ToLower() && s.Year == y);
                DataUsaState specificYearStateB = dataUsa.Data.First(s => s.State.ToLower() == stateB.ToLower() && s.Year == y);
                int populationsDiff = specificYearStateA.Population - specificYearStateB.Population;

                populationsDiffOutputModels.Add(new PopulationsDiffOutputModel(stateA, stateB, populationsDiff, y));
            }

            return populationsDiffOutputModels;
        }

        public async Task<StateOutputModel> FetchBiggestOrSmallest(SuperlativeSize biggestOrSmallest)
        {
            DataUsa dataUsa = await _dataUsaRepository.Request(null);
            IEnumerable<DataUsaState> statesLatestYear = dataUsa.Data.Where(s => s.Year == dataUsa.Data.Max(s => s.Year));
            DataUsaState superlativeState;

            if (biggestOrSmallest == SuperlativeSize.Biggest)
            {
                superlativeState = statesLatestYear.First(s => s.Population == statesLatestYear.Max(s => s.Population));
            }
            else
            {
                superlativeState = statesLatestYear.First(s => s.Population == statesLatestYear.Min(s => s.Population));
            }

            return new StateOutputModel(superlativeState.State, superlativeState.Population);
        }

        public async Task<List<PopulationProgressionOutputModel>> FetchPopulationProgression(string state, int fromYear, int toYear, bool incrementalStep = false)
        {
            if (!await StateExists(state))
                throw new InvalidStateException("Invalid name of state.");

            if (!(await YearExists(fromYear) && await YearExists(toYear)))
                throw new InvalidYearException("Year does not exist.");

            if (fromYear > toYear)
                throw new InvalidYearException("From-year needs to precede to-year.");

            List<PopulationProgressionOutputModel> outputModels = new();
            DataUsa dataUsa = await _dataUsaRepository.Request(null);
            var stateEveryYear = dataUsa.Data.Where(s => s.State.ToLower() == state.ToLower()).ToList();

            if (incrementalStep)
            {
                stateEveryYear = stateEveryYear.OrderBy(s => s.Year).ToList();
                
                for (int i = 1; i < stateEveryYear.Count; i++)
                {
                    int yearlyIncrement = stateEveryYear[i].Population - stateEveryYear[i - 1].Population;
                    double yearlyIncrementDecimal = yearlyIncrement / (double)stateEveryYear[i - 1].Population;
                    double yearlyIncrementPercent = Math.Round(100 * yearlyIncrementDecimal, 2);

                    outputModels.Add(new PopulationProgressionOutputModel(stateEveryYear[i].Year, yearlyIncrement, yearlyIncrementPercent));
                }
            }
            else
            {
                DataUsaState atFromYear = stateEveryYear.First(s => s.Year == fromYear);
                DataUsaState atToYear = stateEveryYear.First(s => s.Year == toYear);

                int yearlyIncrement = atToYear.Population - atFromYear.Population;
                double yearlyIncrementDecimal = yearlyIncrement / (double)atFromYear.Population;
                double yearlyIncrementPercent = Math.Round(100 * yearlyIncrementDecimal, 2);

                outputModels.Add(new PopulationProgressionOutputModel(toYear, yearlyIncrement, yearlyIncrementPercent));
            }

            return outputModels;
        }
    }
}
