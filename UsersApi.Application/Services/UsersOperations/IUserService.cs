using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersApi.Application.DTO;
using X.PagedList;

namespace UsersApi.Application.Services.UsersOperations
{
    public interface IUserService
    {
        public Task<IPagedList<GetUserDto>> GetUsersList(RequestParams requestParams);
        public Task<List<GetUserDto>> GetUsersByLogin(string login);
        public Task<GetUserDto> GetUserById(int id);
        public Task<int> AddUser(AddUserDto userModel);
        public Task<int> DeleteUser(int userId);
    }
}
