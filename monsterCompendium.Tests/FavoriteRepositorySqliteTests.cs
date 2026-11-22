using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;
using monsterCompendium.Services;
using Xunit;

namespace monsterCompendium.Tests
{
    public class FavoriteRepositorySqliteTests : IAsyncLifetime
    {
        private DatabaseService _databaseService = null!;
        private FavoriteRepository _repository = null!;

        public async Task InitializeAsync()
        {
            _databaseService = new DatabaseService(":memory:");
            _repository = new FavoriteRepository(_databaseService);

            await _databaseService.InitializeAsync();
        }

        public async Task DisposeAsync()
        {
            await _databaseService.CloseAsync();
        }

        [Fact]
        public async Task AddAsync_MonstruoNuevo_LoPersisteEnSQLite()
        {
            // Act
            await _repository.AddAsync("goblin");

            // Assert
            bool exists = await _repository.ExistsAsync("goblin");
            Assert.True(exists);

            var favorites = await _repository.GetAllAsync();
            Assert.Single(favorites);
            Assert.Equal("goblin", favorites[0].Index);
        }

        [Fact]
        public async Task RemoveAsync_MonstruoExistente_LoEliminaDeSQLite()
        {
            // Arrange
            await _repository.AddAsync("goblin");

            bool existsBeforeRemove = await _repository.ExistsAsync("goblin");

            Assert.True(existsBeforeRemove);

            // Act
            await _repository.RemoveAsync("goblin");

            // Assert
            bool existsAfterRemove = await _repository.ExistsAsync("goblin");

            Assert.False(existsAfterRemove);

            var favorites = await _repository.GetAllAsync();

            Assert.Empty(favorites);
        }

        [Fact]
        public async Task AddAsync_MonstruoDuplicado_NoLoInsertaDosVecesEnSQLite()
        {
            // Arrange
            await _repository.AddAsync("goblin");

            // Act
            await _repository.AddAsync("goblin");

            // Assert
            var favorites = await _repository.GetAllAsync();

            Assert.Single(favorites);

            Assert.Equal("goblin", favorites[0].Index);

            bool exists = await _repository.ExistsAsync("goblin");

            Assert.True(exists);
        }

        [Fact]
        public async Task GetAllAsync_VariosFavoritos_LosDevuelveTodosDesdeSQLite()
        {
            // Arrange
            await _repository.AddAsync("goblin");
            await _repository.AddAsync("aboleth");
            await _repository.AddAsync("lich");

            // Act
            var favorites = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(3, favorites.Count);

            Assert.Contains(
                favorites,
                favorite => favorite.Index == "goblin");

            Assert.Contains(
                favorites,
                favorite => favorite.Index == "aboleth");

            Assert.Contains(
                favorites,
                favorite => favorite.Index == "lich");
        }

        [Fact]
        public async Task ExistsAsync_BaseVacia_RetornaFalse()
        {
            // Act
            bool exists = await _repository.ExistsAsync("goblin");

            // Assert
            Assert.False(exists);

            var favorites = await _repository.GetAllAsync();

            Assert.Empty(favorites);
        }

        [Fact]
        public async Task UpdatePhotoAsync_FavoritoExistente_PersistePhotoPathEnSQLite()
        {
            // Arrange
            await _repository.AddAsync("goblin");

            string photoPath =
                "/data/user/0/app/files/goblin.jpg";

            // Act
            await _repository.UpdatePhotoAsync("goblin", photoPath);

            // Assert
            var favorites = await _repository.GetAllAsync();

            var goblin =
                favorites.Single(f => f.Index == "goblin");

            Assert.Equal(photoPath, goblin.PhotoPath);
        }
    }
}
