using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WalMan
{
    internal class AsyncTimer
    {
        readonly int timeInterval;
        DateTimeOffset startTime;
        DateTimeOffset suspendTime;
        CancellationTokenSource? cancellationTokenSource;

        public int RemainingTime => timeInterval - (int)(DateTimeOffset.UtcNow - startTime).TotalSeconds;

        public Action Elapsed;

        public AsyncTimer(int timeInterval, Action Elapsed)
        {
            Log.Add($"AsyncTimer {timeInterval}");
            this.timeInterval = timeInterval;
            this.Elapsed = Elapsed;
            startTime = DateTimeOffset.UtcNow;
            SystemEvents.PowerModeChanged += PowerModeChanged;
            Start(this.timeInterval);
        }

        void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Log.Add($"PowerModeChanged: {e.Mode}");

            if (e.Mode == PowerModes.Suspend)
            {
                cancellationTokenSource?.Cancel();
                suspendTime = DateTimeOffset.UtcNow;
            }

            if (e.Mode == PowerModes.Resume)
            {
                startTime += DateTimeOffset.UtcNow - suspendTime;
                Start(RemainingTime);
            }
        }

        async void Start(int timeInterval)
        {
            Log.Add($"Start: {timeInterval}");
            cancellationTokenSource = new();

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(timeInterval), cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                Log.Add($"AsyncTimer Exception: {exception.Message}");
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                return;
            }

            Elapsed?.Invoke();
            Stop();
        }

        public void Stop()
        {
            SystemEvents.PowerModeChanged -= PowerModeChanged;
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }
    }
}
