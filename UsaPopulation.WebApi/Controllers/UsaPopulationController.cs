using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UsaPopulation.Domain.Interactors;
using UsaPopulation.Domain.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using static UsaPopulation.Domain.Utils;

//Write a REST Api where you can query for for the difference between difference states population using: 
//    https://datausa.io/api/data?drilldowns=State&measures=Population&year=latest.

//Also save all queries made with datetime in a database and provide a way to access this information.

//Requirements: Use.NET and SQL Server

//Results:

//Rest API with routes for getting:
//* The difference between two specific states, filtered by year if chosen   [x]
//* The state with the biggest/smallest population                           [x]
//* 1 other interesting comparison/information from the dataset              [x]
//* Route for accessing logs of queries made to above route

namespace UsaPopulation.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsaPopulationController : ControllerBase
    {
        private readonly UsaPopulationInteractor _populationInteractor;

        public UsaPopulationController(UsaPopulationInteractor populationInteractor)
        {
            _populationInteractor = populationInteractor;
        }

        private void SaveQuery(PathString path, HttpRequest request)
        {
            string endpoint = path.ToString().Split('/')[2];
            string pathAndQuery = request.GetEncodedPathAndQuery();

            _populationInteractor.AddQueryLog(DateTime.Now, endpoint, pathAndQuery);
        }

        [HttpGet("populationsDiff")] // Describe Diff property
        public async Task<ActionResult<List<PopulationsDiffOutputModel>>> GetPopulationsDiff(string stateA, string stateB, int? year = null)
        {
            try 
            {
                List<PopulationsDiffOutputModel> outputModels = await _populationInteractor.FetchPopulationsDiff(stateA, stateB, year);
                SaveQuery(HttpContext.Request.Path, HttpContext.Request);

                return outputModels;
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("biggestSmallest")]
        public async Task<StateOutputModel> GetBiggestOrSmallestPopulationState(SuperlativeSize biggestOrSmallest)
        {
            SaveQuery(HttpContext.Request.Path, HttpContext.Request);

            return await _populationInteractor.FetchBiggestOrSmallest(biggestOrSmallest);
        }

        [HttpGet("populationProgression")]
        public async Task<ActionResult<List<PopulationProgressionOutputModel>>> GetPopulationProgression(string state, int fromYear, int toYear, bool incrementalSteps = false)
        {
            try
            {
                List<PopulationProgressionOutputModel> outputModels = await _populationInteractor.FetchPopulationProgression(state, fromYear, toYear, incrementalSteps);
                SaveQuery(HttpContext.Request.Path, HttpContext.Request);

                return outputModels;
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("queries")]
        public List<QueryLogOutputModel> GetQueries(int pageSize, int pageIndex)
        {
            return _populationInteractor.FetchQueries(pageSize, pageIndex);
        }
    }
}
