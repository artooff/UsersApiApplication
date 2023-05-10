using Microsoft.EntityFrameworkCore;
using UsersApi.Application.DTO;
using UsersApi.Application.Repositories;
using UsersApi.Application.Services.Helpers;
using UsersApi.Domain.Exceptions;
using UsersApi.Domain.Models;
using UsersApi.Infrastructure.Data;
using UsersApp.Domain.Models;

namespace UsersApi.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context,
            IUserAddChecker checker,
            IPasswordHashService passwordHashService)
        {
            _context = context;
        }

        public async Task<List<GetUserDto>> GetUsersAsync()
        {
            var records = await _context.Users
                .Include(x => x.UserState)
                .Include(x => x.UserGroup)
                .Select(x => new GetUserDto
                {
                    Id = x.Id,
                    Login = x.Login,
                    Password = x.Password,
                    CreatedDate = x.CreatedDate,
                    UserState = x.UserState,
                    UserGroup = x.UserGroup
                })
                .OrderBy(x => x.Id)
                .ToListAsync();

            return records;
        }

        public async Task<GetUserDto> GetUserByIdAsync(int id)
        {
            var record = await _context.Users
                .Include(x => x.UserState)
                .Include(x => x.UserGroup)
                .Where(x => x.Id == id)
                .Select(x => new GetUserDto
                {
                    Id = x.Id,
                    Login = x.Login,
                    Password = x.Password,
                    CreatedDate = x.CreatedDate,
                    UserState = x.UserState,
                    UserGroup = x.UserGroup
                })
                .FirstOrDefaultAsync();

            return record;
        }

        public async Task<int> AddUserAsync(AddUserDto userModel)
        {
            if (userModel.UserGroupCode == UserGroupCode.Admin && await CheckAdminExisting())
            {
                throw new InvalidOperationException("Cannot add another admin");
            }

            var userState = await _context.UserStates
                .FirstOrDefaultAsync(x => x.Code == UserStateCode.Active);
            var userGroup = await _context.UserGroups
                .FirstOrDefaultAsync(x => x.Code == userModel.UserGroupCode);

            try
            {
                var user = new User
                {
                    Login = userModel.Login,
                    Password = userModel.Password,
                    CreatedDate = DateTime.Now.ToUniversalTime(),
                    UserStateId = userState.Id,
                    UserGroupId = userGroup.Id
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user.Id;
            }
            catch (Exception)
            {
                throw new Exception("Internal server error");
            }
            
        }

        public async Task<int> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(userId);
            }

            var blockedUserState = await _context.UserStates.SingleAsync(x => x.Code == UserStateCode.Blocked);

            user.UserStateId = blockedUserState.Id;

            await _context.SaveChangesAsync();

            return user.Id;
        }

        private async Task<bool> CheckAdminExisting()
        {
            var existingAdmin = await _context.Users
                .Where(x =>
                x.UserGroup.Code == UserGroupCode.Admin)
                .FirstOrDefaultAsync();
            return existingAdmin != null;
        }
    }
}
