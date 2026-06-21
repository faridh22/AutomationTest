using System;

namespace SnacGame.Services;

public class DifficultyService
{
    private const double BaseIntervalMs = 200.0; // Starting interval (5 FPS)
    private const double MinIntervalMs = 16.67; // Max speed (60 FPS)
    private const double SpeedFactor = 5.0; // How much each score point affects the interval

    public double GetTickInterval(int score)
    {
        double newInterval = BaseIntervalMs - (score * SpeedFactor);
        return Math.Max(MinIntervalMs, newInterval);
    }
}
