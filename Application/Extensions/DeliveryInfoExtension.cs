using Application.Configurations;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Application.Extensions
{
    public static class DeliveryInfoExtension
    {
        private const string DeliveryCostKey = "DeliveryCost";
        private const string DeliveryAddressKey = "DeliveryAddress";
        private const string DeliveryDateKey = "DeliveryDate";


        public static void SetDeliveryInfo(this HttpContext context, Guid addressId, decimal cost,string eta)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));


            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(30)
            };

            context.Response.Cookies.Append(DeliveryAddressKey, addressId.ToString(),cookieOptions);
            context.Response.Cookies.Append(DeliveryCostKey, cost.ToString(),cookieOptions);
            var expectedDate = EtaParser.ParseEta(eta);
            if (expectedDate != null)
                context.Response.Cookies.Append(DeliveryDateKey, expectedDate.Value.ToString("O"),cookieOptions); 
        }

        public static GetDeliveryInfoDto GetDeliveryInfo(this HttpContext context)
        {
            if (context == null)
                return new GetDeliveryInfoDto(null, null, null);

            string addressIdString = "";
            if (context.Request.Cookies.TryGetValue(DeliveryAddressKey, out var address) &&
                !string.IsNullOrWhiteSpace(address))
            {
                addressIdString = address;
            }

            string costString = "";
            if (context.Request.Cookies.TryGetValue(DeliveryCostKey, out var costValue) &&
                !string.IsNullOrWhiteSpace(costValue))
            {
                costString = costValue;
            }

            string etaString = "";
            if (context.Request.Cookies.TryGetValue(DeliveryDateKey, out var etaValue) &&
                !string.IsNullOrWhiteSpace(etaValue))
            {
                etaString = etaValue;
            }


           

            if (string.IsNullOrEmpty(addressIdString) ||
                string.IsNullOrEmpty(costString) ||
                string.IsNullOrEmpty(etaString))
                return new GetDeliveryInfoDto(null, null, null);

            if (!Guid.TryParse(addressIdString, out var addressId) ||
                !decimal.TryParse(costString, out var cost) ||
                !DateTime.TryParse(etaString, out var eta))
                return new GetDeliveryInfoDto(null, null, null);

            return new GetDeliveryInfoDto(addressId, cost, eta);
        }

        public static bool IsValidDeliveryInfo(this HttpContext session)
        {
            var info = session.GetDeliveryInfo();
            return info.AddressId.HasValue && info.Cost.HasValue && info.ETA.HasValue;
        }
        public record GetDeliveryInfoDto(Guid? AddressId,decimal? Cost,DateTime? ETA);
    }
}
