namespace WalMan
{
    internal class AsyncTimer
    {
        DateTimeOffset startTime;
        CancellationTokenSource cancellationTokenSource = new();

        public int ElapsedTime => (DateTimeOffset.UtcNow - startTime).Seconds;

        public event Func<Task>? Elapsed;

        public async void Start(int timeInterval)
        {
            startTime = DateTimeOffset.UtcNow;

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(timeInterval), cancellationTokenSource.Token);
                Elapsed?.Invoke();
            }
            catch
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
