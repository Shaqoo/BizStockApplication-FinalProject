using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Dto.RequestModels
{
    public record CreateProductRequestModel(string Name,Guid CategoryId,string SKU,string Barcode,IFormFile QrCodeValue,
        string Description,IFormFile ImageUrl,decimal CostPrice,decimal SellingPrice,UnitOfMeasure UnitOfMeasure,
        Guid BrandId, double Weight = 1);
     
}
