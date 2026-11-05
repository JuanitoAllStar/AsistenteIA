namespace AsistenteIA.Services;

/// <summary>Preferencias no sensibles (el modelo es configurable por si el proveedor lo cambia).</summary>
public static class AppSettings
{
    public const string DefaultModel = "openai/gpt-5.4-nano";

    public static string Model
    {
        get => Preferences.Default.Get("model", DefaultModel);
        set => Preferences.Default.Set("model", value);
    }
}
