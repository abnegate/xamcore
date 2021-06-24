using System;
namespace XamCore.Services
{
    public class DeviceIdService : IDeviceIdService
    {
        private readonly Lazy<IDeviceIdService> _platformIdService;

        public DeviceIdService(IDependencyService dependencyService)
        {
            _platformIdService = new(() =>
                dependencyService.Get<IDeviceIdService>());
        }

        public string? Id => _platformIdService.Value.Id;
    }
}
