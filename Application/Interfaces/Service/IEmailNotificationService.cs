using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Service
{
    public interface IEmailNotificationService
    {
        Task SendEmailAsync(string to, string subject, string body,List<SendSmtpEmailAttachment> attachments = null!);
    }
}
