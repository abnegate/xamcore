using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace XamCore.Services
{
    public class DialogService : IDialogService
    {
        private bool _isShowing = false;

        private readonly Lazy<IPlatformDialogService> _platformDialogService;

        public DialogService(IDependencyService dependencyService)
        {
            _platformDialogService = new(() => dependencyService.Get<IPlatformDialogService>());
        }

        #region Cross platform dialogs
        public async Task ShowAlertAsync(
            string? title,
            string? message,
            string? buttonLabel = null)
        {
            if (_isShowing) {
                return;
            }
            _isShowing = true;

            buttonLabel ??= AppResources.Ok;

            await UserDialogs.Instance.AlertAsync(
                message,
                title,
                buttonLabel);

            _isShowing = false;
        }

        public async Task<bool> ShowConfirmAsync(
            string? title,
            string? message,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            if (_isShowing) {
                return false;
            }
            _isShowing = true;

            okButtonLabel ??= AppResources.Ok;
            cancelButtonLabel ??= AppResources.Cancel;

            var result = await UserDialogs.Instance.ConfirmAsync(
                message,
                title,
                okButtonLabel,
                cancelButtonLabel);

            _isShowing = false;

            return result;
        }

        public void ShowToast(string message) =>
            UserDialogs.Instance.Toast(message);

        public async Task ShowNoInternetDialog()
        {
            if (_isShowing) {
                return;
            }
            _isShowing = true;

            await UserDialogs.Instance.AlertAsync(
                AppResources.NoData,
                AppResources.Sorry,
                AppResources.Ok);

            _isShowing = false;
        }

        public async Task ShowServerDownDialog()
        {
            if (_isShowing) {
                return;
            }
            _isShowing = true;

            await UserDialogs.Instance.AlertAsync(
                AppResources.ErrorApi,
                AppResources.Sorry,
                AppResources.Ok);

            _isShowing = false;
        }

        public async Task ShowTimeoutDialog()
        {
            if (_isShowing) {
                return;
            }
            _isShowing = true;

            await UserDialogs.Instance.AlertAsync(
                AppResources.ErrorTimeout,
                null,
                AppResources.Ok);

            _isShowing = false;
        }

        public async Task ShowUnknownErrorDialog()
        {
            if (_isShowing) {
                return;
            }
            _isShowing = true;

            await UserDialogs.Instance.AlertAsync(
                AppResources.ErrorUnknown,
                AppResources.Sorry,
                AppResources.Ok);

            _isShowing = false;
        }


        #endregion

        #region Platform specific dialogs
        public async Task<bool> ShowBoldFormattedConfirmAsync(
            string title,
            string messageFormatString,
            string[] boldFormatArgs,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            if (_isShowing) {
                return false;
            }
            _isShowing = true;

            okButtonLabel ??= AppResources.Ok;
            cancelButtonLabel ??= AppResources.Cancel;

            var result = await _platformDialogService.Value.ShowBoldFormattedConfirmAsync(
                title,
                messageFormatString,
                boldFormatArgs,
                okButtonLabel,
                cancelButtonLabel);

            _isShowing = false;

            return result;
        }

        public async Task<string?> ShowInputDialogAsync(
            string title,
            string message,
            string? inputHint,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            if (_isShowing) {
                return null;
            }
            _isShowing = true;

            okButtonLabel ??= AppResources.Ok;
            cancelButtonLabel ??= AppResources.Cancel;

            var result = await _platformDialogService.Value.ShowInputDialogAsync(
                title,
                message,
                inputHint,
                okButtonLabel,
                cancelButtonLabel);

            _isShowing = false;

            return result;
        }

        #endregion
    }
}
