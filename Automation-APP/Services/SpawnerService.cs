using Automation_APP.Models;
using Microsoft.Maui.Graphics;

namespace Automation_APP.Services;

public sealed class SpawnerService
{
    readonly Random random = new();

    public PowerUp? ActivePowerUp { get; private set; }
    public bool SlowMotionActive { get; private set; }

    public PowerUp? TrySpawn(int score, int gridWidth, int gridHeight, IReadOnlyCollection<PointF> occupied)
    {
        if (ActivePowerUp is not null || score == 0 || score % 70 != 0)
        {
            return ActivePowerUp;
        }

        PointF position;
        do
        {
            position = new PointF(random.Next(gridWidth), random.Next(gridHeight));
        }
        while (occupied.Contains(position));

        ActivePowerUp = new PowerUp(PowerUpType.SlowMotionBerry, position, TimeSpan.FromSeconds(6));
        return ActivePowerUp;
    }

    public async Task ActivateAsync(PowerUp powerUp, CancellationToken cancellationToken)
    {
        if (powerUp.Type == PowerUpType.SlowMotionBerry)
        {
            SlowMotionActive = true;
            ActivePowerUp = null;
            try
            {
                await Task.Delay(powerUp.Duration, cancellationToken);
            }
            finally
            {
                SlowMotionActive = false;
            }
        }
    }

    public void Reset()
    {
        ActivePowerUp = null;
        SlowMotionActive = false;
    }
}
