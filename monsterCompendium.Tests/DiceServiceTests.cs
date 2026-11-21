using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Services;

namespace monsterCompendium.Tests
{
    public class DiceServiceTests
    {
        [Fact]
        public void RollD20_MultiplesTiradas_SiempreRetornaValorEntre1Y20()
        {
            // Arrange
            var diceService = new DiceService();

            // Act & Assert
            for (int i = 0; i < 1000; i++)
            {
                int result = diceService.RollD20();

                Assert.InRange(result, 1, 20);
            }
        }
    }
}
