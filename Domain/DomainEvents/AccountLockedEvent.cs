using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class AccountLockedEvent(string email,string fullName, string IpAddress, string DeviceInfo) : INotification
    {
        public string FullName { get; } = fullName;
        public string IpAddress { get; } = IpAddress;
        public string DeviceInfo { get; } = DeviceInfo;
        public string Email { get; } = email;
    }
}
