using monsterCompendium.Models;
using monsterCompendium.Tests.Fakes;
using monsterCompendium.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

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

        [Fact]
        public async Task AddPhotoAsync_FotoCapturada_ActualizaPhotoPath()
        {
            // Arrange
            var favoriteRepository = new FakeFavoriteRepository();

            await favoriteRepository.AddAsync("goblin");

            var monsterApiService = new StubMonsterApiService();

            var photoService =
                new FakePhotoService
                {
                    PhotoPathToReturn =
                        "/fake/photos/goblin.jpg"
                };

            var viewModel =
                new FavoritesViewModel(
                    favoriteRepository,
                    monsterApiService,
                    photoService);

            var monster = new Monster
            {
                Index = "goblin",
                Name = "Goblin"
            };

            var favorite = new FavoriteItem(monster, null);

            // Act
            await viewModel.AddPhotoCommand.ExecuteAsync(favorite);

            // Assert
            Assert.Equal("/fake/photos/goblin.jpg", favorite.PhotoPath);

            Assert.Equal(1, photoService.CaptureCallCount);

            Assert.Equal("goblin", photoService.LastMonsterIndex);
        }

        [Fact]
        public async Task AddPhotoAsync_SinFoto_NoActualizaPhotoPath()
        {
            // Arrange
            var favoriteRepository = new FakeFavoriteRepository();

            await favoriteRepository.AddAsync("goblin");

            var monsterApiService = new StubMonsterApiService();

            var photoService =
                new FakePhotoService
                {
                    PhotoPathToReturn = null
                };

            var viewModel =
                new FavoritesViewModel(
                    favoriteRepository,
                    monsterApiService,
                    photoService);

            var monster = new Monster
            {
                Index = "goblin",
                Name = "Goblin"
            };

            var favorite =new FavoriteItem(monster, null);

            // Act
            await viewModel.AddPhotoCommand.ExecuteAsync(favorite);

            // Assert
            Assert.Null(favorite.PhotoPath);

            Assert.Equal(1, photoService.CaptureCallCount);

            Assert.Equal("No se agregó ninguna foto.", viewModel.StatusMessage);
        }

        [Fact]
        public async Task AddPhotoAsync_FotoExistente_ReemplazaPhotoPath()
        {
            // Arrange
            var favoriteRepository = new FakeFavoriteRepository();

            await favoriteRepository.AddAsync("goblin");

            await favoriteRepository.UpdatePhotoAsync("goblin", "/fake/photos/old-goblin.jpg");

            var monsterApiService = new StubMonsterApiService();

            var photoService =
                new FakePhotoService
                {
                    PhotoPathToReturn =
                        "/fake/photos/goblin.jpg"
                };

            var viewModel =
                new FavoritesViewModel(
                    favoriteRepository,
                    monsterApiService,
                    photoService);

            var monster = new Monster
            {
                Index = "goblin",
                Name = "Goblin"
            };

            var favorite = new FavoriteItem(monster, "/fake/photos/old-goblin.jpg");

            // Act
            await viewModel.AddPhotoCommand.ExecuteAsync(favorite);

            // Assert
            Assert.Equal("/fake/photos/goblin.jpg", favorite.PhotoPath);

            var savedFavorites =await favoriteRepository.GetAllAsync();

            var savedGoblin =
                savedFavorites.Single(f => f.Index == "goblin");

            Assert.Equal("/fake/photos/goblin.jpg", savedGoblin.PhotoPath);
        }
    }
}
