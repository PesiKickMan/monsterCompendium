using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Devices.Sensors;

namespace monsterCompendium.Services
{
    public class DeviceSensorService : IDeviceSensorService
    {
        public event EventHandler? ShakeDetected;

        public bool IsAccelerometerSupported =>
            Accelerometer.Default.IsSupported;

        public DeviceSensorService()
        {
            Accelerometer.Default.ShakeDetected += OnShakeDetected;
        }

        public void StartShakeDetection()
        {
            if (!IsAccelerometerSupported)
                return;

            if (Accelerometer.Default.IsMonitoring)
                return;

            Accelerometer.Default.Start(SensorSpeed.Game);
        }

        public void StopShakeDetection()
        {
            if (!IsAccelerometerSupported)
                return;

            if (!Accelerometer.Default.IsMonitoring)
                return;

            Accelerometer.Default.Stop();
        }

        public void Vibrate()
        {
            if (!Vibration.Default.IsSupported)
                return;

            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(150));
        }

        private void OnShakeDetected(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ShakeDetected?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
