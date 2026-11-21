using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Services;

namespace monsterCompendium.Tests.Fakes
{
    public class FakeDeviceSensorService : IDeviceSensorService
    {
        public event EventHandler? ShakeDetected;

        public bool IsAccelerometerSupported { get; set; } = true;

        public bool IsListening { get; private set; }

        public int VibrateCallCount { get; private set; }

        public void StartShakeDetection()
        {
            if (!IsAccelerometerSupported)
                return;

            IsListening = true;
        }

        public void StopShakeDetection()
        {
            IsListening = false;
        }

        public void Vibrate()
        {
            VibrateCallCount++;
        }

        public void TriggerShake()
        {
            if (!IsListening)
                return;

            ShakeDetected?.Invoke(
            this,
            EventArgs.Empty);
        }
    }
}
