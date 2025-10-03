namespace Domain.Enums
{
    public enum DeliveryStatus
    {
        Pending = 0,       // Created but not yet handed to courier
        Processing = 1,    // Sent to Fez
        InTransit = 2,     // Courier picked it up
        Delivered = 3,     // Successfully delivered
        Failed = 4         // Delivery attempt failed
    }

}
