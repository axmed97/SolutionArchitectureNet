using Core.Entities.Concrete;

namespace Core.Utilities.Security.Abstract
{
    public interface ITokenService
    {
        Task<Token> CreateAccessToken(AppUser appUser, List<string> roles);
        string CreateRefreshToken();
    }
}
