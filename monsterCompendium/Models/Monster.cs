using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace monsterCompendium.Models
{
    internal class Monster
    {
        [JsonPropertyName("index")]
        public string Index { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public string Size { get; set; } = string.Empty;

        [JsonPropertyName("hit_points")]
        public int HitPoints { get; set; }

        [JsonPropertyName("armor_class")]
        public List<ArmorClass> ArmorClass { get; set; } = new();

        [JsonPropertyName("strength")]
        public string Strength { get; set; } = string.Empty;

        [JsonPropertyName("dexterity")]
        public string Dexterity { get; set; } = string.Empty;

        [JsonPropertyName("constitution")]
        public string Constitution { get; set; } = string.Empty;

        [JsonPropertyName("intelligence")]
        public string Intelligence { get; set; } = string.Empty;

        [JsonPropertyName("wisdom")]
        public string Wisdom { get; set; } = string.Empty;

        [JsonPropertyName("charisma")]
        public string Charisma { get; set; } = string.Empty;
    }
}
