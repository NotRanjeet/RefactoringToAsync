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
    [Route("api/system")]
    [EnableCors("AllowAnyOrigin")]
    public class SystemApiController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger _logger;

        public SystemApiController(IStoreRepository repo, ILoggerFactory loggerFactory)
        {
            _storeRepository = repo;
            _logger = loggerFactory.CreateLogger(nameof(SystemApiController));
        }
        
        //GET api/system/data
        [HttpGet]
        [Route("data")]
        public ActionResult ReferenceData()
        {
            try
            {
                var data = _storeRepository.GetReferenceData();
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
