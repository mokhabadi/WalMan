namespace WalMan
{
    internal class AsyncTimer
    {
        readonly DateTimeOffset startTime;
        readonly int timeInterval;

        public int RemainingTime => timeInterval - (DateTimeOffset.UtcNow - startTime).Seconds;

        public Action Elapsed;

        public AsyncTimer(int timeInterval, Action Elapsed)
        {
            this.timeInterval = timeInterval;
            this.Elapsed = Elapsed;
            startTime = DateTimeOffset.UtcNow;
            Start(timeInterval);
        }

        async void Start(int timeInterval)
        {
            await Task.Delay(TimeSpan.FromSeconds(timeInterval));
            Elapsed();
        }
    }
}
