using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace monsterCompendium.Models
{
    public class MonsterSummary
    {
        [JsonPropertyName("index")]
        public string Index { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
