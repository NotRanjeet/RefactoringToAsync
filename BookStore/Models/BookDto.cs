using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreExample.Models
{
    public class BookDto
    {
        public int Id { get;set; }
        public string Name { get; set; }

        public string Edition { get; set; }

        public List<Author> Authors { get; set; }
        public List<Genre> Genre { get; set; }
        public Publisher Publisher { get; set; }
    }
}
