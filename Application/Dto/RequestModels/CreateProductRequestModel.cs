using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Dto.RequestModels
{
    public record CreateProductRequestModel(string Name,Guid CategoryId,string SKU,string Barcode,string QrCodeValue,
        string Description,IFormFile ImageUrl,decimal CostPrice,decimal SellingPrice,UnitOfMeasure UnitOfMeasure,
        Guid BrandId);
     
}
