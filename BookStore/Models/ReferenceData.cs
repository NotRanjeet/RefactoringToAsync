using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreExample.Models
{
    public class ReferenceData
    {
        public List<Author> Authors { get; set; }

        public List<Genre> Genres { get; set; }

        public List<Publisher> Publishers { get; set; }
    }
}
