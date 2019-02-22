using System;
using BookStoreExample.Repository;
using BookStoreExample.Repository.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace BookStoreExample.APIs
{
    [Route("api/async/books")]
    [EnableCors("AllowAnyOrigin")]
    public class BookApiControllerAsync : Controller
    {

        private readonly IBookRepositoryAsync _bookRepository;
        private readonly ILogger _logger;

        public BookApiControllerAsync(IBookRepositoryAsync repo, ILoggerFactory factory)
        {
            _bookRepository = repo;
            _logger = factory.CreateLogger(nameof(BookApiController));
        }

        //Get api/async/books
        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            try
            {
                var books = await _bookRepository.GetBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new { Status = false });
            }
        }

        //GET api/async/book/5
        [HttpGet("{id}", Name = "GetBookRouteAsync")]
        public async Task<ActionResult> GetCustomerId(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookAsync(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new {Status = false});
            }
        }

        //GET api/async/books/0/10
        [HttpGet("page/{skip}/{take}")]
        public async Task<ActionResult> CustomersPage(int skip, int take)
        {
            try
            {
                var pagingResult =await _bookRepository.GetBooksPageAsync(skip, take);
                Response.Headers.Add("X-InlineCount", pagingResult.TotalRecords.ToString());
                return Ok(pagingResult.Records);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(new { Status = false });
            }
        }


    }
}
