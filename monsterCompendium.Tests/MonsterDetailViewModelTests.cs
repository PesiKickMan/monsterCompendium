using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
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

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.ToggleFavoriteCommand.ExecuteAsync(null);

            // Assert
            Assert.False(viewModel.IsFavorite);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);
        }

        // <---------- Async Load Tests ---------->

        [Fact]
        public async Task LoadMonster_MonstruoExistente_CargaSelectedMonster()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                MonsterToReturn = new Monster
                {
                    Index = "goblin",
                    Name = "Goblin"
                }
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.NotNull(viewModel.SelectedMonster);

            Assert.Equal("goblin", viewModel.SelectedMonster.Index);

            Assert.Equal("Goblin", viewModel.SelectedMonster.Name);
        }

        [Fact]
        public async Task LoadMonster_MonstruoYaFavorito_ActualizaEstadoDeFavorito()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                MonsterToReturn = new Monster
                {
                    Index = "goblin",
                    Name = "Goblin"
                }
            };

            var favoriteRepository = new FakeFavoriteRepository();

            await favoriteRepository.AddAsync("goblin");

            var viewModel = new MonsterDetailViewModel(
            apiService,
            favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.NotNull(viewModel.SelectedMonster);

            Assert.True(viewModel.IsFavorite);

            Assert.Equal(
            "Quitar de favoritos",
            viewModel.FavoriteButtonText);

            Assert.Equal(
            string.Empty,
            viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);
        }

        [Fact]
        public async Task LoadMonster_MonstruoNoFavorito_MantieneEstadoNoFavorito()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                MonsterToReturn = new Monster
                {
                    Index = "goblin",
                    Name = "Goblin"
                }
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            var selectedMonster = viewModel.SelectedMonster;

            Assert.NotNull(selectedMonster);

            Assert.Equal("goblin", selectedMonster.Index);

            Assert.Equal("Goblin", selectedMonster.Name);

            Assert.False(viewModel.IsFavorite);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal(string.Empty, viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);
        }

        [Fact]
        public async Task LoadMonster_ApiDevuelveNull_MuestraMensajeDeError()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                MonsterToReturn = null
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService,favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.Null(viewModel.SelectedMonster);

            Assert.False(viewModel.IsFavorite);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("No se pudo obtener la información del monstruo.", viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);
        }

        [Fact]
        public async Task LoadMonster_Error404_MuestraMensajeNoEncontrado()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                ExceptionToThrow = new HttpRequestException(
                    "Not Found",
                    null,
                    HttpStatusCode.NotFound)
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.Null(viewModel.SelectedMonster);

            Assert.False(viewModel.IsFavorite);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("No se encontró el monstruo solicitado.", viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);
        }

        [Fact]
        public async Task LoadMonster_Error500_MuestraCodigoHttp()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                ExceptionToThrow = new HttpRequestException(
                    "Internal Server Error",
                    null,
                    HttpStatusCode.InternalServerError)
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.Null(viewModel.SelectedMonster);

            Assert.False(viewModel.IsFavorite);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("Error HTTP: 500.", viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);
        }

        [Fact]
        public async Task LoadMonster_ErrorDeConexion_MuestraMensajeDeConexion()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                ExceptionToThrow = new HttpRequestException(
                    "Connection failed")
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.Null(viewModel.SelectedMonster);

            Assert.False(viewModel.IsFavorite);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("No se pudo conectar con el servidor. Revisá tu conexión.", viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);
        }

        [Fact]
        public async Task LoadMonster_ErrorInesperado_MuestraMensajeGenerico()
        {
            // Arrange
            var apiService = new StubMonsterApiService
            {
                ExceptionToThrow = new InvalidOperationException(
                    "Error inesperado de prueba")
            };

            var favoriteRepository = new FakeFavoriteRepository();

            var viewModel = new MonsterDetailViewModel(apiService, favoriteRepository);

            // Act
            await viewModel.LoadMonsterAsync("goblin");

            // Assert
            Assert.Null(viewModel.SelectedMonster);

            Assert.False(viewModel.IsFavorite);

            Assert.Equal("Agregar a favoritos", viewModel.FavoriteButtonText);

            Assert.Equal("Ocurrió un error inesperado al cargar el monstruo.", viewModel.StatusMessage);

            Assert.False(viewModel.IsBusy);

            var favorites = await favoriteRepository.GetAllAsync();

            Assert.Empty(favorites);
        }
    }
}