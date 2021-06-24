using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IPlatformNotificationService
    {
        public Task StartService();

        public Task<string?> GetToken();
    }
}
