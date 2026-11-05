using System.Text.Json.Serialization;

namespace AsistenteIA.Models;

public class ChatRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = [];
}

public class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class ChatResponse
{
    [JsonPropertyName("choices")]
    public List<ChatChoice> Choices { get; set; } = [];
}

public class ChatChoice
{
    [JsonPropertyName("message")]
    public ChatMessage Message { get; set; } = new();
}

public class ModelsResponse
{
    [JsonPropertyName("data")]
    public List<ModelInfo> Data { get; set; } = [];
}

public class ModelInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
