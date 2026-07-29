using Microsoft.Maui.Devices;

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
        if (HapticFeedback.Default.IsSupported)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
    }

    public void PlayErrorFeedback()
    {
        if (HapticFeedback.Default.IsSupported)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        }
    }
}
