using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookStoreExample.Repository
{
    public class CustomersDbSeeder
    {
        readonly ILogger _Logger;

        public CustomersDbSeeder(ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger("BookStoreSeederLogger");
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var storeDb = serviceScope.ServiceProvider.GetService<BookStoreDbContext>();
                if (await storeDb.Database.EnsureCreatedAsync())
                {
                    if (!await storeDb.Books.AnyAsync())
                    {
                        await InsertSampleData(storeDb);
                    }
                }
            }
        }

        public async Task InsertSampleData(BookStoreDbContext db)
        {
            var authors = new List<Author>
            {
                new Author{Id=1, FirstName = "Captain", LastName = "Marvel", Country = "US"},
                new Author{Id=2, FirstName = "Captain", LastName = "America", Country = "US"},
                new Author{Id=3, FirstName = "Nick", LastName = "Fury", Country = "UK"},
                new Author{Id=4, FirstName = "Ant", LastName = "Man", Country = "UK"},
                new Author{Id=5, FirstName = "Chris", LastName = "Hemworth", Country = "Greece"},
                new Author{Id=6, FirstName = "Bruce", LastName = "Wayne", Country = "Secret"},
                new Author{Id=7, FirstName = "Black", LastName = "Panther", Country = "Wakanda"},
                new Author{Id=8, FirstName = "Iron", LastName = "Man", Country = "Titan"},

            };
            var publishers = new List<Publisher>
            {
                new Publisher{Id=1, Name = "OReilly"},
                new Publisher{Id=2, Name = "Penguin Books"},
                new Publisher{Id=3, Name = "APress"},
                new Publisher{Id=4, Name = "Microsoft Press"},
                new Publisher{Id=5, Name = "Wrox Publications"}
            };

            var generes = new List<Genre>
            {
                new Genre{Id=1,Name = "C#"},
                new Genre{Id=2,Name = ".Net"},
                new Genre{Id=3,Name = ".Net Core"},
                new Genre{Id=4,Name = "Azure"},
                new Genre{Id=5,Name="Python"},
                new Genre{Id=6,Name = "Java"},
                new Genre{Id=7,Name = "SQL"},
                new Genre{Id=8,Name= "NoSql DBs"},
                new Genre{Id=9,Name= "ReactJS"},

            };
            db.Authors.AddRange(authors);
            db.Publishers.AddRange(publishers);
            db.Genres.AddRange(generes);

            var books = GetBooksData(db).ToList();
            db.Books.AddRange(books);
            try
            {
                int numAffected = await db.SaveChangesAsync();
                _Logger.LogInformation($"Saved {numAffected} customers");
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(CustomersDbSeeder)}: " + exp.Message);
                throw;
            }

        }

        private IEnumerable<Book> GetBooksData(BookStoreDbContext context)
        {
            for (int i = 1; i < 25; i++)
            {
                yield return new Book
                {
                    Id = i,
                    Name = $"Title For Book {i}",
                    Publisher = GetPublisher(context),
                    Authors = GetAuthor(context, 2).Distinct().Select(author=>new BookAuthors{AuthorId = author.Id,BookId = i}).ToList(),
                    Edition = "Edition One",
                    Foreword = null,
                    Genres = GetGenres(context, 2).Distinct().Select(genre => new BookGenre{GenreId = genre.Id, BookId = i}).ToList(),
                };
            }
        }


        private Publisher GetPublisher(BookStoreDbContext context)
        {
            
            var random = new Random();
            var index = random.Next(1, context.Publishers.Local.Count());
            return context.Publishers.Local.FirstOrDefault(i => i.Id == index);
        }


        /// <summary>
        /// Get random author object
        /// </summary>
        /// <param name="count">Numbers of items request</param>
        /// <returns></returns>
        private IEnumerable<Author> GetAuthor(BookStoreDbContext context, int count = 1)
        {
           
            var random = new Random();
            var max = context.Authors.Local.Count;
            for (int i = 1; i <= count; i++)
            {
                var index = random.Next(1, max);
                yield return context.Authors.Local.FirstOrDefault(j => j.Id == index);
            }
        }


        /// <summary>
        /// Get Random Genres 
        /// </summary>
        /// <param name="count">No of items request</param>
        /// <returns></returns>
        private IEnumerable<Genre> GetGenres(BookStoreDbContext context, int count = 1)
        {
            
            var random = new Random();
            var max = context.Genres.Local.Count;
            for (int i = 1; i <= count; i++)
            {
                var index = random.Next(1, max);
                yield return context.Genres.Local.FirstOrDefault(j => j.Id == index);
            }
        }

    }
}
