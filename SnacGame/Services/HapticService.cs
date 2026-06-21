using Microsoft.Maui.Devices.Sensors;

namespace SnacGame.Services;

public interface IHapticService
{
    void PlaySuccessFeedback();
    void PlayErrorFeedback();
}

public class HapticService : IHapticService
{
    public void PlaySuccessFeedback()
    {
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
    }

    public void PlayErrorFeedback()
    {
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
    }
}
