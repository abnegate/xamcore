using System;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public class RetryService : IRetryService
    {
        public void WithRetries(
            int retryCount,
            Action onExecute,
            Action<Exception, int>? onError = null)
        {
            int tryCount = 0;

            while (tryCount <= retryCount) {
                try {
                    onExecute();
                    break;
                } catch (Exception e) {
                    onError?.Invoke(e, tryCount);
                    tryCount++;
                }
            }
        }

        public T? WithRetries<T>(
            int retryCount,
            Func<T> onExecute,
            Action<Exception, int>? onError = null)
        {
            int tryCount = 0;

            while (tryCount <= retryCount) {
                try {
                    return onExecute();
                } catch (Exception e) {
                    onError?.Invoke(e, tryCount);
                    tryCount++;
                }
            }
            return default;
        }

        public async Task WithRetriesAsync(
            int retryCount,
            Func<Task> onExecute,
            Action<Exception, int>? onError = null,
            int retryDelay = 0)
        {
            int tryCount = 0;

            while (tryCount <= retryCount) {
                try {
                    await onExecute();
                    break;
                } catch (Exception e) {
                    onError?.Invoke(e, tryCount);
                    if (retryDelay != 0) {
                        await Task.Delay(retryDelay);
                    }
                    tryCount++;
                }
            }
        }

        public async Task<T?> WithRetriesAsync<T>(
            int retryCount,
            Func<Task<T>> onExecute,
            Action<Exception, int>? onError = null,
            int retryDelay = 0)
        {
            int tryCount = 0;

            while (tryCount <= retryCount) {
                try {
                    return await onExecute();
                } catch (Exception e) {
                    onError?.Invoke(e, tryCount);
                    if (retryDelay != 0) {
                        await Task.Delay(retryDelay);
                    }
                    tryCount++;
                }
            }
            return default;
        }
    }
}
