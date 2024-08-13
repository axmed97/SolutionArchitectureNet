using Core.Entities.Concrete;

namespace Core.Utilities.Message.Abstract
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMetadata emailData);
    }
}
