using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Commands.ChatMessages.SendMessage
{
    public class SendMessageHandler(
    IChatMessageRepository messageRepository,
    IChatThreadRepository threadRepository,
    IUserRepository userRepository,
    IUploadService uploadService,
    IUnitOfWork unitOfWork,
    INotifier notifier,
    IAuditLogRepository logRepository,
    IAuthService authService
) : IRequestHandler<SendMessageCommand, Result<MessageDto>>
    {
        public async Task<Result<MessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var sendRequest = request.SendMessageRequest;

            var thread = await threadRepository.GetByIdAsync(sendRequest.ChatThreadId);
            if (thread == null)
                return Result<MessageDto>.Failure("Chat thread not found.");

            if(thread.Status == ChatStatus.Closed)
                return Result<MessageDto>.Failure("Cannot send message to a closed thread.");

            var currentUser = authService.CurrentUser();
            if (currentUser == null)
                return Result<MessageDto>.Failure("Unauthorized to send message on behalf of another user.");

            var sender = await userRepository.GetByIdAsync(currentUser.Id);
            if (sender == null)
                return Result<MessageDto>.Failure("Sender not found.");

            ChatMessage? repliedTo = null;
            if (sendRequest.RepliedToMessageId.HasValue)
                repliedTo = await messageRepository.GetByIdAsync(sendRequest.RepliedToMessageId.Value);

            string? audioUrl = null;
            if (sendRequest.Audio is not null)
            {
                using var audioStream = sendRequest.Audio.OpenReadStream();
                audioUrl = await uploadService.MessageAudioAsync(audioStream, sendRequest.Audio.FileName);
            }

            string? pictureUrl = null;
            if (sendRequest.Picture is not null)
            {
                using var imageStream = sendRequest.Picture.OpenReadStream();
                pictureUrl = await uploadService.MessageImageAsync(imageStream, sendRequest.Picture.FileName);
            }

            var message = new ChatMessage(
                chatThreadId: sendRequest.ChatThreadId,
                senderId: currentUser.Id,
                message: sendRequest.Message,
                audioUrl: audioUrl,
                pictureUrl: pictureUrl,
                replyToId: sendRequest.RepliedToMessageId
            );

            await messageRepository.AddAsync(message);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await logRepository.AddAsync(new AuditLog(
            userId: sender.Id,
            action: "SendMessage",
            entityName: "ChatMessage",
            entityId: message.Id,
            details: $"Sent message in thread {thread.Id} with content: {message.Message ?? "Media"}",
            ip: request.RequestMetadata.IpAddress,
            userAgent: request.RequestMetadata.UserAgent
        ));


            var dto = message.AsDto();
            await notifier.SendChatMessageAsync(dto,thread.Id);

            return Result<MessageDto>.Success(dto);
        }
    }


}
