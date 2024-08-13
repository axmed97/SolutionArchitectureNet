using Business.Abstract;
using Business.Messages.Abstract;
using Business.ValidationRules.FluenValidation.AuthValidations;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Security.Abstract;
using Entities.Common;
using Entities.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILocalizationService _localizationService;
        public AuthManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILocalizationService localizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _localizationService = localizationService;
        }

        public IDataResult<List<UserDTO>> GetAll()
        {
            var result = _userManager.Users.OfType<AppUser>().ToList();

            List<UserDTO> userDTO = result.Select(x => new UserDTO()
            {
                Username = x.UserName,
                Email = x.Email,
                Id = x.Id
            }).ToList();

            return new SuccessDataResult<List<UserDTO>>(data: userDTO, statusCode: HttpStatusCode.OK);
        }

        [ValidationAspect(typeof(LoginValidator))]
        public async Task<IDataResult<Token>> LoginAsync(LoginDTO model)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var user = await _userManager.FindByNameAsync(model.EmailOrUsername);

            user ??= await _userManager.FindByEmailAsync(model.EmailOrUsername);

            if (user == null)
                return new ErrorDataResult<Token>(_localizationService.GetLocalizedString("UserNotFound", langCode), HttpStatusCode.NotFound);


            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            var roles = await _userManager.GetRolesAsync(user);

            if (result.Succeeded)
            {
                Token token = await _tokenService.CreateAccessToken(user, roles.ToList());
                var response = await UpdateRefreshToken(refreshToken: token.RefreshToken, user);
                if (response.Success)
                    return new SuccessDataResult<Token>(data: token, statusCode: HttpStatusCode.OK, message: response.Message);
                else
                    return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: response.Message);
            }
            else
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("UserNotFound", langCode));
        }

        public async Task<IResult> LogOutAsync(string userId)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var findUser = await _userManager.FindByIdAsync(userId);
            if (findUser == null)
                return new ErrorResult(statusCode: HttpStatusCode.NotFound, message: _localizationService.GetLocalizedString("UserNotFound", langCode));
            findUser.RefreshToken = null;
            findUser.RefreshTokenExpiredDate = null;
            var result = await _userManager.UpdateAsync(findUser);
            if (result.Succeeded)
            {
                return new SuccessResult(statusCode: HttpStatusCode.OK);
            }
            else
            {
                string responseMessage = string.Empty;
                foreach (var error in result.Errors)
                {
                    responseMessage += error + ". ";
                };
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: responseMessage);
            }
        }

        public async Task<IDataResult<Token>> RefreshTokenLoginAsync(RefreshTokenDTO refreshToken)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken.RefreshToken);
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null && user?.RefreshTokenExpiredDate > DateTime.UtcNow.AddHours(4))
            {
                Token token = await _tokenService.CreateAccessToken(user, roles.ToList());
                token.RefreshToken = refreshToken.RefreshToken;
                return new SuccessDataResult<Token>(data: token, statusCode: HttpStatusCode.OK);
            }
            else
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("UserNotFound", langCode));
        }

        [ValidationAspect(typeof(RegisterValidator))]
        public async Task<IResult> RegisterAsync(RegisterDTO model)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;

            var checkEmail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            var checkUserName = await _userManager.FindByNameAsync(model.Username);

            if (checkEmail != null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("EmailAlreadyExists", langCode));

            if (checkUserName != null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: "");

            User newUser = new()
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                UserName = model.Username,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, model.Password);

            if (identityResult.Succeeded)
                return new SuccessResult(message: _localizationService.GetLocalizedString("RegistrationSuccess", langCode), statusCode: HttpStatusCode.Created);
            else
            {
                string responseMessage = string.Empty;
                foreach (var error in identityResult.Errors)
                    responseMessage += $"{error.Description}. ";
                return new ErrorResult(message: responseMessage, HttpStatusCode.BadRequest);
            }
        }

        public async Task<IResult> RemoveUserAsync(string userId)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;

            var findUser = await _userManager.FindByIdAsync(userId);
            if (findUser == null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("UserNotFound", langCode));

            var result = await _userManager.DeleteAsync(findUser);

            if (result.Succeeded)
                return new SuccessResult(HttpStatusCode.OK);
            else
            {
                string response = string.Empty;
                foreach (var error in result.Errors)
                {
                    response += error.Description + ". ";
                }
                return new ErrorResult(message: response, HttpStatusCode.BadRequest);
            }
        }

        public async Task<IDataResult<string>> UpdateRefreshToken(string refreshToken, AppUser user)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;

            if (user is not null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiredDate = DateTime.UtcNow.AddMonths(1);

                IdentityResult identityResult = await _userManager.UpdateAsync(user);

                if (identityResult.Succeeded)
                    return new SuccessDataResult<string>(statusCode: HttpStatusCode.OK, data: refreshToken);
                else
                {
                    string responseMessage = string.Empty;
                    foreach (var error in identityResult.Errors)
                        responseMessage += $"{error.Description}. ";
                    return new ErrorDataResult<string>(message: responseMessage, HttpStatusCode.BadRequest);
                }
            }
            else
                return new ErrorDataResult<string>(_localizationService.GetLocalizedString("UserNotFound", langCode), HttpStatusCode.NotFound);
        }
    }
}
