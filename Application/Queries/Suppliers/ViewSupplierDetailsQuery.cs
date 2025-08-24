using Application.Dto;
using MediatR;

namespace Application.Queries.Suppliers
{
    public record ViewSupplierDetailsQuery : IRequest<Result<SupplierDto>>;
}
