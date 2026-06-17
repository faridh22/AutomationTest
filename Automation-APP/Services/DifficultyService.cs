using Microsoft.Maui.Graphics;

namespace Automation_APP.Services;

public sealed class DifficultyService
{
    readonly List<PointF> obstacles = new();

    public IReadOnlyList<PointF> Obstacles => obstacles;

    public TimeSpan GetTickInterval(int score, bool slowMotionActive)
    {
        var interval = Math.Max(65, 160 - (score / 50 * 15));
        return TimeSpan.FromMilliseconds(slowMotionActive ? interval * 1.8 : interval);
    }

    public IReadOnlyList<PointF> UpdateObstacles(int score, int gridWidth, int gridHeight)
    {
        var desiredCount = score / 100;
        while (obstacles.Count < desiredCount)
        {
            var x = 2 + (obstacles.Count * 5 % Math.Max(3, gridWidth - 4));
            var y = 3 + (obstacles.Count * 7 % Math.Max(3, gridHeight - 6));
            obstacles.Add(new PointF(x, y));
        }

        return obstacles;
    }

    public void Reset() => obstacles.Clear();
}
