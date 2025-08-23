using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;

namespace Application.Commands.ChatMessages.MarkAsRead
{
    public class MarkAsReadHandler(
    IChatMessageRepository messageRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    INotifier notifier
) : IRequestHandler<MarkAsReadCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if(currentUser is null)
                return Result<string>.Failure("User not found.");

            var message = await messageRepository.GetByIdAsync(request.MessageId);
            if (message is null)
                return Result<string>.Failure("Message not found.");

            if (message.IsRead)
                return Result<string>.Success("Message Read");  

             
            if (message.SenderId == currentUser.Id)
                return Result<string>.Failure("Sender cannot mark their own message as read.");

            message.MarkAsRead(); 
            await unitOfWork.SaveChangesAsync(cancellationToken);

            
            await notifier.SendMessageReadAsync(message.ChatThreadId, message.Id, currentUser.Id);

            return Result<string>.Success("Message Read");
        }
    }

}
