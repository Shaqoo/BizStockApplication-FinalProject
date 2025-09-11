using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Commands.ChatThreads.Create
{
    public class CreateChatThreadHandler(IAuthService authService,
        ICustomerRepository customerRepository,
        IAuditLogRepository auditLogRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IChatThreadRepository chatThreadRepository) : IRequestHandler<CreateChatThreadCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateChatThreadCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                return Result<Guid>.Failure("User not authenticated.");
            }

            var customer = await customerRepository.GetByEmailAsync(user.Email);

            if (customer == null)
            {
                return Result<Guid>.Failure("Customer not found.");
            }
            if (customer.IsDeleted)
            {
                return Result<Guid>.Failure("Customer account is deleted.");
            }

            var existingOpenThread = await chatThreadRepository.FindAsync(a => a.CustomerId == customer.Id &&
         (a.Status == ChatStatus.Open || a.Status == ChatStatus.InProgress));

            if (existingOpenThread.Count() != 0)
                return Result<Guid>.Failure("An open chat thread already exists.");



            var chatThread = new ChatThread(customer.Id);
            chatThread.SetCreatedBy(customer.FullName);

            try
            {
                await unitOfWork.BeginTransactionAsync();
                await chatThreadRepository.AddAsync(chatThread);
                await unitOfWork.CommitTransactionAsync();
                await mediator.Publish(new ChatThreadCreatedEvent(chatThread.Id,customer.FullName));

                await auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "CreateChatThread",
                    "Customer",
                    chatThread.Id,
                    $"Chat thread created for customer '{customer.FullName}'.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));
                return Result<Guid>.Success(chatThread.Id);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();

                await auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "CreateChatThreadError",
                    "Customer",
                    null,
                    $"Error creating chat thread for customer '{customer.FullName}': {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));

                return Result<Guid>.Failure($"An error occurred while creating the chat thread: {ex.Message}");
            }
        }
    }
}
