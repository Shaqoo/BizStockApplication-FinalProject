namespace Application.Interfaces.Service
{
    public interface IPaymentGatewayService
    {
        Task<string> InitializeTransactionAsync(decimal amount, string email, string reference);
        Task<string> VerifyTransactionAsync(string reference);
    }

}
