using System.Runtime.Serialization;

namespace SpotPG.Models.Configuration;

[DataContract]
[Serializable]
public class SpotifyAccessConfiguration
{
    [DataMember(Name = "clientId")]
    public string ClientId { get; set; } = "ClientId";

    [DataMember(Name = "clientSecret")]
    public string ClientSecret { get; set; } = "ClientSecret";

    [DataMember(Name = "accessToken")]
    public string AccessToken { get; set; } = "AccessToken";

    [DataMember(Name = "refreshToken")]
    public string RefreshToken { get; set; } = "RefreshToken";
}