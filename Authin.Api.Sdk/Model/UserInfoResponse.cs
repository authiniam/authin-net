using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model
{
    public class UserInfoResponse
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("profile")] public string Profile { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("phoneNumber")] public string PhoneNumber { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("firstName")] public string FirstName { get; set; }
        [JsonProperty("lastName")] public string LastName { get; set; }
    }
}