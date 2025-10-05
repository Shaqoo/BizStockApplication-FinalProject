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


        public static void SetDeliveryInfo(this ISession session, Guid addressId, decimal cost,string eta)
        {
            session.SetString(DeliveryAddressKey, addressId.ToString());
            session.SetString(DeliveryCostKey, cost.ToString());
            var expectedDate = EtaParser.ParseEta(eta);
            if (expectedDate != null)
                session.SetString(DeliveryDateKey, expectedDate.Value.ToString("O")); 
        }

        public static GetDeliveryInfoDto GetDeliveryInfo(this ISession session)
        {
            if (session == null)
                return new GetDeliveryInfoDto(null, null, null);

            var addressIdString = session.GetString(DeliveryAddressKey);
            var costString = session.GetString(DeliveryCostKey);
            var etaString = session.GetString(DeliveryDateKey);

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

        public static bool IsValidDeliveryInfo(this ISession session)
        {
            var info = session.GetDeliveryInfo();
            return info.AddressId.HasValue && info.Cost.HasValue && info.ETA.HasValue;
        }
        public record GetDeliveryInfoDto(Guid? AddressId,decimal? Cost,DateTime? ETA);
    }
}
