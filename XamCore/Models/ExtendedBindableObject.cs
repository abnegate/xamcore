using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace XamCore.Models
{
    public abstract class ExtendedBindableObject : BindableObject
    {
        internal void SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string name = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                OnPropertyChanged(name);
            }
        }
    }
}
