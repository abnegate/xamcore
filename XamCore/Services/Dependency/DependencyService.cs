namespace XamCore.Services
{
    public class DependencyService : IDependencyService
    {
        public T Get<T>() where T : class =>
            Xamarin.Forms.DependencyService.Get<T>();
    }
}
