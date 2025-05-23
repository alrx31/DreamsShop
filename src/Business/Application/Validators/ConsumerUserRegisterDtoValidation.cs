using Application.DTO;
using FluentValidation;

namespace Application.Validators;

public class ConsumerUserRegisterDtoValidation : AbstractValidator<ConsumerUserRegisterDto>
{
    public ConsumerUserRegisterDtoValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name length must be less than 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.")
            .MaximumLength(100).WithMessage("Email length must be less than 100 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password length must be at least 8 characters.");
        
        RuleFor(x=>x.PasswordRepeat)
            .NotEmpty().WithMessage("PasswordRepeat is required.")
            .MinimumLength(8).WithMessage("Password length must be at least 8 characters.")
            .Equal(x => x.Password).WithMessage("Password must match.");
    }
    
}