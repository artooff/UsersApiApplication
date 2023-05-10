namespace UsersApi.Domain.Models
{
    public enum UserGroupCode
    {
        Admin,
        User
    }
    public class UserGroup
    {
        public int Id { get; set; }
        public UserGroupCode Code { get; set; }
        public string Description { get; set; }
    }
}
