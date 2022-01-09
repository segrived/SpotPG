using System.Runtime.Serialization;

namespace SpotPG.Models.Configuration;

[DataContract]
[Serializable]
public class ServiceConfiguration
{
    [DataMember(Name = "spotifyAccess")]
    public SpotifyAccessConfiguration SpotifyAccess { get; set; } = new();
}