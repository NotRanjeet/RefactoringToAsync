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
    public class StoreRepositoryAsync : IStoreRepositoryAsync
    {

        private readonly BookStoreDbContext _context;
        private readonly ILogger _logger;

        public StoreRepositoryAsync(BookStoreDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger(nameof(StoreRepository));
        }

        /// <summary>
        /// Load all the reference data to cache at Front End 
        /// </summary>
        /// <returns></returns>
        public async Task<ReferenceData> GetReferenceDataAsync()
        {
            try
            {

                var authors = _context.Authors.ToListAsync();
                //We start Genre Database fetch before we get the result for Authors
                var genres = _context.Genres.ToListAsync();
                //We start Publishers Database fetch before we get the result for Genres
                var publishers = _context.Publishers.ToListAsync();
                //So instead of executing these queries in serial we can trigger them in parallel
                await Task.WhenAll(authors, genres, publishers);
                return new ReferenceData
                {
                    Authors = authors.Result,
                    Genres = genres.Result,
                    Publishers = publishers.Result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
