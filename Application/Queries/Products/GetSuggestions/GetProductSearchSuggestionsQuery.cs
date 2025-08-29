using Application.Dto;
using MediatR;

namespace Application.Queries.Products.GetSuggestions
{
    public record GetProductSearchSuggestionsQuery(string keyword) : IRequest<Result<IEnumerable<string>>>;
}
