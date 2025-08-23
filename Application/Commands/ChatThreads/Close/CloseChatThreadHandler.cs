using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatThreads.Close
{
    public class CloseChatThreadHandler(
    IAuthService authService,
    IChatThreadRepository chatThreadRepository,
    IAuditLogRepository auditLogRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator
) : IRequestHandler<CloseChatThreadCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CloseChatThreadCommand request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if (currentUser == null)
                return Result<string>.Failure("Unauthorized access.");

            var thread = await chatThreadRepository.GetByIdAsync(request.ChatThreadId);
            if (thread == null)
                return Result<string>.Failure("Chat thread not found.");

            if (thread.Status == ChatStatus.Closed)
                return Result<string>.Failure("Thread is already closed.");

            if (thread.AssignedAgentId != currentUser.Id && currentUser.RoleName != Role.Admin.ToString())
                return Result<string>.Failure("You are not authorized to close this thread.");

            Guid agentId = thread.AssignedAgentId ?? Guid.Empty;

            thread.CloseThread();  

            try
            {
                await unitOfWork.BeginTransactionAsync();
                await chatThreadRepository.UpdateThread(thread);
                await unitOfWork.CommitTransactionAsync();
                await mediator.Publish(new ChatThreadClosedEvent(thread.Id,agentId,thread.CustomerId));

                await auditLogRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "CloseChatThread",
                    "ChatThread",
                    thread.Id,
                    $"Thread closed by {currentUser.Email}.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));

                return Result<string>.Success("Chat thread closed successfully.");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();

                await auditLogRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "CloseChatThreadError",
                    "ChatThread",
                    thread.Id,
                    $"Error closing thread: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));

                return Result<string>.Failure("An error occurred while closing the thread.");
            }
        }
    }

}
