using UsersApi.Domain.Models;

namespace UsersApi.Application.DTO
{
    public class UserGroupDto
    {
        public UserGroupCode Code { get; set; }
        public string Description { get; set; }
    }
}
