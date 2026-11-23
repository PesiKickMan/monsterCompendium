using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.Input;
using monsterCompendium.Models;
using monsterCompendium.Services;

namespace monsterCompendium.ViewModels
{
    public partial class FavoritesViewModel : BaseViewModel
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMonsterApiService _monsterApiService;
        private readonly IPhotoService _photoService;

        public ObservableCollection<FavoriteItem> Favorites { get; } = new();

        public FavoritesViewModel(
            IFavoriteRepository favoriteRepository,
            IMonsterApiService monsterApiService,
            IPhotoService photoService)
        {
            _favoriteRepository = favoriteRepository;
            _monsterApiService = monsterApiService;
            _photoService = photoService;
        }

        [RelayCommand]
        private async Task LoadFavoritesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                StatusMessage = "Cargando favoritos...";

                Favorites.Clear();

                var savedFavorites =
                    await _favoriteRepository.GetAllAsync();

                if (savedFavorites.Count == 0)
                {
                    StatusMessage =
                        "Todavía no agregaste monstruos a favoritos.";

                    return;
                }

                foreach (var favorite in savedFavorites)
                {
                    var monster =await _monsterApiService.GetMonsterAsync(favorite.Index);

                    if (monster is not null)
                    {
                        Favorites.Add(new FavoriteItem(monster, favorite.PhotoPath));
                    }
                }

                StatusMessage =
                    $"Tenés {Favorites.Count} monstruos favoritos.";
            }
            catch (HttpRequestException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                StatusMessage =
                    "No se pudo encontrar uno de los monstruos favoritos.";
            }
            catch (HttpRequestException ex)
                when (ex.StatusCode != null)
            {
                StatusMessage =
                    $"Error HTTP: {(int)ex.StatusCode.Value}.";
            }
            catch (HttpRequestException)
            {
                StatusMessage =
                    "No se pudo conectar con el servidor. Revisá tu conexión.";
            }
            catch (Exception)
            {
                StatusMessage =
                    "Ocurrió un error inesperado al cargar los favoritos.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RemoveFavoriteAsync(
            FavoriteItem favorite)
        {
            if (favorite is null)
                return;

            try
            {
                await _favoriteRepository.RemoveAsync(favorite.Monster.Index);

                Favorites.Remove(favorite);

                if (Favorites.Count == 0)
                {
                    StatusMessage =
                        "Todavía no agregaste monstruos a favoritos.";
                }
                else
                {
                    StatusMessage =
                        $"Tenés {Favorites.Count} monstruos favoritos.";
                }
            }
            catch (Exception)
            {
                StatusMessage =
                    "Ocurrió un error al eliminar el favorito.";
            }
        }

        [RelayCommand]
        private async Task SelectMonsterAsync(FavoriteItem favorite)
        {
            if (favorite is null)
                return;

            await Shell.Current.GoToAsync($"monsterdetails?index={favorite.Monster.Index}");
        }

        [RelayCommand]
        private async Task AddPhotoAsync(FavoriteItem favorite)
        {
            if (favorite is null)
                return;

            try
            {
                string? photoPath =await _photoService.CapturePhotoAsync(favorite.Monster.Index);

                if (string.IsNullOrWhiteSpace(photoPath))
                {
                    StatusMessage =
                        "No se agregó ninguna foto.";

                    return;
                }

                await _favoriteRepository.UpdatePhotoAsync(favorite.Monster.Index, photoPath);

                favorite.PhotoPath = photoPath;

                StatusMessage =
                    $"Foto actualizada para {favorite.Monster.Name}.";
            }
            catch (Exception ex)
            {
                StatusMessage =
                    $"Ocurrió un error al agregar la foto: {ex.GetType().FullName} - {ex.Message}";
            }
        }
    }
}
