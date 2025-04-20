using System.Windows.Input;
using AIHomeProject.Models;
using AIHomeProject.Services;

namespace AIHomeProject.ViewModels
{
    public class EditComponentViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private Component _currentComponent;
        private bool _isOnline;

        public Component CurrentComponent
        {
            get => _currentComponent;
            set => SetProperty(ref _currentComponent, value);
        }

        public bool IsOnline
        {
            get => _isOnline;
            private set
            {
                if (SetProperty(ref _isOnline, value))
                {
                    ((Command)UpdateCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand UpdateCommand { get; }

        public EditComponentViewModel(ApiService apiService)
        {
            _apiService = apiService;
            UpdateCommand = new Command(async () => await UpdateComponent(), () => IsOnline);
            CheckConnection();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        public void Initialize(Component component)
        {
            CurrentComponent = component ?? new Component();
        }

        private async Task UpdateComponent()
        {
            IsBusy = true;
            try
            {
                bool success = await _apiService.UpdateComponentAsync(CurrentComponent);
                string message = success ? "Компонент успешно обновлен" : "Не удалось обновить компонент";

                if (success) await Shell.Current.GoToAsync("..");
                await Shell.Current.DisplayAlert(success ? "Успех" : "Ошибка", message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
            => CheckConnection();

        private void CheckConnection()
            => IsOnline = Connectivity.NetworkAccess == NetworkAccess.Internet;

        ~EditComponentViewModel()
            => Connectivity.ConnectivityChanged -= OnConnectivityChanged;
    }
}