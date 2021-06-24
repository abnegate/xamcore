using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IDatabaseService
    {
        Task InitAsync();

        Task CreateTablesAsync();

        Task DeleteTablesAsync();

        Task DropTablesAsync();

        Task InsertAsync<T>(T item);

        Task InsertAllAsync<T>(IEnumerable<T> items);

        Task<int> DeleteAsync<T>(T item);

        Task DeleteAllAsync<T>() where T : new();

        Task<IEnumerable<T>?> GetItemsAsync<T>() where T : new();

        Task<bool> DoesTableExistAsync<T>();
    }
}
