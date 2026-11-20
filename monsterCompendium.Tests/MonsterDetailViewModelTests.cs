using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;
using monsterCompendium.Tests.Fakes;
using monsterCompendium.ViewModels;

namespace monsterCompendium.Tests
{
    public class MonsterDetailViewModelTests
    {
        [Fact]
        public async Task ToggleFavorite_MonstruoNoFavorito_LoAgregaAFavoritos()
        {
            // Arrange
            var apiService = new StubMonsterApiService();
            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            viewModel.SelectedMonster = new Monster
            {
                Index = "goblin",
                Name = "Goblin"
            };

            // Act
            await viewModel.ToggleFavoriteCommand.ExecuteAsync(null);

            // Assert
            Assert.True(viewModel.IsFavorite);

            bool exists = await favoriteRepository.ExistsAsync("goblin");

            Assert.True(exists);

            Assert.Equal("Quitar de favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("Monstruo agregado a favoritos.", viewModel.StatusMessage);
        }

        [Fact]
        public async Task ToggleFavorite_MonstruoYaFavorito_LoEliminaDeFavoritos()
        {
            // Arrange
            var apiService = new StubMonsterApiService();
            var favoriteRepository = new FakeFavoriteRepository();

            await favoriteRepository.AddAsync("goblin");

            var viewModel = new MonsterDetailViewModel(apiService,  favoriteRepository);

            viewModel.SelectedMonster = new Monster
            {
                Index = "goblin",
                Name = "Goblin"
            };

            viewModel.IsFavorite = true;

            // Act
            await viewModel.ToggleFavoriteCommand.ExecuteAsync(null);

            // Assert
            Assert.False(viewModel.IsFavorite);

            bool exists = await favoriteRepository.ExistsAsync("goblin");

            Assert.False(exists);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("Monstruo eliminado de favoritos.", viewModel.StatusMessage);
        }

        [Fact]
        public async Task ToggleFavorite_SinMonstruoSeleccionado_NoHaceNada()
        {
            // Arrange
            var apiService = new StubMonsterApiService();
            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(
                apiService,
                favoriteRepository);

            // Act
            await viewModel.ToggleFavoriteCommand.ExecuteAsync(null);

            // Assert
            Assert.False(viewModel.IsFavorite);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);
        }
    }
}