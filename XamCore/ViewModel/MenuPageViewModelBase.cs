using System.Collections.ObjectModel;
using XamCore.Models;
using XamCore.View;
using Prism.Commands;
using Prism.Navigation;

namespace XamCore.ViewModel
{
    public abstract class MenuPageViewModelBase : ViewModelBase
    {
        private ObservableCollection<MenuItem> _data;

        public ObservableCollection<MenuItem> Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        private MenuItem? _selectedItem;
        public MenuItem? SelectedItem
        {
            get => _selectedItem ??= Data[0];
            set => SetProperty(ref _selectedItem, value);
        }

        public DelegateCommand NavigateCommand { get; }

#pragma warning disable CS8618
        public MenuPageViewModelBase(INavigationService navigationService) : base(navigationService)
        {
            NavigateCommand = new(NavigateCommandExecuted);
            Data = new(BuildMenuItems());
        }
#pragma warning restore CS8618

        public abstract Collection<MenuItem> BuildMenuItems();

        private async void NavigateCommandExecuted()
        {
            await NavigationService.NavigateAsync(
                $"{nameof(TransparentNavigationPage)}/{SelectedItem?.NavigationName}");
        }
    }
}
