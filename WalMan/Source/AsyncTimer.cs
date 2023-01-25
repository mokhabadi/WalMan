using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WalMan
{
    internal class AsyncTimer
    {
        readonly int interval;
        DateTimeOffset startTime;
        DateTimeOffset suspendTime;
        CancellationTokenSource? cancellationTokenSource;

        public Action? Elapsed;

        public AsyncTimer(int interval, Action Elapsed)
        {
            Log.Add($"AsyncTimer {interval}");

            if (interval <= 0)
            {
                Elapsed?.Invoke();
                return;
            }

            this.interval = interval;
            this.Elapsed = Elapsed;
            startTime = DateTimeOffset.UtcNow;
            SystemEvents.PowerModeChanged += PowerModeChanged;
            Start(interval);
        }

        public int RemainingTime()
        {
            int remainingTime = interval - (int)(DateTimeOffset.UtcNow - startTime).TotalSeconds;
            return remainingTime > 0 ? remainingTime : 0;
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
                Start(RemainingTime());
            }
        }

        async void Start(int interval)
        {
            Log.Add($"Start: {interval}");
            cancellationTokenSource = new();

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(interval), cancellationTokenSource.Token);
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
