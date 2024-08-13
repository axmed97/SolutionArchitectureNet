using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AuthDTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(RegisterDTO model);
        Task<IDataResult<Token>> LoginAsync(LoginDTO model);
        Task<IDataResult<string>> UpdateRefreshToken(string refreshToken, AppUser user);
        Task<IDataResult<Token>> RefreshTokenLoginAsync(RefreshTokenDTO refreshToken);
        Task<IResult> LogOutAsync(string userId);
        IDataResult<List<UserDTO>> GetAll();
        Task<IResult> RemoveUserAsync(string userId);
    }
}
