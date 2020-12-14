using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SpotPG.Models
{
    [Serializable]
    [DataContract(Name = "spotifyPlaylist")]
    public class SpotifyPlaylist
    {
        [Key, DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [DataMember(Name = "externalUrl")]
        public string ExternalUrl { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "coverUrl")]
        public string CoverUrl { get; set; }

        [DataMember(Name = "isPublic")]
        public bool? IsPublic { get; set; }

        [DataMember(Name = "tracks")]
        public IEnumerable<SpotifyTrack> Tracks { get; set; }
    }
}