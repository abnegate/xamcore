using System.Collections.Generic;
using System.Drawing;

namespace XamCore.Models
{
    public class StringKeyGroup<T> : Group<string, T>
    {
        public StringKeyGroup(
            string key,
            IEnumerable<T> items) : base(key, items)
        { }

        public StringKeyGroup(
            string key,
            IEnumerable<T> items,
            Color color) : base(key, items, color)
        { }
    }
}
