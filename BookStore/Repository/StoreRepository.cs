using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookStoreExample.Models;
using BookStoreExample.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStoreExample.Repository
{
    /// <summary>
    /// This repo is intended for system level operations
    /// For example load all the reference data for caching at Front End
    /// </summary>
    public class StoreRepository: IStoreRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly ILogger _logger;

        public StoreRepository(BookStoreDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger(nameof(StoreRepository));
        }

        /// <summary>
        /// Load all the reference data to cache at Front End 
        /// </summary>
        /// <returns></returns>
        public ReferenceData GetReferenceData()
        {
            try
            {
                var authors = _context.Authors.ToList();
                var genres = _context.Genres.ToList();
                var publishers = _context.Publishers.ToList();
                return new ReferenceData
                {
                    Authors = authors,
                    Genres = genres,
                    Publishers = publishers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        
    }
}
