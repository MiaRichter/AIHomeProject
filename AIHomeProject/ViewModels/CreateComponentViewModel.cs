using System.Windows.Input;
using AIHomeProject.Models;
using AIHomeProject.Services;

namespace AIHomeProject.ViewModels
{
    public class CreateComponentViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private Component _currentComponent = new();
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
                    ((Command)CreateCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand CreateCommand { get; }

        public CreateComponentViewModel(ApiService apiService)
        {
            _apiService = apiService;
            CreateCommand = new Command(async () => await CreateComponent(), () => IsOnline);
            CheckConnection();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private async Task CreateComponent()
        {
            if (!ValidateInput()) return;

            IsBusy = true;
            try
            {
                bool success = await _apiService.CreateComponentAsync(CurrentComponent);
                string message = success ? "Компонент успешно создан" : "Не удалось создать компонент";
                await Shell.Current.DisplayAlert(success ? "Успех" : "Ошибка", message, "OK");
                if (success) await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(CurrentComponent.ComponentId))
            {
                Shell.Current.DisplayAlert("Ошибка", "ID компонента обязателен", "OK");
                return false;
            }
            return true;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
            => CheckConnection();

        private void CheckConnection()
            => IsOnline = Connectivity.NetworkAccess == NetworkAccess.Internet;

        ~CreateComponentViewModel()
            => Connectivity.ConnectivityChanged -= OnConnectivityChanged;
    }
}