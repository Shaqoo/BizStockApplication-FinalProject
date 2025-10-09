using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Dto.RequestModels
{
    public class CreateFezOrderRequestItem
    {
        [JsonPropertyName("recipientAddress")]
        public string RecipientAddress { get; set; } = string.Empty;

        [JsonPropertyName("recipientState")]
        public string RecipientState { get; set; } = string.Empty;

        [JsonPropertyName("recipientName")]
        public string RecipientName { get; set; } = string.Empty;

        [JsonPropertyName("recipientPhone")]
        public string RecipientPhone { get; set; } = string.Empty;

        [JsonPropertyName("recipientEmail")]
        public string? RecipientEmail { get; set; }

        [JsonPropertyName("uniqueID")]
        public string UniqueID { get; set; } = string.Empty;

        [JsonPropertyName("BatchID")]
        public string BatchID { get; set; } = string.Empty;

        [JsonPropertyName("CustToken")]
        public string? CustToken { get; set; }

        [JsonPropertyName("itemDescription")]
        public string? ItemDescription { get; set; }

        [JsonPropertyName("additionalDetails")]
        public string? AdditionalDetails { get; set; }

        [JsonPropertyName("valueOfItem")]
        public decimal ValueOfItem { get; set; }

        [JsonPropertyName("weight")]
        public decimal Weight { get; set; } = 1;

        [JsonPropertyName("pickUpState")]
        public string? PickUpState { get; set; }

        [JsonPropertyName("pickUpAddress")]
        public string? PickUpAddress { get; set; }

        [JsonPropertyName("waybillNumber")]
        public string? WaybillNumber { get; set; }

        [JsonPropertyName("pickUpDate")]
        public DateTime? PickUpDate { get; set; }

        [JsonPropertyName("isItemCod")]
        public bool? IsItemCod { get; set; } = false;

        [JsonPropertyName("cashOnDeliveryAmount")]
        public decimal? CashOnDeliveryAmount { get; set; }

        [JsonPropertyName("fragile")]
        public bool? Fragile { get; set; } = false;

        [JsonPropertyName("lockerID")]
        public string? LockerID { get; set; }
    }


    public class CreateFezOrderResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string> OrderNos { get; set; } = new();  
    }


    public class CostEstimateRequestDto
    {
        [JsonPropertyName("state")]
        public string DestinationState { get; set; } = string.Empty;

        [JsonPropertyName("pickUpState")]
        public string? PickUpState { get; set; }

        [JsonPropertyName("weight")]
        public double? Weight { get; set; }

        [JsonPropertyName("locker")]
        public bool Locker { get; set; } = false;
    }

    public class CostEstimateResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("Cost")]
        [JsonConverter(typeof(SingleOrArrayConverter<CostItem>))]
        public List<CostItem> Cost { get; set; } = new();
    }
    public class CostItem
    {
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("cost")]
   
        public decimal Cost { get; set; }
    }

    public record GetOrderCostAndETAResponseDto(CostEstimateResponseDto CostEstimateResponseDto
        ,DeliveryTimeEstimateResponseDto DeliveryTimeEstimateResponseDto);


    public class CheckOrderStatusRequestDto
    {
        public string OrderNumber { get; set; } = string.Empty;  
    }

    public class CheckOrderStatusResponseDto
    {
        public bool Success { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; 
        public string LastUpdated { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DeliveryTimeEstimateRequestDto
    {
        [JsonPropertyName("delivery_type")]
        public string DeliveryType { get; set; } = "local";

        [JsonPropertyName("pick_up_state")]
        public string PickUpState { get; set; } = string.Empty;

        [JsonPropertyName("drop_off_state")]
        public string DropOffState { get; set; } = string.Empty;
    }
    public class DeliveryTimeEstimateResponseDto
    {
        public bool Success { get; set; }
        public string ETA { get; set; } = string.Empty;  
        public string Message { get; set; } = string.Empty;
    }

    public class TrackOrderResponseDto
    {
        public bool Success { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientAddress { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderAddress { get; set; } = string.Empty;
        public string RecipientState { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderHistoryDto> History { get; set; } = new();
    }

    public class OrderHistoryDto
    {
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime StatusCreationDate { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
    }


    public class SingleOrArrayConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return JsonSerializer.Deserialize<List<T>>(ref reader, options)!;
            }

            var item = JsonSerializer.Deserialize<T>(ref reader, options)!;
            return new List<T> { item };
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }

    public class FezOrderSummaryDto
    {
        [JsonPropertyName("orderNo")]
        public string OrderNo { get; set; } = string.Empty;

        [JsonPropertyName("recipientName")]
        public string RecipientName { get; set; } = string.Empty;

        [JsonPropertyName("recipientAddress")]
        public string RecipientAddress { get; set; } = string.Empty;

        [JsonPropertyName("recipientPhone")]
        public string RecipientPhone { get; set; } = string.Empty;

        [JsonPropertyName("orderStatus")]
        public string OrderStatus { get; set; } = string.Empty;

        [JsonPropertyName("cost")]
        public string? Cost { get; set; }

        [JsonPropertyName("orderDate")]
        public string? OrderDate { get; set; }
    }

    public class FezOrderListResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("orders")]
        public FezOrderData Orders { get; set; } = new();
    }

    public class FezOrderData
    {
        [JsonPropertyName("data")]
        public List<FezOrderSummaryDto> Data { get; set; } = new();
    }

}
