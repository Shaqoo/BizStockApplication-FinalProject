using Application.Dto;
using MediatR;

namespace Application.Queries.Suppliers.GetByEmail
{
    public record GetSuppliersByEmailQuery(string Email) : IRequest<Result<SupplierDto>>;
}
