using System.Text.Json.Serialization;
using UsersApi.Domain.Models;

namespace UsersApi.Application.DTO
{
    public class AddUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserGroupCode UserGroupCode { get; set; }
    }
}
