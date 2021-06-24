using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace XamCore.Models
{
    public class Group<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public Color HeaderBackgroundColor { get; set; }

        public Group(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items) {
                Items.Add(item);
            }
        }

        public Group(K key, IEnumerable<T> items, Color color)
        {
            Key = key;
            HeaderBackgroundColor = color;
            foreach (var item in items) {
                Items.Add(item);
            }
        }
    }
}
