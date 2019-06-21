namespace KeepFitStore.Services.MessageSender
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        private const string SenderEmail = "keepFitStore@team.com";
        private const string SenderName = "Pavel Iliev";

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(SenderEmail, SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}