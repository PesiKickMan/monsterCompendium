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

        [Fact]
        public async Task ExistsAsync_MonstruoNoAgregado_RetornaFalse()
        {
            // Arrange
            var repository = new FakeFavoriteRepository();
            string monsterIndex = "goblin";

            // Act
            bool exists = await repository.ExistsAsync(monsterIndex);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task RemoveAsync_MonstruoExistente_DejaDeSerFavorito()
        {
            // Arrange
            var repository = new FakeFavoriteRepository();
            string monsterIndex = "goblin";

            await repository.AddAsync(monsterIndex);

            // Act
            await repository.RemoveAsync(monsterIndex);

            // Assert
            bool exists = await repository.ExistsAsync(monsterIndex);

            Assert.False(exists);
        }

        [Fact]
        public async Task AddAsync_MismoMonstruoDosVeces_NoGeneraDuplicados()
        {
            // Arrange
            var repository = new FakeFavoriteRepository();
            string monsterIndex = "goblin";

            // Act
            await repository.AddAsync(monsterIndex);
            await repository.AddAsync(monsterIndex);

            // Assert
            var favorites = await repository.GetAllAsync();

            Assert.Single(favorites);
        }

        [Fact]
        public async Task GetAllAsync_VariosFavoritos_RetornaTodos()
        {
            // Arrange
            var repository = new FakeFavoriteRepository();

            await repository.AddAsync("goblin");
            await repository.AddAsync("aboleth");
            await repository.AddAsync("lich");

            // Act
            var favorites = await repository.GetAllAsync();

            // Assert
            Assert.Equal(3, favorites.Count);

            Assert.Contains(favorites, favorite => favorite.Index == "goblin");
            Assert.Contains(favorites, favorite => favorite.Index == "aboleth");
            Assert.Contains(favorites, favorite => favorite.Index == "lich");
        }
    }
}
