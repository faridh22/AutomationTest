using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Automation_APP.Controls;

public partial class HeaderView : ContentView
{
    public HeaderView()
    {
        InitializeComponent();
        BindingContext = this;
        SearchCommand = new Command(async () => await OnSearchAsync());
        NavigateCommand = new Command(async () => await OnNavigateAsync());
    }

    public string SearchText { get; set; } = string.Empty;
    public ICommand SearchCommand { get; }
    public int BoundNotificationCount { get; set; }
    public ICommand NavigateCommand { get; }

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

    async Task OnNavigateAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("NotificationsPage");
        }
        catch (Exception ex)
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
