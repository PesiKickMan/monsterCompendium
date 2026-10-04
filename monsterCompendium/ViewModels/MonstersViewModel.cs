using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using monsterCompendium.Models;
using monsterCompendium.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace monsterCompendium.ViewModels
{
    public partial class MonstersViewModel : BaseViewModel
    {
        private readonly IMonsterApiService _monsterApiService;

        private List<MonsterSummary> _allMonsters = new();

        private const int PageSize = 20;

        private int _currentCount = 0;

        public ObservableCollection<MonsterSummary> Monsters { get; } = new();

        [ObservableProperty]
        private  string loadButtonText = "Cargar monstruos";

        public MonstersViewModel(IMonsterApiService monsterApiService)
        {
            _monsterApiService = monsterApiService;

            StatusMessage = "Presioná el botón para cargar monstruos.";
        }

        [RelayCommand]
        private async Task LoadMoreAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                StatusMessage = "Cargando monstruos...";

                if (_allMonsters.Count == 0)
                {
                    _allMonsters = await _monsterApiService.GetMonstersAsync();
                }

                var monstersToAdd = _allMonsters
                .Skip(_currentCount)
                .Take(PageSize)
                .ToList();

                foreach (var monster in monstersToAdd)
                {
                    Monsters.Add(monster);
                }

                _currentCount += monstersToAdd.Count;
                
                LoadButtonText = "Cargar más monstruos";

                StatusMessage =
                $"Mostrando {_currentCount} de {_allMonsters.Count} monstruos.";
            }
            catch (HttpRequestException ex)
            when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                StatusMessage =
                "No se encontraron los monstruos solicitados.";
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
                "Ocurrió un error inesperado al cargar los monstruos.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SelectMonsterAsync(MonsterSummary monster)
        {
            if (monster == null)
                return;

            await Shell.Current.GoToAsync(
            $"monsterdetails?index={monster.Index}");
        }
    }
}
