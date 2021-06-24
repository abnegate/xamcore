using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamCore.Services
{
    public class MediaService : IMediaService
    {
        private readonly Lazy<IPlatformMediaService> _platformMediaService;

        public MediaService(IDependencyService dependencyService)
        {
            _platformMediaService = new(() => dependencyService.Get<IPlatformMediaService>());
        }

        public string StoragePath => Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);

        public async Task<string?> TakePicture(MediaPickerOptions? options = null)
        {
            var mediaResult = await MediaPicker.CapturePhotoAsync(options);

            return mediaResult.FullPath;
        }

        public Task<IEnumerable<string?>?> SelectPictures(int max = 15) =>
            _platformMediaService.Value.SelectPictures(max);
    }
}
