using System;
using BookStoreExample.Repository;
using BookStoreExample.Repository.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStoreExample.APIs
{
    [Route("api/books")]
    [EnableCors("AllowAnyOrigin")]
    public class BookApiController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly ILogger _logger;

        public BookApiController(IBookRepository repo, ILoggerFactory factory)
        {
            _bookRepository = repo;
            _logger = factory.CreateLogger(nameof(BookApiController));
        }

        //Get api/books
        [HttpGet]
        public ActionResult GetBooks()
        {
            try
            {
                var books = _bookRepository.GetBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new { Status = false });
            }
        }

        //GET api/book/5
        [HttpGet("{id}", Name = "GetBookRoute")]
        public ActionResult GetCustomerId(int id)
        {
            try
            {
                var book = _bookRepository.GetBook(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new {Status = false});
            }
        }

        //GET api/books/0/10
        [HttpGet("page/{skip}/{take}")]
        public ActionResult CustomersPage(int skip, int take)
        {
            try
            {
                var pagingResult = _bookRepository.GetBooksPage(skip, take);
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
