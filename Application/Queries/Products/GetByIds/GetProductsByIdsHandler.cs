using Application.Dto;
using Application.Interfaces.Repository;
using MediatR;

namespace Application.Queries.Products.GetByIds
{
    public class GetProductsByIdsHandler(IProductRepository productRepository) 
        : IRequestHandler<GetProductByIdsQuery, IEnumerable<ProductDto>>
    {
        public async Task<IEnumerable<ProductDto>> Handle(GetProductByIdsQuery request, CancellationToken cancellationToken)
        {
            return await productRepository.GetByIdsAsync(request.Ids);
        }
    }
}
