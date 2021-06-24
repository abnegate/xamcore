using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using XamCore.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Essentials;

namespace XamCore.Services
{
    public class DatabaseService : IDatabaseService
    {
        private const int RetryCount = 2;

        private readonly IAnalyticsService _analyticsService;
        private readonly IRetryService _retryer;
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseService(
            IAnalyticsService analyticsService,
            ISQLiteService sqlLiteService,
            IRetryService retryService)
        {
            _analyticsService = analyticsService;
            _retryer = retryService;
            _connection = sqlLiteService.GetConnection();
        }

        public async Task InitAsync()
        {
            await CreateTablesAsync();
        }

        public async Task CreateTablesAsync()
        {
            await _retryer.WithRetriesAsync(
                RetryCount,
                () => _connection.RunInTransactionAsync((SQLiteConnection conn) =>
                    conn.CreateTables(
                        CreateFlags.None,
                        new[] {
                            typeof(string)
                        })),
                (ex, tryCount) => Catch(ex, tryCount));
        }

        public async Task DeleteTablesAsync()
        {
            await _retryer.WithRetriesAsync(RetryCount, () =>
               _connection.RunInTransactionAsync(conn => {

               }),
            (ex, tryCount) => Catch(ex, tryCount));
        }

        public async Task DropTablesAsync()
        {
            await _retryer.WithRetriesAsync(RetryCount, () =>
               _connection.RunInTransactionAsync(conn => {

               }),
            (ex, tryCount) => Catch(ex, tryCount));
        }

        public async Task InsertAsync<T>(T item)
        {
            await _retryer.WithRetriesAsync(RetryCount, () =>
                _connection.RunInTransactionAsync(conn =>
                    conn.InsertWithChildren(item)),
            (ex, tryCount) => Catch<T>(ex, tryCount));
        }

        public async Task InsertAllAsync<T>(IEnumerable<T> items)
        {
            await _retryer.WithRetriesAsync(RetryCount, () =>
                _connection.RunInTransactionAsync(conn =>
                    conn.InsertOrReplaceAllWithChildren(items, recursive: true)),
            (ex, tryCount) => Catch<T>(ex, tryCount));
        }

        public async Task<int> DeleteAsync<T>(T item)
        {
            return await _retryer.WithRetriesAsync(RetryCount, () =>
                _connection.DeleteAsync(item),
            (ex, tryCount) => Catch<T>(ex, tryCount));
        }

        public async Task DeleteAllAsync<T>() where T : new()
        {
            await _retryer.WithRetriesAsync(RetryCount, () =>
                _connection.RunInTransactionAsync(conn => conn.DeleteAll<T>()),
            (ex, tryCount) => Catch<T>(ex, tryCount));
        }

        public async Task<IEnumerable<T>?> GetItemsAsync<T>() where T : new()
        {
            return await _retryer.WithRetriesAsync(RetryCount, () =>
               _connection.GetAllWithChildrenAsync<T>(),
            (ex, tryCount) => Catch<T>(ex, tryCount));
        }

        public async Task<bool> DoesTableExistAsync<T>()
        {
            return await _retryer.WithRetriesAsync(RetryCount, async () =>
               await _connection.ExecuteScalarAsync<int>(
                    $"SELECT count(*) FROM sqlite_master where type = 'table' AND name = '{typeof(T).Name}'"
                ) > 0,
            (ex, tryCount) => Catch<T>(ex, tryCount));
        }

        private void Catch(
            Exception e,
            int attempt,
            [CallerMemberName] string? methodName = null)
        {
            _analyticsService.TrackException(e, new Dictionary<string, string?> {
                    { "Method", methodName ?? "unknown" },
                    { "Attempt", (attempt + 1).ToString() }
                });
            Debug.WriteLine(e.StackTrace);
        }


        private void Catch<T>(
            Exception e,
            int attempt,
            [CallerMemberName] string? methodName = null)
        {
            _analyticsService.TrackException(e, new Dictionary<string, string?> {
                    { "Type", typeof(T).Name },
                    { "Method", methodName ?? "unknown" },
                    { "Attempt", (attempt + 1).ToString() }
                });
            Debug.WriteLine(e.StackTrace);
        }
    }
}
