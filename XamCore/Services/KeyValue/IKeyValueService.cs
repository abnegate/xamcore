using System;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IKeyValueService
    {
        public T? Get<T>(string key, T? defualtValue = default);
        //where T : new();

        public dynamic? Get(Type objType, string key, object? defualtValue = default);

        public Task<T?> GetSecureAsync<T>(
            string key,
            T? defualtValue = default);
        //where T : new();

        public Task<dynamic?> GetSecureAsync(
            Type objType,
            string key,
            object? defualtValue = default);

        public void Set<T>(string key, T? value);
        public void Set(string key, object? value);

        public Task SetSecureAsync<T>(string key, T? value);
        public Task SetSecureAsync(string key, object? value);
    }
}
