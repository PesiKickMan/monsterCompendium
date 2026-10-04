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

        public ObservableCollection<Monster> Favorites { get; } = new();

        public FavoritesViewModel(
            IFavoriteRepository favoriteRepository,
            IMonsterApiService monsterApiService)
        {
            _favoriteRepository = favoriteRepository;
            _monsterApiService = monsterApiService;
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
                    var monster = await _monsterApiService.GetMonsterAsync(favorite.Index);

                    if (monster is not null)
                    {
                        Favorites.Add(monster);
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
        private async Task RemoveFavoriteAsync(Monster monster)
        {
            if (monster is null)
                return;

            try
            {
                await _favoriteRepository.RemoveAsync(monster.Index);

                Favorites.Remove(monster);

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
        private async Task SelectMonsterAsync(Monster monster)
        {
            if (monster is null)
                return;

            await Shell.Current.GoToAsync($"monsterdetails?index={monster.Index}");
        }
    }
}
