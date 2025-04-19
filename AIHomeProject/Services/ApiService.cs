using System.Text;
using System.Text.Json;
using AIHomeProject.Models;

namespace AIHomeProject.Services
{
    // Services/ApiService.cs
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://mia.local:5000/Api/ComponentManipulation/";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Component>> GetComponentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}ShowComponents");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка сервера: {response.StatusCode}");
                    return new List<Component>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<List<Component>>(content, options);
                return result ?? new List<Component>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка сети: {ex.Message}");
                await Shell.Current.DisplayAlert("Ошибка", "Не удалось подключиться к серверу", "OK");
                return new List<Component>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex}");
                return new List<Component>();
            }
        }

        public async Task<bool> CreateComponentAsync(Component component)
        {
            try
            {
                var json = JsonSerializer.Serialize(component);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}Create", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании компонента: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateComponentAsync(Component component)
        {
            try
            {
                var json = JsonSerializer.Serialize(component);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{BaseUrl}Update", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении компонента: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteComponentAsync(string componentId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}Delete/{componentId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении компонента: {ex.Message}");
                return false;
            }
        }
    }
}
