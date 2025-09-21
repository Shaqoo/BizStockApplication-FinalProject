using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Suppliers.GetAllSuppliers
{
    public class GetAllSuppliersHandler(ISupplierRepository supplierRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetAllSuppliersQuery, Result<PaginatedList<SupplierDto>>>
    {
        public async Task<Result<PaginatedList<SupplierDto>>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetAllSuppliers-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var suppliers = await supplierRepository.GetAllAsync(request.PageRequest);
                var supplierDtos = suppliers.Items.Select(s => s.SupplierAsDto()).ToList();
                var paginatedList = new PaginatedList<SupplierDto>(supplierDtos, suppliers.TotalCount, suppliers.PageNumber, suppliers.PageSize);
                return Result<PaginatedList<SupplierDto>>.Success(paginatedList);
            },TimeSpan.FromMinutes(10));

            return cachedResult ?? new Result<PaginatedList<SupplierDto>>();
        }
    }
}
