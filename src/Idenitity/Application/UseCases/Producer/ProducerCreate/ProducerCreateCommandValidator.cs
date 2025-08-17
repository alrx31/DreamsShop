using System;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using FluentValidation;

namespace Application.UseCases.Producer.ProducerCreate;

public class ProducerCreateCommandValidator : AbstractValidator<ProducerCreateCommand>
{
    public ProducerCreateCommandValidator()
    {
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.ProducerUserDto)
            .NotNull().WithMessage("Producer user details are required.")
            .SetValidator(new ProducerUserRegisterCommandValidator());
    }
}