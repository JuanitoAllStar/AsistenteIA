using AsistenteIA.ViewModels;

namespace AsistenteIA.Views;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _vm;

    public HistoryPage(HistoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _vm = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}
