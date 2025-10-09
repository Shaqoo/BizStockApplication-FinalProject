using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using Domain.Enums;
using MediatR;

namespace Application.Queries.ChatThreads.GetCustomerOpenThread
{
    public class GetCustomerOpenThreadHandler(IChatThreadRepository chatThreadRepository,
        ICustomerRepository customerRepository,
        IAuthService authService) : IRequestHandler<GetCustomerOpenThreadQuery, Result<ChatThreadDto>>
    {
        public async Task<Result<ChatThreadDto>> Handle(GetCustomerOpenThreadQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = authService.CurrentUser();
            if (currentUserId == null)
            {
                return Result<ChatThreadDto>.Failure("User not authenticated.");
            }
            var customer = await customerRepository.GetByEmailAsync(currentUserId.Email);
            if (customer == null)
            {
                return Result<ChatThreadDto>.Failure("Customer not found.");
            }
            var thread = await chatThreadRepository.GetByExpression(a => a.CustomerId == customer.Id && 
             (a.Status == ChatStatus.Open || a.Status == ChatStatus.InProgress) );
            if (thread == null)
            {
                return Result<ChatThreadDto>.Failure("No Exiting Chat Thread For This User");
            }

            return Result<ChatThreadDto>.Success(thread.ChatThreadAsDto());
        }
    }
}
