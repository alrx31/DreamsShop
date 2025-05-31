using Domain.Entity;
using MediatR;

namespace Application.UseCases.Dreams.DreamGetAll;

public class DreamGetAllCommand : IRequest<List<Dream>>
{
    public int StartIndex { get; init; }
    public int Count { get; init; }
}