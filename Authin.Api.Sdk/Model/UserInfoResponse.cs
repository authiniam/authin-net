using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model
{
    public class UserInfoResponse
    {
        [JsonProperty("sub")] public string Sub { get; set; }
        [JsonProperty("profile")] public string Profile { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("phone_number")] public string PhoneNumber { get; set; }
        [JsonProperty("preferred_name")] public string PreferredUsername { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("given_name")] public string GivenName { get; set; }
        [JsonProperty("family_name")] public string FamilyName { get; set; }
    }
}