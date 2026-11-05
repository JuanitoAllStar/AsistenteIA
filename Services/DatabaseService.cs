using SQLite;
using AsistenteIA.Models;

namespace AsistenteIA.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _db;

    private async Task InitAsync()
    {
        if (_db is not null) return;
        string path = Path.Combine(FileSystem.AppDataDirectory, "asistenteia.db3");
        _db = new SQLiteAsyncConnection(path);
        await _db.CreateTableAsync<ConversationRecord>();
    }

    /// <summary>Crea el registro (Id = 0) o lo actualiza.</summary>
    public async Task<int> SaveAsync(ConversationRecord item)
    {
        await InitAsync();
        return item.Id == 0
            ? await _db!.InsertAsync(item)
            : await _db!.UpdateAsync(item);
    }

    public async Task<List<ConversationRecord>> GetAllAsync()
    {
        await InitAsync();
        var list = await _db!.Table<ConversationRecord>().ToListAsync();
        return list.OrderByDescending(x => x.CreatedAt).ToList();
    }

    public async Task<ConversationRecord?> GetByIdAsync(int id)
    {
        await InitAsync();
        return await _db!.Table<ConversationRecord>()
            .Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> DeleteAsync(ConversationRecord item)
    {
        await InitAsync();
        return await _db!.DeleteAsync(item);
    }
}
