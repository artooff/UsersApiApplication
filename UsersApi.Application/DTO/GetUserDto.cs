using UsersApi.Domain.Models;

namespace UsersApi.Application.DTO
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserGroup UserGroup { get; set; }
        public UserState UserState { get; set; }
    }
}
