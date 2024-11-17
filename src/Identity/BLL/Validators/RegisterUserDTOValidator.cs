using BLL.DTO;
using FluentValidation;

namespace BLL.Validators;

public class RegisterUserDTOValidator: AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .MaximumLength(1000).WithMessage("Email is too long");
        
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name is too short")
            .MaximumLength(100).WithMessage("Name is too long");
        
        RuleFor(x => x.Password)
            .NotNull().NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password is too short")
            .MaximumLength(1000).WithMessage("Password is too long");
    }
}