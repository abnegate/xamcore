using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace XamCore.Services
{
    public class KeyValueService : IKeyValueService
    {
        public T? Get<T>(string key, T? defualtValue = default)
        //where T : new()
        {
            var stringVal = Preferences.Get(key, null);
            if (stringVal is null) {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(stringVal);
        }

        public dynamic? Get(
            Type objType,
            string key,
            object? defualtValue = null)
        {
            var stringVal = Preferences.Get(key, null);
            if (stringVal is null) {
                return default;
            }
            return JsonConvert.DeserializeObject(stringVal, objType);
        }


        public async Task<T?> GetSecureAsync<T>(
            string key,
            T? defualtValue = default)
        //where T : new()
        {
            // Don't store in a var here
            return JsonConvert.DeserializeObject<T>(
                await SecureStorage.GetAsync(key) ?? string.Empty);
        }

        public async Task<dynamic?> GetSecureAsync(
            Type objType,
            string key,
            object? defualtValue = null)
        {
            return JsonConvert.DeserializeObject(
                await SecureStorage.GetAsync(key) ?? string.Empty, objType);
        }


        public void Set<T>(string key, T? value)
        {
            Preferences.Set(key, JsonConvert.SerializeObject(value));
        }

        public void Set(string key, object? value)
        {
            Preferences.Set(key, JsonConvert.SerializeObject(value));
        }


        public async Task SetSecureAsync<T>(string key, T? value)
        {
            await SecureStorage.SetAsync(key,
                JsonConvert.SerializeObject(value));
        }

        public async Task SetSecureAsync(string key, object? value)
        {
            await SecureStorage.SetAsync(key,
                JsonConvert.SerializeObject(value));
        }
    }
}
