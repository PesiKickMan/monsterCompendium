using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;
using monsterCompendium.Services;

namespace monsterCompendium.Tests.Fakes
{
    public class StubMonsterApiService : IMonsterApiService
    {
        public Monster? MonsterToReturn { get; set; }

        public Task<List<MonsterSummary>> GetMonstersAsync()
        {
            return Task.FromResult(new List<MonsterSummary>());
        }

        public Task<Monster?> GetMonsterAsync(string index)
        {
            return Task.FromResult(MonsterToReturn);
        }
    }
}
