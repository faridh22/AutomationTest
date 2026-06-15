namespace Automation_APP.Services;

public sealed class GameLoopService : IDisposable
{
    IDispatcherTimer? timer;

    public event EventHandler? Tick;

    public TimeSpan Interval
    {
        get => timer?.Interval ?? TimeSpan.FromMilliseconds(16);
        set
        {
            if (timer is not null)
            {
                timer.Interval = value;
            }
        }
    }

    public void Start(IDispatcher dispatcher, TimeSpan interval)
    {
        Stop();
        timer = dispatcher.CreateTimer();
        timer.Interval = interval;
        timer.Tick += OnTick;
        timer.Start();
    }

    public void Stop()
    {
        if (timer is null)
        {
            return;
        }

        timer.Stop();
        timer.Tick -= OnTick;
        timer = null;
    }

    void OnTick(object? sender, EventArgs e) => Tick?.Invoke(this, EventArgs.Empty);

    public void Dispose() => Stop();
}
