using SQLite;

namespace AsistenteIA.Models;

public class ConversationRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Category { get; set; } = "General";
    public bool IsVerified { get; set; }

    [Ignore]
    public string CreatedLocal => CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");

    [Ignore]
    public string VerifiedText => IsVerified
        ? "Verificada con fuentes confiables"
        : "Sin verificar: contraste con fuentes confiables";
}
