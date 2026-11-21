using System;
using System.Collections.Generic;
using System.Text;

namespace monsterCompendium.Services
{
    public class DiceService : IDiceService
    {
        public int RollD20()
        {
            return Random.Shared.Next(1, 21);
        }
    }
}
