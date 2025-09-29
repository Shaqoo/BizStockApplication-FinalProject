using Domain.Entities;
using MediatR;

namespace Application.Queries.Lgas.GetLgasByStateId
{
    public record GetLgasByStateIdQuery(int StateId) : IRequest<IEnumerable<Lga>>;
}
