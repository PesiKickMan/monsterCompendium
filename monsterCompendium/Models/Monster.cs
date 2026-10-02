using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace monsterCompendium.Models
{
    public class Monster
    {
        [JsonPropertyName("index")]
        public string Index { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonIgnore]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? string.Empty : $"https://www.dnd5eapi.co{Image}";

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public string Size { get; set; } = string.Empty;

        [JsonPropertyName("hit_points")]
        public int HitPoints { get; set; }

        [JsonPropertyName("armor_class")]
        public List<ArmorClass> ArmorClass { get; set; } = new();

        [JsonPropertyName("strength")]
        public int Strength { get; set; };

        [JsonPropertyName("dexterity")]
        public int Dexterity { get; set; }

        [JsonPropertyName("constitution")]
        public int Constitution { get; set; }

        [JsonPropertyName("intelligence")]
        public int Intelligence { get; set; }

        [JsonPropertyName("wisdom")]
        public int Wisdom { get; set; }

        [JsonPropertyName("charisma")]
        public int Charisma { get; set; }
    }
}
