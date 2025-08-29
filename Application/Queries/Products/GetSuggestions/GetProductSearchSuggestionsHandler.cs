using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Products.GetSuggestions
{
    public class GetProductSearchSuggestionsHandler(IProductRepository productRepository,
        ILogger<GetProductSearchSuggestionsHandler> logger,
        IMemoryCacheService memoryCacheService)
        : IRequestHandler<GetProductSearchSuggestionsQuery, Result<IEnumerable<string>>>
    {
        public async Task<Result<IEnumerable<string>>> Handle(GetProductSearchSuggestionsQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.keyword))
                return Result<IEnumerable<string>>.Success(new  List<string>());

            string cacheKey = $"GetProductSearchSuggestionsQuery:{request.keyword}";
            logger.LogInformation("Product Suggestion Made For {name}", request.keyword);

            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var suggestions = await productRepository.GetSearchSuggestions(request.keyword);
                    return suggestions;
                });

            return Result<IEnumerable<string>>.Success(result);
        }
    }
}
