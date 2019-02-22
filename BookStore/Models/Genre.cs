using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreExample.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<BookGenre> Books { get;set; }
    }
}
