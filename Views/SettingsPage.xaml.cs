using AsistenteIA.ViewModels;

namespace AsistenteIA.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _vm;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _vm = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InitAsync();
    }
}
