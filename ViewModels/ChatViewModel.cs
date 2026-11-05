using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Models;
using AsistenteIA.Services;

namespace AsistenteIA.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly AiService _aiService;
    private readonly DatabaseService _db;
    private CancellationTokenSource? _cts;

    [ObservableProperty] private string prompt = string.Empty;
    [ObservableProperty] private string answer = string.Empty;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private string promptCounter = "0/2000";
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private PromptTemplate? selectedTemplate;

    public List<PromptTemplate> Templates => PromptTemplates.All;

    public ChatViewModel(AiService aiService, DatabaseService db)
    {
        _aiService = aiService;
        _db = db;
    }

    partial void OnPromptChanged(string value) => PromptCounter = $"{value.Length}/2000";

    // La plantilla rellena el prompt, que el usuario puede editar (RF07).
    partial void OnSelectedTemplateChanged(PromptTemplate? value)
    {
        if (value is not null) Prompt = value.Text;
    }

    [RelayCommand]
    private async Task AskAsync()
    {
        if (IsBusy) return;
        _cts = new CancellationTokenSource();
        IsBusy = true;
        Answer = string.Empty;
        StatusMessage = "Consultando el servicio...";

        try
        {
            Answer = await _aiService.AskAsync(Prompt, AppSettings.Model, _cts.Token);
            StatusMessage = "Respuesta recibida. Verifique la información antes de usarla.";
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Consulta cancelada o tiempo agotado. Puede reintentar.";
        }
        catch (HttpRequestException ex)
        {
            StatusMessage = ex.StatusCode switch
            {
                System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden
                    => "Error de autorización (401/403): revise la clave en Configuración.",
                System.Net.HttpStatusCode.TooManyRequests
                    => "Se superó la cuota del servicio (429). Intente más tarde.",
                null => "No se pudo conectar con el servicio. Verifique su conexión.",
                _ => $"Error del servicio: {(int)ex.StatusCode} {ex.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    [RelayCommand]
    private void Cancel() => _cts?.Cancel();

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Answer))
        {
            StatusMessage = "No hay una respuesta para guardar.";
            return;
        }

        await _db.SaveAsync(new ConversationRecord
        {
            Prompt = Prompt.Trim(),
            Answer = Answer,
            Category = SelectedTemplate?.Category ?? "General"
        });
        StatusMessage = "Consulta guardada en el historial.";
    }

    [RelayCommand]
    private void Clear()
    {
        Prompt = string.Empty;
        Answer = string.Empty;
        SelectedTemplate = null;
        StatusMessage = string.Empty;
    }
}
