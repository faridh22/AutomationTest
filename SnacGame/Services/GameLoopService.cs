using System;
using System.Timers;

namespace SnacGame.Services;

public class GameLoopService
{
    private readonly Timer _timer;
    public event EventHandler? Tick;

    public GameLoopService()
    {
        _timer = new Timer(200.0);
        _timer.Elapsed += (s, e) => Tick?.Invoke(this, EventArgs.Empty);
    }

    public void SetInterval(double intervalMs)
    {
        _timer.Interval = intervalMs;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }
}
