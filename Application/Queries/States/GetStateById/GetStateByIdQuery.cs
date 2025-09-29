using Domain.Entities;
using MediatR;

namespace Application.Queries.States.GetStateById
{
    public record GetStateByIdQuery(int StateId) : IRequest<State?>;

}
