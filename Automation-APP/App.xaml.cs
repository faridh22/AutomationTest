using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Automation_APP;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
