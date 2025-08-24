using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Suppliers.GetByEmail
{
    public class GetSupplierByEmailHandler(ISupplierRepository supplierRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetSuppliersByEmailQuery, Result<SupplierDto>>
    {
        public async Task<Result<SupplierDto>> Handle(GetSuppliersByEmailQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetSuppliersByEmailQuery:{request.Email}";

            var supplerDto = await memoryCacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var supplier = await supplierRepository.GetByEmailAsync(request.Email);
                    if (supplier == null)
                        return null!;
                    return supplier.SupplierAsDto();
                },
                TimeSpan.FromMinutes(10)
            );
            if (supplerDto is null)
            {
                return Result<SupplierDto>.Failure("Supplier not found.");
            }
            return Result<SupplierDto>.Success(supplerDto);
        }
    }
}
