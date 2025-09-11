using MediatR;

namespace Domain.DomainEvents
{
    public class LoginEvent(Guid userId, string ipAddress, string deviceInfo) : INotification
    {
        public Guid UserId { get; } = userId;
        public string IpAddress { get; } = ipAddress;
        public string DeviceInfo { get; } = deviceInfo;
    }

}
