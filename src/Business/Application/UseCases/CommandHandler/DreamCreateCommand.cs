using Application.DTO;
using MediatR;

namespace Application.UseCases.CommandHandler;

public class DreamCreateCommand(DreamCreateDto dto) : IRequest
{
    public DreamCreateDto Dto { get; set; } = dto;
}