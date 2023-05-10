using System.Security;
using UsersApi.Application.DTO;
using UsersApi.Application.Repositories;
using UsersApi.Application.Services.Helpers;
using UsersApi.Application.Services.UsersOperations;

namespace UsersApi.Application.Services.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHashService _passwordHashService;

        public AuthenticateService(IUserService userService, IPasswordHashService passwordHashService)
        {
            _userService = userService;
            _passwordHashService = passwordHashService;
        }

        public async Task<GetUserDto> Authenticate(string login, string password)
        {
            var users = await _userService.GetUsersByLogin(login);
            foreach (var user in users)
            {
                if (_passwordHashService.VerifyPassword(password, user.Password))
                {
                    return user;
                }
            }
            throw new SecurityException();
        }
    }
}
