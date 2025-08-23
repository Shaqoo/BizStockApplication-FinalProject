using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatThreads.GetChatThreadsByCustomer
{
    public class GetChatThreadsByCurrentUserQueryHandler(
    IChatThreadRepository repository,
    IAuthService authService,
    ICustomerRepository customerRepository,
    IMemoryCacheService cache)
    : IRequestHandler<GetChatThreadsByCurrentUserQuery, Result<PaginatedList<ChatThreadDto>>>
    {
        public async Task<Result<PaginatedList<ChatThreadDto>>> Handle(GetChatThreadsByCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = authService.CurrentUser();
            if (currentUserId == null)
            {
                return Result<PaginatedList<ChatThreadDto>>.Failure("User not authenticated.");
            }
            var customer = await customerRepository.GetByEmailAsync(currentUserId.Email);
            if (customer == null)
            {
                return Result<PaginatedList<ChatThreadDto>>.Failure("Customer not found.");
            }
            var cacheKey = $"chat-threads:user:{currentUserId}:page:{request.PageRequest.Page}:size:{request.PageRequest.PageSize}";

            var result = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var paged = await repository.GetByCustomerIdAsync(customer.Id, request.PageRequest);

                var dtoList = paged.Items.Select(thread => thread.ChatThreadAsDto()).ToList();

                return new PaginatedList<ChatThreadDto>
                {
                    Items = dtoList,
                    TotalCount = paged.TotalCount,
                    PageNumber = paged.PageNumber,
                    PageSize = paged.PageSize
                };
            });

            return Result<PaginatedList<ChatThreadDto>>.Success(result);
        }
    }

}
