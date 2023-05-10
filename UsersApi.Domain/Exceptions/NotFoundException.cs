namespace UsersApi.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(int id)
            : base($"User with id \"{id}\" was not found.") { }
    }
}
