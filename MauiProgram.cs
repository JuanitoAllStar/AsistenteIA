using AsistenteIA.Services;
using AsistenteIA.ViewModels;
using AsistenteIA.Views;

namespace AsistenteIA;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // HttpClient reutilizable (sin claves en el código)
        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("https://gen.pollinations.ai/v1/"),
            Timeout = TimeSpan.FromSeconds(60)
        });

        // Servicios
        builder.Services.AddSingleton<ApiKeyService>();
        builder.Services.AddSingleton<AiService>();
        builder.Services.AddSingleton<DatabaseService>();

        // ViewModels
        builder.Services.AddTransient<ChatViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Vistas
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<ChatPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
