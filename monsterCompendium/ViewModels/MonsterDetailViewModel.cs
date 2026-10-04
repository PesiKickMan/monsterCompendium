using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using monsterCompendium.Models;
using monsterCompendium.Services;
using Microsoft.Maui.Controls;

namespace monsterCompendium.ViewModels
{
    public partial class MonsterDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IMonsterApiService _monsterApiService;
        private readonly IFavoriteRepository _favoriteRepository;

        [ObservableProperty]
        private Monster? selectedMonster;

        [ObservableProperty]
        private bool isFavorite;

        [ObservableProperty]
        private string favoriteButtonText = "Agregar a favoritos";

        public MonsterDetailViewModel(IMonsterApiService monsterApiService, IFavoriteRepository favoriteRepository)
        {
            _monsterApiService = monsterApiService;
            _favoriteRepository = favoriteRepository;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("index", out var index))
            {
                _ = LoadMonsterAsync(index.ToString());
            }
        }

        private async Task LoadMonsterAsync(string? index)
        {
            if (string.IsNullOrWhiteSpace(index) || IsBusy)
                return;

            try
            {
                IsBusy = true;

                StatusMessage = "Cargando monstruo...";

                SelectedMonster = await _monsterApiService.GetMonsterAsync(index);

                if (SelectedMonster is null)
                {
                    StatusMessage =
                        "No se pudo obtener la información del monstruo.";

                    return;
                }

                IsFavorite = await _favoriteRepository.ExistsAsync(index);

                UpdateFavoriteButtonText();

                StatusMessage = string.Empty;
            }
            catch (HttpRequestException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                StatusMessage =
                    "No se encontró el monstruo solicitado.";
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
                    "Ocurrió un error inesperado al cargar el monstruo.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ToggleFavoriteAsync()
        {
            if (SelectedMonster is null)
                return;

            try
            {
                if (IsFavorite)
                {
                    await _favoriteRepository.RemoveAsync(
                        SelectedMonster.Index);

                    IsFavorite = false;

                    StatusMessage =
                        "Monstruo eliminado de favoritos.";
                }
                else
                {
                    await _favoriteRepository.AddAsync(
                        SelectedMonster.Index);

                    IsFavorite = true;

                    StatusMessage =
                        "Monstruo agregado a favoritos.";
                }

                UpdateFavoriteButtonText();
            }
            catch (Exception)
            {
                StatusMessage =
                    "Ocurrió un error al actualizar favoritos.";
            }
        }

        private void UpdateFavoriteButtonText()
        {
            FavoriteButtonText = IsFavorite
                ? "Quitar de favoritos"
                : "Agregar a favoritos";
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
