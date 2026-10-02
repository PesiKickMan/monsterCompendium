using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace monsterCompendium.Models
{
    public class ArmorClass
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}
