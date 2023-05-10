using UsersApi.Application.DTO;

namespace UsersApi.Application.Services.Helpers
{
    public interface IUserAddChecker
    {
        public bool CheckLoginBlock(AddUserDto userModel);
    }
}
