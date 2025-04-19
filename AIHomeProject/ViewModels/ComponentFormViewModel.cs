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

        public Component CurrentComponent
        {
            get => _currentComponent;
            set => SetProperty(ref _currentComponent, value);
        }

        public string Title => _isNew ? "Добавить компонент" : "Редактировать компонент";

        public ICommand SaveCommand { get; }

        public ComponentFormViewModel(ApiService apiService)
        {
            _apiService = apiService;
            SaveCommand = new Command(OnSave);
        }

        public void Initialize(Component component, bool isNew)
        {
            CurrentComponent = component ?? new Component();
            _isNew = isNew;
            OnPropertyChanged(nameof(Title));
        }

        private async void OnSave()
        {
            if (string.IsNullOrWhiteSpace(CurrentComponent.ComponentId) ||
                string.IsNullOrWhiteSpace(CurrentComponent.Name))
            {
                await Shell.Current.DisplayAlert("Ошибка", "ID и название компонента обязательны", "OK");
                return;
            }

            IsBusy = true;
            bool success;

            if (_isNew)
            {
                success = await _apiService.CreateComponentAsync(CurrentComponent);
            }
            else
            {
                success = await _apiService.UpdateComponentAsync(CurrentComponent);
            }

            IsBusy = false;

            if (success)
            {
                await Shell.Current.DisplayAlert("Успех",
                    _isNew ? "Компонент успешно создан" : "Компонент успешно обновлен",
                    "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    _isNew ? "Не удалось создать компонент" : "Не удалось обновить компонент",
                    "OK");
            }
        }
    }
}