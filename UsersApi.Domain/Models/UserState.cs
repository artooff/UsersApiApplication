namespace UsersApi.Domain.Models
{
    public enum UserStateCode
    {
        Active,
        Blocked
    }
    public class UserState
    {
        public int Id { get; set; }
        public UserStateCode Code { get; set; }
        public string Description { get; set; }
    }
}
