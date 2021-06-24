using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamCore.Services
{
    public interface IMediaService : IPlatformMediaService
    {
        string StoragePath { get; }

        Task<string?> TakePicture(MediaPickerOptions? options = null);
    }
}
