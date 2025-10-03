using Application.Dto;

namespace Application.Interfaces.Service
{
    public interface IPaymentGatewayService
    {
        Task<string> InitializeTransactionAsync(decimal amount, string email, string reference);
        Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference);
    }

}
