using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using AIHomeProject.Models;
using AIHomeProject.Services;
using AIHomeProject.Pages;

namespace AIHomeProject.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Component> Components { get; } = new();

        public ICommand AddComponentCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand EditComponentCommand { get; }
        public ICommand DeleteComponentCommand { get; }
        public ICommand LoadComponentsCommand { get; }

        public MainViewModel(ApiService apiService)
        {
            _apiService = apiService;

            AddComponentCommand = new Command(OnAddComponent);
            ShowDetailsCommand = new Command<Component>(OnShowDetails);
            EditComponentCommand = new Command<Component>(OnEditComponent);
            DeleteComponentCommand = new Command<string>(OnDeleteComponent);
            LoadComponentsCommand = new Command(async () => await LoadComponentsAsync());

            //LoadComponents();
        }

        public async Task LoadComponentsAsync()
        {
            IsBusy = true;
            var components = await _apiService.GetComponentsAsync();
            Components.Clear();
            foreach (var component in components)
            {
                Components.Add(component);
            }
            IsBusy = false;
        }

        public async void LoadComponents() => await LoadComponentsAsync();

        private async void OnAddComponent()
        {
            await Shell.Current.GoToAsync(nameof(CreateComponentPage));
        }

        private async void OnEditComponent(Component component)
        {
            if (component == null || component.ComponentId == null) return;

            var parameters = new Dictionary<string, object>
            {
                { "Component", component }
            };

            await Shell.Current.GoToAsync(nameof(EditComponentPage), parameters);
        }

        private async void OnShowDetails(Component component)
        {
            if (component == null) return;

            var details = new StringBuilder();
            details.AppendLine($"ID: {component.Id}");
            details.AppendLine($"Component ID: {component.ComponentId}");
            details.AppendLine($"Тип: {component.ComponentType}");
            details.AppendLine($"Местоположение: {component.Location}");
            details.AppendLine($"Описание: {component.Description}");
            details.AppendLine($"Создан: {component.CreatedAt}");
            details.AppendLine($"Обновлён: {component.UpdatedAt}");
            details.AppendLine($"Статус: {(component.IsActive ? "Активен" : "Неактивен")}");

            await Shell.Current.DisplayAlert(
                $"Детали: {component.Name}",
                details.ToString(),
                "Закрыть");
        }

        private async void OnDeleteComponent(string componentId)
        {
            if (string.IsNullOrEmpty(componentId)) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Подтверждение",
                "Вы уверены, что хотите удалить этот компонент?",
                "Да", "Нет");

            if (confirm)
            {
                IsBusy = true;
                bool success = await _apiService.DeleteComponentAsync(componentId);
                IsBusy = false;

                if (success)
                {
                    await Shell.Current.DisplayAlert("Успех", "Компонент успешно удален", "OK");
                    LoadComponents();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Не удалось удалить компонент", "OK");
                }
            }
        }
    }
}