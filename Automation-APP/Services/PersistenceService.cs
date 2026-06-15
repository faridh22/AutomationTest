using Microsoft.Maui.Storage;

namespace Automation_APP.Services;

public sealed class PersistenceService
{
    const string HighScoreKey = "high_score";

    public int GetHighScore() => Preferences.Default.Get(HighScoreKey, 0);

    public void SaveHighScore(int score)
    {
        if (score > GetHighScore())
        {
            Preferences.Default.Set(HighScoreKey, score);
        }
    }
}
