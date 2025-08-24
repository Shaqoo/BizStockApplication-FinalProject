using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BizStockContext _context;

        public ProductRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.Include(a => a.StockByWarehouse).FirstAsync(a => a.Id == id);
        }

        public async Task<PaginatedList<Product>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse).AsNoTracking().Where(a => a.Status == ProductStatus.Approved).AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<PaginatedList<Product>> GetProductsByCategoryId(Guid categoryId, PageRequest pageRequest)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse)
                .Where(p => p.CategoryId == categoryId && p.Status == ProductStatus.Approved);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<Product>> GetProductsOrderedByPrice(PageRequest pageRequest, bool ascending = true)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse).Where(a => a.Status == ProductStatus.Approved).AsQueryable();

            query = ascending
                ? query.OrderBy(p => p.SellingPrice)
                : query.OrderByDescending(p => p.SellingPrice);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<Product>> GetProductsByCategoryOrderedByPrice(Guid categoryId, PageRequest pageRequest, bool ascending = true)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse)
                .Where(p => p.CategoryId == categoryId && p.Status == ProductStatus.Approved);

            query = ascending
                ? query.OrderBy(p => p.SellingPrice)
                : query.OrderByDescending(p => p.SellingPrice);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        

        public async Task<PaginatedList<Product>> SearchProductsAsync(string keyword, PageRequest pageRequest)
        {
            var query = await GetFullTextSearchQueryAsync(keyword);
            if (query != null)
            {
                return await GetPaginatedResultsAsync(query, pageRequest);
            }

            query = await GetILikeSearchQueryAsync(keyword);
             
            return await GetPaginatedResultsAsync(query, pageRequest);
            //query = await GetTrigramSearchQueryAsync(keyword);
           // return await GetPaginatedResultsAsync(query, pageRequest);
        }

        private IQueryable<Product> IncludeProductRelations(IQueryable<Product> query)
        {
            return query
                .Include(p => p.StockByWarehouse)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductTags);
        }

        private async Task<IQueryable<Product>> GetFullTextSearchQueryAsync(string keyword)
        {
            var formattedKeyword = keyword.Trim().Replace(" ", " & ");

            var query = IncludeProductRelations(_context.Products)
                .Where(p =>
                    p.SearchVector.Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword))
                    || EF.Functions.ToTsVector("english", p.Category.Name)
                        .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword))
                    || EF.Functions.ToTsVector("english", p.Brand.Name)
                        .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword))
                    || p.ProductTags.Any(t =>
                        EF.Functions.ToTsVector("english", t.Tag.Name)
                            .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword))
                    )
                    && p.Status == ProductStatus.Approved);

            return await query.AnyAsync() ? query : query;
        }

        private async Task<IQueryable<Product>> GetILikeSearchQueryAsync(string keyword)
        {
            var query = IncludeProductRelations(_context.Products)
                .Where(p =>
                    (
                        EF.Functions.ILike(p.Name, $"%{keyword}%")
                        || EF.Functions.ILike(p.Category.Name, $"%{keyword}%")
                        || EF.Functions.ILike(p.Brand.Name, $"%{keyword}%")
                        || p.ProductTags.Any(t => EF.Functions.ILike(t.Tag.Name, $"%{keyword}%"))
                    )
                    && p.Status == ProductStatus.Approved
                );

            return await query.AnyAsync() ? query : query;
        }

        //private async Task<IQueryable<Product>> GetTrigramSearchQueryAsync(string keyword)
        //{
        //    var query = IncludeProductRelations(_context.Products)
        //        .Where(p =>
        //            (
        //                EF.Functions.TrigramsSimilarity(p.Name, keyword) > 0.3
        //                || EF.Functions.TrigramsSimilarity(p.Category.Name, keyword) > 0.3
        //                || EF.Functions.TrigramsSimilarity(p.Brand.Name, keyword) > 0.3
        //                || p.ProductTags.Any(t => EF.Functions.TrigramsSimilarity(t.Tag.Name, keyword) > 0.3)
        //            )
        //            && p.Status == ProductStatus.Approved
        //        )
        //        .OrderByDescending(p =>
        //            Math.Max(
        //                Math.Max(
        //                    EF.Functions.TrigramsSimilarity(p.Name, keyword),
        //                    EF.Functions.TrigramsSimilarity(p.Category.Name, keyword)
        //                ),
        //                Math.Max(
        //                    EF.Functions.TrigramsSimilarity(p.Brand.Name, keyword),
        //                    p.ProductTags.Max(t => EF.Functions.TrigramsSimilarity(t.Tag.Name, keyword))
        //                )
        //            )
        //        );

        //    await Task.CompletedTask;
        //    return query;
        //}


        private async Task<PaginatedList<Product>> GetPaginatedResultsAsync(IQueryable<Product> query, PageRequest pageRequest)
        {
            var total = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }


     
        public async Task<PaginatedList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, PageRequest pageRequest)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse)
                .Where(p => p.SellingPrice >= minPrice && p.SellingPrice <= maxPrice && p.Status == ProductStatus.Approved)
                .OrderBy(p => p.SellingPrice);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<Product?> GetByExpression(Expression<Func<Product, bool>> predicate)
        {
             return await _context.Products.Include(a => a.StockByWarehouse).FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Product product)
        {
             _context.Products.Update(product);
            await Task.CompletedTask;
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await _context.Products.AsNoTracking().AnyAsync(p => p.Id == Id);
        }

        public async Task<PaginatedList<Product>> GetProductsByWarehouseIdAsync(Guid warehouseId, PageRequest pageRequest)
        {
            var query = _context.WarehouseItems
                .Include(a => a.Product)
                .AsNoTracking()
                .Where(p => p.WarehouseId == warehouseId && p.Product.Status == ProductStatus.Approved);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.Product.Name)  
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(a => a.Product)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<Product>> GetProductsWithLowStockAsync(PageRequest pageRequest)
        {
            var query = _context.WarehouseItems
                .AsNoTracking()
                .Where(p => p.Quantity <= p.ReorderLevel).Select(a => a.Product);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }


        public async Task<PaginatedList<Product>> GetRecentlyAddedProductsAsync(PageRequest pageRequest)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse)
                .AsNoTracking()
                .OrderByDescending(p => p.DateCreated);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }


        public async Task<PaginatedList<Product>> GetTopRatedProductsAsync(PageRequest pageRequest)
        {
            var query = _context.Products.Include(a => a.StockByWarehouse)
                .AsNoTracking()
                .Include(p => p.Reviews)
                .Select(p => new
                {
                    Product = p,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0
                })
                .OrderByDescending(p => p.AverageRating);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(x => x.Product)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<Product>> GetProductsByStatus(PageRequest pageRequest, ProductStatus productStatus)
        {
             var query = _context.Products.Include(a => a.StockByWarehouse)
                .AsNoTracking()
                .Where(p => p.Status == productStatus);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
        }
    }

    /*
     public async Task<PaginatedList<Product>> SearchProductsAsync(string keyword, PageRequest pageRequest)
{
    var formattedKeyword = keyword.Trim().Replace(" ", " & ");

    var query = _context.Products
        .Where(p =>
            EF.Functions.ToTsVector("english", EF.Property<string>(p, "SearchVector"))
            .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword)) && p.Status == ProductStatus.Approved);

    var total = await query.CountAsync();

    if (total == 0)
    {
        query = _context.Products
            .Where(p => EF.Functions.ILike(p.Name, $"%{keyword}%") && p.Status == ProductStatus.Approved);
        total = await query.CountAsync();
    }

    if (total == 0)
    {
        query = _context.Products
            .Where(p => EF.Functions.TrigramsSimilarity(p.Name, keyword) > 0.3 && p.Status == ProductStatus.Approved)
            .OrderByDescending(p => EF.Functions.TrigramsSimilarity(p.Name, keyword));

        total = await query.CountAsync();
    }

    var items = await query
        .OrderBy(p => p.Name)
        .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
        .Take(pageRequest.PageSize)
        .ToListAsync();

    return new PaginatedList<Product>(items, total, pageRequest.Page, pageRequest.PageSize);
}
     */

}
