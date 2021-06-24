using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IDialogService : IPlatformDialogService
    {
        Task ShowAlertAsync(
            string? title,
            string? message,
            string? buttonLabel = null);

        Task<bool> ShowConfirmAsync(
            string? title,
            string? message,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null);

        void ShowToast(string message);

        Task ShowNoInternetDialog();

        Task ShowServerDownDialog();

        Task ShowTimeoutDialog();

        Task ShowUnknownErrorDialog();
    }
}
