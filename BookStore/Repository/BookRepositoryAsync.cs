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
    public class BookRepositoryAsync : IBookRepositoryAsync
    {

        private readonly BookStoreDbContext _context;
        private readonly ILogger _logger;

        public BookRepositoryAsync(BookStoreDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger(nameof(BookRepositoryAsync));
        }

        #region Get Methods

        public async Task<List<BookDto>> GetBooksAsync()
        {
            //Do not include like this in you production code 
            //Instead create DTOs and Use Select to get selected fields
            return await _context.Books
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
                                 .ToListAsync();
        }

        public async Task<PagingResult<BookDto>> GetBooksPageAsync(int skip, int take)
        {
            var totalRecords = await _context.Books.CountAsync();
            var books = await _context.Books
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
                .ToListAsync();

            return new PagingResult<BookDto>(books, totalRecords);
        }

        public async Task<Book> GetBookAsync(int bookId)
        {
            return await _context.Books.SingleOrDefaultAsync(i => i.Id == bookId);
        }

        #endregion

        #region Create/Update Method

        public async Task<Book> InsertBookAsync(Book book)
        {
            try
            {
                _context.Books.Add(book);
                var inserts = await _context.SaveChangesAsync();
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
