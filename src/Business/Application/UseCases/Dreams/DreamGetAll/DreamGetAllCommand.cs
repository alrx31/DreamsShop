using Application.DTO;
using MediatR;

namespace Application.UseCases.Dreams.DreamGetAll;

public class DreamGetAllCommand : IRequest<List<DreamResponseDto>>
{
    public int StartIndex { get; init; }
    public int Count { get; init; }
}