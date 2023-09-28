using AuthorService.Application.Models.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application.Validations.Author
{
    public class CreateSingleAuthorRequestValidation : AbstractValidator<CreateSingleAuthorRequest>
    {
        public CreateSingleAuthorRequestValidation()
        {
            RuleFor(r => r.AuthorName).NotEmpty().WithMessage("AuthorName can not be empty");
            RuleFor(r => r.Age).LessThanOrEqualTo(99).GreaterThanOrEqualTo(1).WithMessage("Author age should be between 1 and 99");

        }
    }
}
