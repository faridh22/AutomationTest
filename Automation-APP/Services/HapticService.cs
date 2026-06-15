using Microsoft.Maui.Devices;

namespace Automation_APP.Services;

public sealed class HapticService
{
    public void Click()
    {
        if (HapticFeedback.Default.IsSupported)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
    }

    public void LongPress()
    {
        if (HapticFeedback.Default.IsSupported)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        }
    }
}
