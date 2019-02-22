using BookStoreExample.APIs;
using BookStoreExample.Models;
using BookStoreExample.Repository.Contracts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class Tests
    {
        private Mock<IBookRepository> bookRepoMock;
        private Mock<IBookRepositoryAsync> bookAsyncRepoMock;
        private Mock<ILoggerFactory> loggerMock;

        [SetUp]
        public void Setup()
        {
            bookRepoMock= new Moq.Mock<IBookRepository>();
            bookAsyncRepoMock= new Moq.Mock<IBookRepositoryAsync>();
            loggerMock = new Mock<ILoggerFactory>();
        }

        [Test]
        public void TestSyncMethod()
        {
            //Arrange 
            //GetTestBooks return some Test BooksDto List 
            //We set repo mock to return the test data.
            bookRepoMock.Setup(i => i.GetBooks()).Returns(GetTestBooks());
            var booksRepo = bookRepoMock.Object;
            var logger = loggerMock.Object;
            var booksController = new BookApiController(booksRepo, logger);

            //Act
            var result = booksController.GetBooks();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okData = result as OkObjectResult;
            Assert.IsInstanceOf<List<BookDto>>(okData.Value);
            var list = okData.Value as List<BookDto>;
            Assert.AreEqual(1, list.Count);
        }

        
        [Test]
        public async Task TestAsyncMethod()
        {
            //Arrange 
            //GetTestBooks return some Test BooksDto List 
            //We set repo mock to return the test data.
            //NOTE: When we setup mock we Use Task.FromResult instead of plain List
            bookAsyncRepoMock.Setup(i => i.GetBooksAsync()).Returns(Task.FromResult(GetTestBooks()));
            var booksRepoAsync = bookAsyncRepoMock.Object;
            var logger = loggerMock.Object;
            var booksController = new BookApiControllerAsync(booksRepoAsync, logger);

            //Act
            //NOTE:We await the Controller Action Call
            var result =await booksController.GetBooks();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okData = result as OkObjectResult;
            Assert.IsInstanceOf<List<BookDto>>(okData.Value);
            var list = okData.Value as List<BookDto>;
            Assert.AreEqual(1, list.Count);
        }
        
        #region Data Generator Method

        private List<BookDto> GetTestBooks()
        {
            return new List<BookDto>
            {
                new BookDto
                {
                    Publisher = new Publisher
                    {
                        Id = 1,
                        Name = "TestPublisher"
                    },
                    Genre = new List<Genre>
                    {
                        new Genre
                        {
                            Name = "C#",
                            Id = 1,
                        }
                    },
                    Authors = new List<Author>
                    {
                        new Author
                        {
                            Id = 1,
                            FirstName = "TestFirstName",
                            LastName = "TestLastName"
                        }
                    },
                    Name = "TestBookName",
                    Id = 1,
                    Edition = "First Test Edition"
                }
            };
        }


        #endregion


    }
}