using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Payments.VerifyPayment
{
    public record VerifyPaymentCommand(string Reference,RequestMetadata RequestMetadata) : IRequest<Result<PaystackVerifyResponse>>;

}
