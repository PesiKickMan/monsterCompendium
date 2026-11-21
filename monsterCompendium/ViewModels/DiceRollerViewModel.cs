using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using monsterCompendium.Services;

namespace monsterCompendium.ViewModels
{
    public partial class DiceRollerViewModel : ObservableObject
    {
        private readonly IDiceService _diceService;
        private readonly IDeviceSensorService _deviceSensorService;

        [ObservableProperty]
        private int result;

        public DiceRollerViewModel(
        IDiceService diceService,
        IDeviceSensorService deviceSensorService)
        {
            _diceService = diceService;
            _deviceSensorService = deviceSensorService;

            _deviceSensorService.ShakeDetected += OnShakeDetected;
        }

        [RelayCommand]
        private void Roll()
        {
            Result = _diceService.RollD20();

            _deviceSensorService.Vibrate();
        }

        public void StartListening()
        {
            _deviceSensorService.StartShakeDetection();
        }

        public void StopListening()
        {
            _deviceSensorService.StopShakeDetection();
        }

        private void OnShakeDetected(
        object? sender,
        EventArgs e)
        {
            Roll();
        }
    }
}
