namespace AsistenteIA.Services;

public class ApiKeyService
{
    private const string KeyName = "pollinations_api_key";

    public Task SaveAsync(string key) =>
        SecureStorage.Default.SetAsync(KeyName, key.Trim());

    public Task<string?> GetAsync() =>
        SecureStorage.Default.GetAsync(KeyName);

    public bool Remove() => SecureStorage.Default.Remove(KeyName);

    public async Task<bool> HasKeyAsync() =>
        !string.IsNullOrWhiteSpace(await GetAsync());
}
