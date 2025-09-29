using Domain.Entities;
using MediatR;

namespace Application.Queries.Lgas.GetLgaById
{
    public record GetLgaByIdQuery(int LgaId) : IRequest<Lga?>;
}
