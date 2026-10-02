using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;

namespace monsterCompendium.Services
{
    public interface IMonsterApiService
    {
        Task<List<MonsterSummary>> GetMonstersAsync();
        Task<Monster?> GetMonsterAsync(string index);
    }
}
