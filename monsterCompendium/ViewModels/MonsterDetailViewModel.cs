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

        [ObservableProperty]
        public Monster? selectedMonster;

        public MonsterDetailViewModel(IMonsterApiService monsterApiService)
        {
            _monsterApiService = monsterApiService;
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
                    StatusMessage = "No se pudo obtener la información del monstruo.";
                    return;
                }

                StatusMessage = "Se cargaron los detalles del monstruo correctamente.";
            }
            catch (HttpRequestException ex)
            when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                StatusMessage = "No se encontró el monstruo solicitado.";
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
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
