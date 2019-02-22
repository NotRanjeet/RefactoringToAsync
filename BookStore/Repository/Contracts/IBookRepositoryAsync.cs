using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreExample.Models;

namespace BookStoreExample.Repository.Contracts
{
    public interface IBookRepositoryAsync
    {
        Task<List<BookDto>> GetBooksAsync();
        Task<PagingResult<BookDto>> GetBooksPageAsync(int skip, int take);
        Task<Book> GetBookAsync(int bookId);
        Task<Book> InsertBookAsync(Book book);
    }
}
