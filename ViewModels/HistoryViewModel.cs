using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Models;
using AsistenteIA.Services;

namespace AsistenteIA.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly DatabaseService _db;
    private List<ConversationRecord> _all = [];

    public ObservableCollection<ConversationRecord> Items { get; } = [];

    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private string emptyMessage = string.Empty;

    public HistoryViewModel(DatabaseService db) => _db = db;

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    [RelayCommand]
    public async Task LoadAsync()
    {
        _all = await _db.GetAllAsync();
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        string q = SearchText.Trim();
        var result = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(x =>
                    x.Prompt.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    x.Answer.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    x.Category.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

        Items.Clear();
        foreach (var item in result) Items.Add(item);

        EmptyMessage = Items.Count == 0
            ? (string.IsNullOrEmpty(q) ? "Aún no hay consultas guardadas." : "Sin resultados para la búsqueda.")
            : string.Empty;
    }

    [RelayCommand]
    private async Task DeleteAsync(ConversationRecord item)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Eliminar consulta", "¿Desea eliminar esta consulta del historial?", "Eliminar", "Cancelar");
        if (!confirm) return;

        await _db.DeleteAsync(item);
        await LoadAsync();
    }

    [RelayCommand]
    private Task OpenAsync(ConversationRecord item) =>
        Shell.Current.GoToAsync($"{nameof(Views.DetailPage)}?id={item.Id}");
}
