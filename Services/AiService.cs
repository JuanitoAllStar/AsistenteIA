using System.Net.Http.Headers;
using System.Net.Http.Json;
using AsistenteIA.Models;

namespace AsistenteIA.Services;

public class AiService
{
    private readonly HttpClient _client;
    private readonly ApiKeyService _keyService;

    public AiService(HttpClient client, ApiKeyService keyService)
    {
        _client = client;
        _keyService = keyService;
    }

    public async Task<string> AskAsync(
        string prompt,
        string model,
        CancellationToken token = default)
    {
        string cleanPrompt = prompt.Trim();
        if (cleanPrompt.Length == 0)
            throw new ArgumentException("Escriba una consulta antes de enviar.");
        if (cleanPrompt.Length < 5)
            throw new ArgumentException("La consulta es demasiado corta.");
        if (cleanPrompt.Length > 2000)
            throw new ArgumentException("La consulta supera 2000 caracteres.");

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            throw new InvalidOperationException("Sin conexión a internet. Revise su red e intente de nuevo.");

        string? apiKey = await _keyService.GetAsync();
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Configure la clave de la API en la pestaña Configuración.");

        var requestBody = new ChatRequest
        {
            Model = model,
            Messages =
            [
                new ChatMessage
                {
                    Role = "system",
                    Content = "Eres un asistente académico. Responde de forma clara, " +
                              "indica incertidumbre y no inventes fuentes."
                },
                new ChatMessage { Role = "user", Content = cleanPrompt }
            ]
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(requestBody);

        using HttpResponseMessage response = await _client.SendAsync(request, token);
        response.EnsureSuccessStatusCode();

        ChatResponse? result = await response.Content
            .ReadFromJsonAsync<ChatResponse>(cancellationToken: token);

        string? answer = result?.Choices.FirstOrDefault()?.Message.Content;
        return string.IsNullOrWhiteSpace(answer)
            ? "El servicio no devolvió contenido."
            : answer.Trim();
    }

    /// <summary>Consulta los modelos disponibles. Si falla, la app usa una lista local.</summary>
    public async Task<List<string>> GetModelsAsync(CancellationToken token = default)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "models");
            string? apiKey = await _keyService.GetAsync();
            if (!string.IsNullOrWhiteSpace(apiKey))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            using var response = await _client.SendAsync(request, token);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<ModelsResponse>(cancellationToken: token);
            var ids = data?.Data.Select(m => m.Id).Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
            if (ids is { Count: > 0 }) return ids;
        }
        catch (Exception) { /* se usa la lista local */ }

        return [AppSettings.DefaultModel, "openai", "openai-fast"];
    }
}
