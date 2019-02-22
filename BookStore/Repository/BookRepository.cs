using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreExample.Models;
using BookStoreExample.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStoreExample.Repository
{
    /// <summary>
    /// Repository to Read/Write/Update data related to Books in the database.
    /// </summary>
    public class BookRepository : IBookRepository
    {

        private readonly BookStoreDbContext _context;
        private readonly ILogger _logger;

        public BookRepository(BookStoreDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger("BookRepository");
        }

        #region Get Methods

        public List<BookDto> GetBooks()
        {
            //Do not include like this in you production code 
            //Instead create DTOs and Use Select to get selected fields
            return _context.Books
                           .Select(i => new BookDto
                            {
                                Publisher = i.Publisher,
                                Genre = i.Genres.Select(g => g.Genre).ToList(),
                                Authors = i.Authors.Select(a => a.Author).ToList(),
                                Id = i.Id,
                                Name = i.Name,
                                Edition = i.Edition,
                           })
                           .OrderBy(i => i.Id)
                           .ToList();
        }

        public PagingResult<BookDto> GetBooksPage(int skip, int take)
        {
            var totalRecords = _context.Books.Count();
            var books = _context.Books
                .Select(i => new BookDto
                {
                    Publisher = i.Publisher,
                    Genre = i.Genres.Select(g => g.Genre).ToList(),
                    Authors = i.Authors.Select(a => a.Author).ToList(),
                    Id = i.Id,
                    Name = i.Name,
                    Edition = i.Edition,
                })
                .OrderBy(i => i.Id)
                .Skip(skip)
                .Take(take)
                .ToList();
            return new PagingResult<BookDto>(books, totalRecords);
        }

        public Book GetBook(int bookId)
        {
            return _context.Books.SingleOrDefault(i => i.Id == bookId);
        }

        #endregion

        #region Create/Update Method

        public Book InsertBook(Book book)
        {
            try
            {
                _context.Books.Add(book);
                var inserts = _context.SaveChanges();
                if (inserts > 0)
                {
                    return book;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to insert book", e);
                return null;
            }
        }

        #endregion
    }
}
