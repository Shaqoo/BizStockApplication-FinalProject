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

namespace Application.Commands.ChatThreads.Assign
{
    public class AssignAgentToThreadHandler(IAuthService authService,
    IUserRepository userRepository,
    IChatThreadRepository chatThreadRepository,
    IAuditLogRepository auditLogRepository,
    IMediator mediator,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AssignAgentToThreadCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AssignAgentToThreadCommand request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            var isAdmin = currentUser.RoleName == "Admin";
            var isAgent = currentUser.RoleName == "CustomerService";

            if (currentUser == null || (!isAdmin && !isAgent))
            {
                return Result<string>.Failure("Unauthorized access.");
            }


            var thread = await chatThreadRepository.GetByIdAsync(request.ChatThreadId);
            if (thread == null)
            {
                return Result<string>.Failure("Chat thread not found.");
            }

            if (thread.Status == ChatStatus.Closed)
            {
                return Result<string>.Failure("Cannot assign a closed thread.");
            }

            if (thread.AssignedAgentId != null && !isAdmin)
            {
                return Result<string>.Failure("Only admins can reassign a thread already in progress.");
            }


            var agent = await userRepository.GetByIdAsync(request.AgentId);
            if (agent == null || !agent.HasRole(Role.CustomerService))
            {
                return Result<string>.Failure("Assigned user is not a valid agent.");
            }

            thread.AssignAgent(agent.Id);

            try
            {
                await unitOfWork.BeginTransactionAsync();
                await chatThreadRepository.UpdateThread(thread);
                await unitOfWork.CommitTransactionAsync();

                await mediator.Publish(new ChatThreadAssignedEvent(thread.Id, agent.Id));

                await auditLogRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "AssignChatThread",
                    "ChatThread",
                    thread.Id,
                    $"Thread assigned to agent '{agent.FullName}'",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));

                return Result<string>.Success("Agent assigned to chat thread.");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();

                await auditLogRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "AssignChatThreadError",
                    "ChatThread",
                    thread.Id,
                    $"Failed to assign thread: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));

                return Result<string>.Failure("Error assigning agent to thread.");
            }

        }
    }
}
