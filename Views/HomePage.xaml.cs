namespace AsistenteIA.Views;

public partial class HomePage : ContentPage
{
    public HomePage() => InitializeComponent();

    private async void OnGoChat(object? s, EventArgs e) => await Shell.Current.GoToAsync("//ChatPage");
    private async void OnGoHistory(object? s, EventArgs e) => await Shell.Current.GoToAsync("//HistoryPage");
    private async void OnGoSettings(object? s, EventArgs e) => await Shell.Current.GoToAsync("//SettingsPage");
}
