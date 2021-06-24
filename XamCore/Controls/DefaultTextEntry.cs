using Xamarin.Forms;

namespace XamCore.Controls
{
    public class DefaultTextEntry : Entry
    {
        public static readonly BindableProperty DefaultTextOnSelectedProperty = BindableProperty.Create(
            nameof(DefaultTextOnSelected),
            typeof(string),
            typeof(DefaultTextEntry),
            string.Empty);

        public string DefaultTextOnSelected
        {
            get => (string)GetValue(DefaultTextOnSelectedProperty);
            set => SetValue(DefaultTextOnSelectedProperty, value);
        }
    }
}