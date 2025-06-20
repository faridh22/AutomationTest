using CommunityToolkit.Mvvm.ComponentModel;

namespace Automation_APP.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    int notifications = 0;
}
