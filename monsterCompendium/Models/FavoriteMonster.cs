using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace monsterCompendium.Models
{
    [Table("FavoriteMonsters")]
    public class FavoriteMonster
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Index { get; set; } = string.Empty;
    }
}
