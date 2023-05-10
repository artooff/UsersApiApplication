namespace UsersApi.Domain.Exceptions
{
    public class LoginBlockedException : Exception
    {
        public LoginBlockedException(string login)
            : base($"Adding a user with login \"{login}\" is temporarily blocked.") { }
    }
}
