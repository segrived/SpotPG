using System;
using System.Runtime.Serialization;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services.Configuration.Models
{
    [DataContract]
    [Serializable]
    public class ServiceConfiguration : IServiceConfiguration
    {
        [DataMember(Name = "spotifyAccess")]
        public SpotifyAccessConfiguration SpotifyAccess { get; set; } = new();
    }
}