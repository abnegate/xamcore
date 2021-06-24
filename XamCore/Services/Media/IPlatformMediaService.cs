using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IPlatformMediaService
    {
        Task<IEnumerable<string?>?> SelectPictures(int max = 15);
    }
}
