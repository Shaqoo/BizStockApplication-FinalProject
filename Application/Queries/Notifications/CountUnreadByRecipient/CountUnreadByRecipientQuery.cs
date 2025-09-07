using Application.Dto;
using MediatR;

namespace Application.Queries.Notifications.CountUnreadByRecipient
{
    public record CountUnreadByRecipientQuery() : IRequest<Result<int>>;

}
