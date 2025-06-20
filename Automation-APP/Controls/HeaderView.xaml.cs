using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Automation_APP.Controls;

public partial class HeaderView : ContentView
{
    public HeaderView()
    {
        InitializeComponent();
        BindingContext = this;
    }

    [ObservableProperty]
    string searchText = string.Empty;

    [ObservableProperty]
    ICommand searchCommand = new AsyncRelayCommand(OnSearchAsync);

    [ObservableProperty]
    int boundNotificationCount;

    public static readonly BindableProperty NotificationCountProperty =
        BindableProperty.Create(nameof(NotificationCount), typeof(int), typeof(HeaderView), 0,
            propertyChanged: (b, o, n) =>
            {
                var control = (HeaderView)b;
                control.BoundNotificationCount = (int)n;
            });

    public int NotificationCount
    {
        get => (int)GetValue(NotificationCountProperty);
        set => SetValue(NotificationCountProperty, value);
    }

    [ObservableProperty]
    ICommand navigateCommand = new AsyncRelayCommand(OnNavigateAsync);

    async Task OnNavigateAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("NotificationsPage");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    async Task OnSearchAsync()
    {
        try
        {
            await Shell.Current.DisplayAlert("Search", $"Searching for {SearchText}", "OK");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
