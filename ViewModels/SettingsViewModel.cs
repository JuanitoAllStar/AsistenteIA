using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Services;

namespace AsistenteIA.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ApiKeyService _keys;
    private readonly AiService _ai;

    public ObservableCollection<string> Models { get; } = [];

    [ObservableProperty] private string apiKey = string.Empty;      // Entry con IsPassword="True"
    [ObservableProperty] private string status = string.Empty;
    [ObservableProperty] private string keyState = "Clave: no configurada";
    [ObservableProperty] private string? selectedModel;
    [ObservableProperty] private bool isBusy;

    public SettingsViewModel(ApiKeyService keys, AiService ai)
    {
        _keys = keys;
        _ai = ai;
        Models.Add(AppSettings.Model);
        SelectedModel = AppSettings.Model;
    }

    partial void OnSelectedModelChanged(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value)) AppSettings.Model = value;
    }

    [RelayCommand]
    public async Task InitAsync()
    {
        await RefreshKeyStateAsync();
        await LoadModelsAsync();
    }

    private async Task RefreshKeyStateAsync() =>
        KeyState = await _keys.HasKeyAsync() ? "Clave: configurada (oculta)" : "Clave: no configurada";

    [RelayCommand]
    private async Task LoadModelsAsync()
    {
        var list = await _ai.GetModelsAsync();
        string current = AppSettings.Model;
        Models.Clear();
        foreach (var m in list) Models.Add(m);
        if (!Models.Contains(current)) Models.Insert(0, current);
        SelectedModel = current;
    }

    [RelayCommand]
    private async Task SaveKeyAsync()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            Status = "Ingrese una clave para guardarla.";
            return;
        }
        await _keys.SaveAsync(ApiKey);
        ApiKey = string.Empty;                 // la clave nunca se vuelve a mostrar
        await RefreshKeyStateAsync();
        Status = "Clave guardada de forma segura.";
    }

    [RelayCommand]
    private async Task TestAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        Status = "Probando conexión...";
        try
        {
            await _ai.AskAsync("Responde solamente: OK", AppSettings.Model);
            Status = "Conexión exitosa.";
        }
        catch (OperationCanceledException) { Status = "Tiempo agotado. Intente de nuevo."; }
        catch (HttpRequestException ex)
        {
            Status = ex.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden
                ? "Error de autorización: clave inválida."
                : $"Error de conexión: {(ex.StatusCode is null ? "sin respuesta" : ((int)ex.StatusCode).ToString())}";
        }
        catch (Exception ex) { Status = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task DeleteKeyAsync()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Eliminar clave", "¿Desea eliminar la clave guardada?", "Eliminar", "Cancelar");
        if (!confirm) return;
        _keys.Remove();
        await RefreshKeyStateAsync();
        Status = "Clave eliminada.";
    }
}
