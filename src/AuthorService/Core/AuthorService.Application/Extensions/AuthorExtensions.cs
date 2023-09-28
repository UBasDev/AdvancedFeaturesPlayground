using AuthorService.Application.Models.Requests;
using AuthorService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application.Extensions
{
    public static class AuthorExtensions
    {
        public static Author CreateSingleAuthorRequestToAuthorMapper(this Author author, CreateSingleAuthorRequest createSingleAuthorRequest)
        {
            author.AuthorName = createSingleAuthorRequest.AuthorName;
            author.Age = createSingleAuthorRequest.Age;
            return author;
        }
    }
}
