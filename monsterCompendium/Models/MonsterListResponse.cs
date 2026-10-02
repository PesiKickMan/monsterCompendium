using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace monsterCompendium.Models
{
    internal class MonsterListResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("results")]
        public List<MonsterSummary> Results { get; set; } = new();
    }
}
