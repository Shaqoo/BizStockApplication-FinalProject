using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;

namespace Application.Commands.ChatMessages.ReactToMessage
{
    public class ReactToMessageHandler(
    IChatMessageRepository messageRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    INotifier notifier
) : IRequestHandler<ReactToMessageCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ReactToMessageCommand request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if (currentUser is null)
                return Result<string>.Failure("User not found.");

            var message = await messageRepository.GetByIdAsync(request.ReactToMessageRequest.MessageId);
            if (message is null)
                return Result<string>.Failure("Message not found.");

            var user = await userRepository.GetByIdAsync(currentUser.Id);
            if (user is null)
                return Result<string>.Failure("User not found.");

            var existingReaction = message.Reactions.FirstOrDefault(r => r.ReactedByUserId == currentUser.Id);

            if (existingReaction != null)
            {
                if (existingReaction.Emoji == request.ReactToMessageRequest.Emoji)
                {
                    message.Reactions.Remove(existingReaction);
                }
                else
                {
                    existingReaction.UpdateEmoji(request.ReactToMessageRequest.Emoji);
                }
            }
            else
            {
                message.Reactions.Add(new MessageReaction(message.Id,currentUser.Id, request.ReactToMessageRequest.Emoji));
            }

            await unitOfWork.BeginTransactionAsync();
            await messageRepository.UpdateMessage(message);
            await unitOfWork.CommitTransactionAsync();

            await notifier.SendMessageReactionAsync(message.ChatThreadId, message.Id, currentUser.Id, request.ReactToMessageRequest.Emoji);

            return Result<string>.Success("Reacted To Message");
        }
    }

}
