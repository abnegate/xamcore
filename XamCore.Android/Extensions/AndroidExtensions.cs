using System.Collections.Generic;
using Android.OS;
using NETAPI.Extensions;

namespace XamCore.Droid
{
    public static class AndroidExtensions
    {
        public static Bundle? ToBundle(this IDictionary<string, string?>? dictionary)
        {
            var bundle = new Bundle();
            if (dictionary is null) {
                return null;
            }
            foreach (var param in dictionary) {
                bundle.PutString(param.Key, param.Value);
            }
            return bundle;
        }

        public static Dictionary<string, string> ToDictionary(this Bundle bundle)
        {
            var dictionary = new Dictionary<string, string>();
            if (bundle?.KeySet() is null) {
                return dictionary;
            }
            foreach (var key in bundle.KeySet()?.OrEmpty()!) {
                var value = bundle.Get(key);
                if (value is not null) {
                    dictionary.Add(key, value.ToString());
                }
            }
            return dictionary;
        }
    }
}
