using FluentValidation;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;

public class ConsumerUserRegisterCommandValidator : AbstractValidator<ConsumerUserRegisterCommand>
{
    public ConsumerUserRegisterCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name length must be less than 100 characters.");

        RuleFor(x => x.Dto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.")
            .MaximumLength(100).WithMessage("Email length must be less than 100 characters.");
        
        RuleFor(x => x.Dto.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password length must be at least 8 characters.");
        
        RuleFor(x=>x.Dto.PasswordRepeat)
            .NotEmpty().WithMessage("PasswordRepeat is required.")
            .MinimumLength(8).WithMessage("Password length must be at least 8 characters.")
            .Equal(x => x.Dto.Password).WithMessage("Password must match.");
    }
}