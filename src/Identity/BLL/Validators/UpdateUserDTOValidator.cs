using BLL.DTO;
using FluentValidation;

namespace BLL.Validators;

public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        RuleFor(x => x.Password)
            .NotNull().WithMessage("Invalid Password")
            .MinimumLength(6).WithMessage("Password is too short")
            .MaximumLength(1000).WithMessage("Password is too long");

        RuleFor(x => x.RequestorId)
            .NotNull().WithMessage("Requester ID is required");

        
        // TODO: how to user one validator in enother
        
        RuleFor(x => x.UserDTO.Email)
            .NotNull().NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .MaximumLength(1000).WithMessage("Email is too long");
        
        RuleFor(x => x.UserDTO.Name)
            .NotNull().NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name is too short")
            .MaximumLength(100).WithMessage("Name is too long");
        
        RuleFor(x => x.UserDTO.Password)
            .NotNull().NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password is too short")
            .MaximumLength(1000).WithMessage("Password is too long");
    }
}