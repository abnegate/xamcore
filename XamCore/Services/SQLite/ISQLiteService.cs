using SQLite;

namespace XamCore.Services
{
    public interface ISQLiteService
    {
        SQLiteAsyncConnection GetConnection();
    }
}
