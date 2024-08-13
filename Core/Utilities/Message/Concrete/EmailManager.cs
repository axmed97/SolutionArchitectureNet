using Core.Entities.Concrete;
using Core.Utilities.Message.Abstract;
using FluentEmail.Core;

namespace Core.Utilities.Message.Concrete
{
    public class EmailManager : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailManager(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task SendEmailAsync(EmailMetadata emailData)
        {
            //EmailData - FluentEmail.Core.Models if you want can use
            await _fluentEmail
                .To(emailData.ToAddress)
                .Subject(emailData.Subject)
                .Body(emailData.Body)
                .SendAsync();
        }
    }
}
