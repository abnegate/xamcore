using XamCore.Services;
using XamCore.View;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Xamarin.Forms;
using DependencyService = XamCore.Services.DependencyService;

namespace XamCore
{
    public partial class XamCoreApplication : PrismApplication
    {
        public XamCoreApplication(IPlatformInitializer? platformInitializer = null) : base(platformInitializer) { }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TransparentNavigationPage>();

            containerRegistry.RegisterSingleton<IAnalyticsService, AnalyticsService>();
            containerRegistry.RegisterSingleton<IAppCloserService, AppCloserService>();
            containerRegistry.RegisterSingleton<IDatabaseService, DatabaseService>();
            containerRegistry.RegisterSingleton<IDependencyService, DependencyService>();
            containerRegistry.RegisterSingleton<IDeviceIdService, DeviceIdService>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<IKeyValueService, KeyValueService>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<IRetryService, RetryService>();
            containerRegistry.RegisterSingleton<ISQLiteService, SQLiteService>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
        }

        protected override void OnResume()
        {
        }

        protected override void OnSleep()
        {
        }
    }
}
