namespace Application.Dto
{
    public record CustomerStatsDto
    {
        public int TotalCustomers { get; set; }
        public int VerifiedCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int OpenComplaints { get; set; }
    }

}
