namespace Application.Dto.RequestModels
{
    public record ChangeProductPriceDto(Guid ProductId,
    decimal CostPrice,
    decimal SellingPrice);
     
}
