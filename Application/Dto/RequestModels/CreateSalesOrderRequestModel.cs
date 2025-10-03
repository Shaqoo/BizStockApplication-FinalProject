namespace Application.Dto.RequestModels
{
    public record CreateSalesOrderRequestModel(Guid DeliveryAddressId,DateTime ExpectedDeliveyDate,decimal DeliveryCost);
    
}
