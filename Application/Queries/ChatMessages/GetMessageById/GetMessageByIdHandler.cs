using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatMessages.GetMessageById
{
    public class GetMessageByIdHandler(
    IChatMessageRepository repository,
    IMemoryCacheService cacheService)
    : IRequestHandler<GetMessageByIdQuery, Result<MessageDto>>
    {
        public async Task<Result<MessageDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"chat-message:{request.MessageId}";

             
            var cached = await cacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var message = await repository.GetByIdAsync(request.MessageId);
                    return message?.AsDto();
                },
                TimeSpan.FromMinutes(30)  
            );

            if (cached is null)
                return Result<MessageDto>.Failure("Message not found");

            return Result<MessageDto>.Success(cached);
        }
    }

}
