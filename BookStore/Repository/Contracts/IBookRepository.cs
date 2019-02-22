using System.Collections.Generic;
using BookStoreExample.Models;

namespace BookStoreExample.Repository.Contracts
{
    public interface IBookRepository
    {
        List<BookDto> GetBooks();
        PagingResult<BookDto> GetBooksPage(int skip, int take);
        Book GetBook(int bookId);
        Book InsertBook(Book book);
    }
}
