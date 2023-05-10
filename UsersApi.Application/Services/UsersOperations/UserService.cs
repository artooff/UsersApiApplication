using UsersApi.Application.DTO;
using UsersApi.Application.Repositories;
using UsersApi.Application.Services.Helpers;
using UsersApi.Domain.Exceptions;
using X.PagedList;

namespace UsersApi.Application.Services.UsersOperations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAddChecker _checker;
        private readonly IPasswordHashService _passwordHashService;

        public UserService(IUserRepository userRepository,
            IUserAddChecker checker,
            IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _checker = checker;
            _passwordHashService = passwordHashService;
        }

        
        public async Task<IPagedList<GetUserDto>> GetUsersList(RequestParams requestParams)
        {
            return (await _userRepository.GetUsersAsync())
                .ToPagedList(requestParams.PageNumber, requestParams.PageSize);
        }

        public async Task<List<GetUserDto>> GetUsersByLogin(string login)
        {
            return (await _userRepository.GetUsersAsync())
                .Where(user => user.Login == login)
                .ToList();
        }

        public async Task<GetUserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException(id);
            }
            return user;
        }

        public async Task<int> AddUser(AddUserDto userModel)
        {
            if (!_checker.CheckLoginBlock(userModel))
            {
                throw new LoginBlockedException(userModel.Login);
            }
            try
            {
                userModel.Password = _passwordHashService.HashPassword(userModel.Password);
                return await _userRepository.AddUserAsync(userModel);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<int> DeleteUser(int userId)
        {
            try
            {
                return await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
