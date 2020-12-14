using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SpotPG.Services.SpotifyCredentialsManager.Models
{
    public class AppConfigItem
    {
        [Key]
        public int Id { get; set; }

        [DataMember(Name="key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}