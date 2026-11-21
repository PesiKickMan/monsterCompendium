using System;
using System.Collections.Generic;
using System.Text;

namespace monsterCompendium.Services
{
    public interface IDeviceSensorService
    {
        event EventHandler? ShakeDetected;

        bool IsAccelerometerSupported { get; }

        void StartShakeDetection();

        void StopShakeDetection();

        void Vibrate();
    }
}
