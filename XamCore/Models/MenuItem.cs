namespace XamCore.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string NavigationName { get; set; }
        public string? IconImageSource { get; set; }

        public MenuItem(
            string title,
            string navigationName,
            string? iconImageSource = null)
        {
            Title = title;
            NavigationName = navigationName;
            IconImageSource = iconImageSource;
        }
    }
}
