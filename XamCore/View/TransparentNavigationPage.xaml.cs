using Xamarin.Forms;

namespace XamCore.View
{
    public partial class TransparentNavigationPage : NavigationPage
    {
        public TransparentNavigationPage() : base()
        {
            InitializeComponent();
        }

        public TransparentNavigationPage(Page root) : base(root)
        {
            InitializeComponent();
        }

        public bool IgnoreLayoutChange { get; set; } = false;

        protected override void OnSizeAllocated(double width, double height)
        {
            if (!IgnoreLayoutChange)
                base.OnSizeAllocated(width, height);
        }
    }
}
