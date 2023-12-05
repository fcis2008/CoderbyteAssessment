using BusinessLogicLayer.DTO;
using FluentValidation;

namespace WebAPI.Validation
{
    public class UserValidator : AbstractValidator<NewUserDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Please specify your first name.").Length(1, 150).WithMessage("First name shoudn't exceed 150 characters");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Please specify your last name.").Length(1, 150).WithMessage("Last name shoudn't exceed 150 characters");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Please specify your email address.").EmailAddress().WithMessage("Please enter a valid email address");
        }
    }
}
