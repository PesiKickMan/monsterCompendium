using monsterCompendium.Tests.Fakes;
using monsterCompendium.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace monsterCompendium.Tests
{
    public class DiceRollerViewModelTests
    {
        [Fact]
        public void RollCommand_DadoDevuelve20_ActualizaResultA20()
        {
            // Arrange
            var diceService = new FakeDiceService
            {
                NextRoll = 20
            };

            var sensorService = new FakeDeviceSensorService();

            var viewModel =
                new DiceRollerViewModel(
                    diceService,
                    sensorService);

            // Act
            viewModel.RollCommand.Execute(null);

            // Assert
            Assert.Equal(20, viewModel.Result);

            Assert.Equal(1, sensorService.VibrateCallCount);
        }

        [Fact]
        public void RollCommand_NuevaTirada_ActualizaResultadoAnterior()
        {
            // Arrange
            var diceService = new FakeDiceService
            {
                NextRoll = 5
            };

            var sensorService = new FakeDeviceSensorService();

            var viewModel =
                new DiceRollerViewModel(
                    diceService,
                    sensorService);

            viewModel.RollCommand.Execute(null);

            Assert.Equal(5, viewModel.Result);

            diceService.NextRoll = 17;

            // Act
            viewModel.RollCommand.Execute(null);

            // Assert
            Assert.Equal(17, viewModel.Result);

            Assert.Equal(2, sensorService.VibrateCallCount);
        }

        [Fact]
        public void ShakeDetected_CuandoEstaEscuchando_TiraElDado()
        {
            // Arrange
            var diceService = new FakeDiceService
            {
                NextRoll = 14
            };

            var sensorService = new FakeDeviceSensorService();

            var viewModel =
                new DiceRollerViewModel(
                    diceService,
                    sensorService);

            viewModel.StartListening();

            // Act
            sensorService.TriggerShake();

            // Assert
            Assert.Equal(14, viewModel.Result);

            Assert.Equal(1, sensorService.VibrateCallCount);
        }

        [Fact]
        public void StartListening_IniciaDeteccionDeShake()
        {
            // Arrange
            var diceService = new FakeDiceService();

            var sensorService = new FakeDeviceSensorService();

            var viewModel =
                new DiceRollerViewModel(
                    diceService,
                    sensorService);

            // Act
            viewModel.StartListening();

            // Assert
            Assert.True(sensorService.IsListening);
        }

        [Fact]
        public void StopListening_DetieneDeteccionDeShake()
        {
            // Arrange
            var diceService = new FakeDiceService();

            var sensorService = new FakeDeviceSensorService();

            var viewModel =
            new DiceRollerViewModel(
                diceService,
                sensorService);

            viewModel.StartListening();

            Assert.True(sensorService.IsListening);

            // Act
            viewModel.StopListening();

            // Assert
            Assert.False(sensorService.IsListening);
        }

        [Fact]
        public void ShakeDetected_CuandoNoEstaEscuchando_NoTiraElDado()
        {
            // Arrange
            var diceService = new FakeDiceService
            {
                NextRoll = 12
            };

            var sensorService = new FakeDeviceSensorService();

            var viewModel =
                new DiceRollerViewModel(
                    diceService,
                    sensorService);

            // Act
            sensorService.TriggerShake();

            // Assert
            Assert.Equal(0, viewModel.Result);

            Assert.Equal(0, sensorService.VibrateCallCount);
        }
    }
}
