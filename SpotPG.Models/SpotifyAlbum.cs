using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SpotPG.Models;

[Serializable]
[DataContract(Name = "spotifyAlbum")]
public class SpotifyAlbum
{
    [Key, DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "uri")]
    public string Uri { get; set; }

    [DataMember(Name = "externalUrl")]
    public string ExternalUrl { get; set; }

    [DataMember(Name = "releaseName")]
    public string ReleaseName { get; set; }

    [DataMember(Name = "artists")]
    public IList<string> Artists { get; set; }

    [DataMember(Name = "releaseDate")]
    public DateTime ReleaseDate { get; set; }

    [DataMember(Name = "coverUrl")]
    public string CoverUrl { get; set; }

    [DataMember(Name = "releaseType")]
    public ReleaseType ReleaseType { get; set; }
}