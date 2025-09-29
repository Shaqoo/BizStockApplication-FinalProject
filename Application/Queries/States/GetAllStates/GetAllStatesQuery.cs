using Domain.Entities;
using MediatR;

namespace Application.Queries.States.GetAllStates
{
    public record GetAllStatesQuery : IRequest<IEnumerable<State>>;
}
