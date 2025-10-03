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
            var stateString = session.GetString(DeliveryAddressKey);
            var costString = session.GetString(DeliveryCostKey);
            var eta = session.GetString(DeliveryDateKey);

            decimal? cost = null;
            if (!string.IsNullOrEmpty(costString))
                cost = decimal.Parse(costString);

            Guid? addressId = null;
            if (!string.IsNullOrEmpty(stateString))
                addressId = Guid.Parse(stateString);

            DateTime? parsedEta = null;
            if (!string.IsNullOrEmpty(eta))
                parsedEta = DateTime.Parse(eta,null,DateTimeStyles.RoundtripKind);

            return new GetDeliveryInfoDto(addressId,cost,parsedEta);
        }

        public record GetDeliveryInfoDto(Guid? AddressId,decimal? Cost,DateTime? ETA);
    }
}
