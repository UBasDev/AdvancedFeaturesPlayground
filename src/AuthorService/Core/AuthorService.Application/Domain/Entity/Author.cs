using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application.Domain.Entity
{
    public class Author
    {
        public long Id { get; set; }
        public string AuthorName { get; set; } = "";
        public int Age { get; set; } = 0;
    }
}
