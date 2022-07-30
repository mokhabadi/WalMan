namespace WalMan
{
    internal class AsyncTimer
    {
        readonly DateTimeOffset startTime;

        public int ElapsedTime => (DateTimeOffset.UtcNow - startTime).Seconds;

        public event Action? Elapsed;

        public AsyncTimer(int timeInterval)
        {
            startTime = DateTimeOffset.UtcNow;
            Start(timeInterval);
        }

        async void Start(int timeInterval)
        {
            await Task.Delay(TimeSpan.FromSeconds(timeInterval));
            Elapsed?.Invoke();
        }
    }
}
