using Xamarin.Forms;

namespace XamCore.View
{
    // Have to use MasterDetailPage because of Prism
    public partial class MenuPage : MasterDetailPage, IMasterDetailPageController
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;
    }
}
