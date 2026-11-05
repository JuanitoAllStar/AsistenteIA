using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Models;
using AsistenteIA.Services;

namespace AsistenteIA.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class DetailViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    [ObservableProperty] private int id;
    [ObservableProperty] private ConversationRecord? record;

    public DetailViewModel(DatabaseService db) => _db = db;

    async partial void OnIdChanged(int value) => Record = await _db.GetByIdAsync(value);

    // UPDATE del CRUD: marca la respuesta como verificada tras contrastarla.
    [RelayCommand]
    private async Task ToggleVerifiedAsync()
    {
        if (Record is null) return;
        Record.IsVerified = !Record.IsVerified;
        await _db.SaveAsync(Record);

        var updated = await _db.GetByIdAsync(Record.Id);
        Record = updated;
    }
}
