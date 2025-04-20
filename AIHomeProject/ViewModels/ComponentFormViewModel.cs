using System.Windows.Input;
using AIHomeProject.Models;
using AIHomeProject.Services;

namespace AIHomeProject.ViewModels
{
    public class ComponentFormViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private Component _currentComponent;
        private bool _isNew;
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
                    // Обновляем доступность команды при изменении состояния
                    ((Command)SaveCommand).ChangeCanExecute();
                }
            }
        }

        public string Title => _isNew ? "Добавить компонент" : "Редактировать компонент";

        public ICommand SaveCommand { get; }

        public ComponentFormViewModel(ApiService apiService)
        {
            _apiService = apiService;
            SaveCommand = new Command(async () => await OnSave(), () => IsOnline);

            // Первоначальная проверка соединения
            CheckConnection();

            // Подписываемся на изменения состояния сети
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }


        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            // Обновляем статус при изменении подключения
            CheckConnection();
        }

        private void CheckConnection()
        {
            IsOnline = Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public void Initialize(Component component, bool isNew)
        {
            CurrentComponent = component ?? new Component();
            _isNew = isNew;
            OnPropertyChanged(nameof(Title));
        }

        private async Task OnSave()
        {
            if (string.IsNullOrWhiteSpace(CurrentComponent.ComponentId) ||
                string.IsNullOrWhiteSpace(CurrentComponent.Name))
            {
                await Shell.Current.DisplayAlert("Ошибка", "ID и название компонента обязательны", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                bool success;
                string message;

                if (_isNew)
                {
                    success = await _apiService.CreateComponentAsync(CurrentComponent);
                    message = success ? "Компонент успешно создан" : "Не удалось создать компонент";
                }
                else
                {
                    success = await _apiService.UpdateComponentAsync(CurrentComponent);
                    message = success ? "Компонент успешно обновлен" : "Не удалось обновить компонент";
                }

                if (success)
                {
                    await Shell.Current.GoToAsync("..");
                }

                await Shell.Current.DisplayAlert(success ? "Успех" : "Ошибка", message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
            
        }
        ComponentFormViewModel()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }
    }
}