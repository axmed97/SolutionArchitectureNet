namespace Core.Utilities.Message.Abstract
{
    public interface ISmsService
    {
        Task<bool> SendOtpSmsAsync(string phoneNumber, string otp);
    }
}
