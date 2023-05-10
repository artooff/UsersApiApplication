using UsersApi.Application.DTO;

namespace UsersApi.Application.Services.Authentication
{
    public interface IAuthenticateService
    {
        public Task<GetUserDto> Authenticate(string login, string password);
    }
}
