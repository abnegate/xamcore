using System;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IRetryService
    {
        void WithRetries(
            int retryCount,
            Action onExecute,
            Action<Exception, int>? onError = null);

        T? WithRetries<T>(
            int retryCount,
            Func<T> onExecute,
            Action<Exception, int>? onError = null);

        Task WithRetriesAsync(
            int retryCount,
            Func<Task> onExecute,
            Action<Exception, int>? onError = null,
            int retryDelay = 0);

        Task<T?> WithRetriesAsync<T>(
            int retryCount,
            Func<Task<T>> onExecute,
            Action<Exception, int>? onError = null,
            int retryDelay = 0);
    }
}
