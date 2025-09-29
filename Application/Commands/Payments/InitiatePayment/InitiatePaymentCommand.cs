using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Payments.InitiatePayment
{
    public record InitiatePaymentCommand(InitiatePaymentRequest Request,RequestMetadata RequestMetadata)
    : IRequest<Result<string>>; 

}
