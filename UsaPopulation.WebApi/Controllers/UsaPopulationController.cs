using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UsaPopulation.Domain.Interactors;
using UsaPopulation.Domain.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using static UsaPopulation.Domain.Utils;

namespace UsaPopulation.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsaPopulationController : ControllerBase
    {
        private readonly UsaPopulationInteractor _populationInteractor;
        private readonly QueryLogInteractor _queryLogInteractor;

        public UsaPopulationController(UsaPopulationInteractor populationInteractor, QueryLogInteractor queryLogInteractor)
        {
            _populationInteractor = populationInteractor;
            _queryLogInteractor = queryLogInteractor;
        }

        private void SaveQuery(PathString path, HttpRequest request)
        {
            string endpoint = path.ToString().Split('/')[2];
            string pathAndQuery = request.GetEncodedPathAndQuery();

            _queryLogInteractor.AddQueryLog(DateTime.Now, endpoint, pathAndQuery);
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
        public ActionResult<List<QueryLogOutputModel>> GetQueries(int pageSize, int pageIndex)
        {
            try
            {
                List<QueryLogOutputModel> outputModels = _queryLogInteractor.FetchQueries(pageSize, pageIndex);
                SaveQuery(HttpContext.Request.Path, HttpContext.Request);

                return outputModels;
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
