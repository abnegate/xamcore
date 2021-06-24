using System;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IPlatformDialogService
    {
        Task<bool> ShowBoldFormattedConfirmAsync(
            string title,
            string messageFormatString,
            string[] boldFormatArgs,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null);

        Task<string?> ShowInputDialogAsync(
            string title,
            string message,
            string? inputHint,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null);
    }
}
