using System.Text.Json.Serialization;

namespace GrpcTraining.Resources.OpenDota
{
    public class OpenDotaHero
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("localized_name")]
        public string? LocalizedName { get; set; }

        [JsonPropertyName("primary_attr")]
        public string? PrimaryAttr { get; set; }

        [JsonPropertyName("attack_type")]
        public string? AttackType { get; set; }

    }
}
