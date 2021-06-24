using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XamCore.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T>? ToObservableCollection<T>(this IEnumerable<T>? collection) =>
            collection is null ? null : new ObservableCollection<T>(collection);

        public static void AddRange<T>(this ObservableCollection<T>? collection, IEnumerable<T>? source)
        {
            if (collection is null || source is null) {
                return;
            }
            foreach (T item in source) {
                collection.Add(item);
            }
        }

        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> collection) =>
            collection ?? Enumerable.Empty<T>();
    }
}
