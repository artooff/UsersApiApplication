namespace UsersApi.Application.Services.Helpers
{
    public interface IPasswordHashService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hash);
    }
}
