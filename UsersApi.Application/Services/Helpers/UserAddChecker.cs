using Microsoft.Extensions.Caching.Memory;
using UsersApi.Application.DTO;

namespace UsersApi.Application.Services.Helpers
{
    public class UserAddChecker : IUserAddChecker
    {
        private readonly IMemoryCache _cache;

        public UserAddChecker(IMemoryCache cache)
        {
            _cache = cache;
        }
        public bool CheckLoginBlock(AddUserDto userModel)
        {
            var cacheKey = $"user_lock_{userModel.Login}";

            if (_cache.TryGetValue(cacheKey, out _))
            {
                return false;
            }

            _cache.Set(cacheKey, true, TimeSpan.FromSeconds(5));
            return true;
        }


    }
}
