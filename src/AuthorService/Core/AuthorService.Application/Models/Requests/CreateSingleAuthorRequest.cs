using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application.Models.Requests
{
    public class CreateSingleAuthorRequest
    {
        public string AuthorName { get; set; } = "";
        public int Age { get; set; } = 0;
    }
}
