using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Services;

namespace monsterCompendium.Tests.Fakes
{
    public class FakeDiceService : IDiceService
    {
        public int NextRoll { get; set; } = 1;

        public int RollD20()
        {
            return NextRoll;
        }
    }
}
