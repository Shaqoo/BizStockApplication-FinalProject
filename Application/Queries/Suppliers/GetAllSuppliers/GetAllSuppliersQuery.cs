using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Suppliers.GetAllSuppliers
{
    public record GetAllSuppliersQuery(PageRequest PageRequest) : IRequest<Result<PaginatedList<SupplierDto>>>;

}
