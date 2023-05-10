using UsersApi.Application.DTO;

namespace UsersApi.Application.Repositories
{
    public interface IUserRepository
    {
        public Task<List<GetUserDto>> GetUsersAsync();
        public Task<GetUserDto> GetUserByIdAsync(int id);
        public Task<int> AddUserAsync(AddUserDto userModel);
        public Task<int> DeleteUserAsync(int userId);

    }
}
