using Application.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace Infrastructures.Service.Email
{
    public class BrevoNotificationService(IConfiguration configuration) : IEmailNotificationService
    {
        public async Task SendEmailAsync(string to, string subject, string body, List<SendSmtpEmailAttachment> attachments = null)
        {
            string apiKey = configuration["SendInBlue:Key"]!;
            sib_api_v3_sdk.Client.Configuration.Default.ApiKey["api-key"] = apiKey;
            var apiInstance = new TransactionalEmailsApi();

            var sender = new SendSmtpEmailSender("BizStock Application", "ShakirullahOhio@gmail.com");
            var recipient = new SendSmtpEmailTo(to);
            var recipients = new List<SendSmtpEmailTo> { recipient };


            string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8' />
<meta name='viewport' content='width=device-width, initial-scale=1.0' />
<title>BizStock Email</title>
</head>
<body style='font-family:Segoe UI, Tahoma, Geneva, Verdana, sans-serif; background-color:#f4f4f4; margin:0; padding:0;'>

  <div style='max-width:600px; margin:20px auto; background:white; padding:20px; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.05);'>

    <img src='https://localhost:7124/photos/3e6196fc-904d-4485-853e-4537cc2f44e5.jpg' alt='BizStock Logo' style='max-width:150px; display:block; margin:0 auto 20px;' />

    <div style='font-size:16px; color:#333; line-height:1.6;'>
      {System.Net.WebUtility.HtmlEncode(body).Replace("\n", "<br/>")}
    </div>

    <div style='font-size:13px; color:#666; text-align:center; margin-top:30px; border-top:1px solid #ddd; padding-top:15px;'>
      <p>Contact Us: 08109094694</p>
      <p>Email: <a href='mailto:support@BizStock.com' style='color:#6a1b9a;'>support@BizStock.com</a></p>
      <p>&copy; 2025 <strong>BizStock</strong>. All rights reserved.</p>
    </div>

  </div>

</body>
</html>";



            try
            {
                var email = new SendSmtpEmail(
                    sender: sender,
                    to: recipients,
                    subject: subject,
                    htmlContent: htmlContent,
                    attachment: attachments
                );

                await apiInstance.SendTransacEmailAsync(email);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }
    }
}
