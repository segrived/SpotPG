using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SpotPG.Models;

[Serializable]
[DataContract(Name = "spotifyTrack")]
public class SpotifyTrack
{
    [Key, DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "uri")]
    public string Uri { get; set; }

    [DataMember(Name = "externalUrl")]
    public string ExternalUrl { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "artists")]
    public IList<string> Artists { get; set; }

    [DataMember(Name = "explicit")]
    public bool Explicit { get; set; }

    [DataMember(Name = "duration")]
    public TimeSpan Duration { get; set; }

    [DataMember(Name = "diskNumber")]
    public int DiskNumber { get; set; }

    [DataMember(Name = "trackNumber")]
    public int TrackNumber { get; set; }
}