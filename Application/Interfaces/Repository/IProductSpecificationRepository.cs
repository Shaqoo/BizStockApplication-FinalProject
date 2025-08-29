using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IProductSpecificationRepository
    {
        Task<ProductSpecification?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductSpecification>> GetByProductIdAsync(Guid productId);
        Task AddAsync(ProductSpecification productSpecification);
        Task Update(ProductSpecification productSpecification);
        Task Remove(ProductSpecification productSpecification);
        Task<ProductSpecification?> GetByProductAndSpecificationAsync(Guid productId, Guid specificationId);

    }
}
