using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Suppliers
{
    public class ViewSupplierDetailsHandler(IUserRepository userRepository,
        IAuthService authService,
        ISupplierRepository supplierRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<ViewSupplierDetailsQuery, Result<SupplierDto>>
    {
        public async Task<Result<SupplierDto>> Handle(ViewSupplierDetailsQuery request, CancellationToken cancellationToken)
        {
             var currentUser = authService.CurrentUser();   

            if (currentUser == null)
                return Result<SupplierDto>.Failure("User not found.");

            string cacheKey = $"SupplierDetails:{currentUser.Id}";

            var user = await userRepository.CheckIfExists(x => x.Id == currentUser.Id && !x.IsDeleted);

            if (!user)
                return Result<SupplierDto>.Failure("User not found.");
            
            var supplerDto = await distributedCacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var supplier = await supplierRepository.GetByEmailAsync(currentUser.Email);
                    if (supplier == null)
                        return null!;
                    return supplier.SupplierAsDto();
                },
                TimeSpan.FromMinutes(10)  
            );
            if(supplerDto is null)
            {
                return Result<SupplierDto>.Failure("Supplier not found.");
            }
            return Result<SupplierDto>.Success(supplerDto);
        }
    }
}
