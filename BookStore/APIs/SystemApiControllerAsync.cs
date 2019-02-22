using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreExample.Repository;
using BookStoreExample.Repository.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStoreExample.APIs
{
    [Route("api/async/system")]
    [EnableCors("AllowAnyOrigin")]
    public class SystemApiControllerAsync : Controller
    {
        private readonly IStoreRepositoryAsync _storeRepository;
        private readonly ILogger _logger;

        public SystemApiControllerAsync(IStoreRepositoryAsync repo, ILoggerFactory loggerFactory)
        {
            _storeRepository = repo;
            _logger = loggerFactory.CreateLogger(nameof(SystemApiController));
        }
        
        //GET api/async/system/data
        [HttpGet]
        [Route("data")]
        public async Task<ActionResult> ReferenceData()
        {
            try
            {
                var data =await _storeRepository.GetReferenceDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new {Status = false});
            }
        }


    }
}
