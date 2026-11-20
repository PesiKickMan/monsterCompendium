using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Tests.Fakes;

namespace monsterCompendium.Tests
{
    public class FavoriteRepositoryFakeTests
    {
        [Fact]
        public async Task AddAsync_MonstruoNuevo_QuedaComoFavorito()
        {
            // Arrange
            var repository = new FakeFavoriteRepository();
            string monsterIndex = "goblin";

            // Act
            await repository.AddAsync(monsterIndex);

            // Assert
            bool exists = await repository.ExistsAsync(monsterIndex);

            Assert.True(exists);
        }
    }
}
