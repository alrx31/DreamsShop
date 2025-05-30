using Application.DTO;
using MediatR;

namespace Application.UseCases.DreamCreate;

public class DreamCreateCommand(DreamCreateDto dto) : IRequest
{
    public DreamCreateDto Dto { get; init; } = dto;
}