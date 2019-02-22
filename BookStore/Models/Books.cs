using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreExample.Models
{
    public class Book
    {
        public int Id { get;set; }
        public string Name { get; set; }

        public string Edition { get; set; }
        public string Foreword { get; set; }

        public virtual Publisher Publisher { get;set; }
        public virtual ICollection<BookGenre> Genres { get; set; }
        public virtual ICollection<BookAuthors> Authors { get; set; }
        
    }
}
