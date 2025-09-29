namespace Application.Dto.RequestModels
{
    public class CreateWalletRequest
    {
        public Guid CustomerId { get; set; }
        public int Pin { get; set; } = default!;
    }

    public class ChangeWalletPinRequest
    {
        public Guid WalletId { get; set; }
        public int OldPin { get; set; } = default!;
        public int NewPin { get; set; } = default!;
    }

    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }

}
