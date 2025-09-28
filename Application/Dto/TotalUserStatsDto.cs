namespace Application.Dto
{
    public record TotalUserStatsDto
    {
        public int TotalAdmins { get; init; }
        public int TotalCustomers { get; init; }
        public int TotalManagers { get; init; }
        public int TotalSuppliers { get; init; }
        public int TotalDeliveryAgents { get; init; }
        public int TotalCustomerServiceAgents { get; init; }
        public int TotalInventoryManagers { get; init; }
        public int TotalUsers { get; init; }
    }

}
