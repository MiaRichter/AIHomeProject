using System.Net;
using System.Text;
using System.Text.Json;
using AIHomeProject.Models;


namespace AIHomeProject.Services
{
    // Services/ApiService.cs
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://mia.local:5000/Api/ComponentManipulation/"; // Измените на http
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        public ApiService()
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

        }

        public async Task<List<Component>> GetComponentsAsync()
        {
            try
            {
                string url = $"{BaseUrl}ShowComponents";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    //await ShowAlert($"Ошибка сервера: {response.StatusCode}");
                    return new List<Component>();
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new List<Component>();
                }

                return JsonSerializer.Deserialize<List<Component>>(content, _jsonOptions) ?? new List<Component>();
            }
            catch (Exception ex)
            {
                //await ShowAlert($"Ошибка: {ex.Message}");
                return new List<Component>();
            }
        }

        public async Task<bool> CreateComponentAsync(Component component)
        {
            try
            {
                // Проверка интернет-соединения
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await ShowAlert("Нет интернет-соединения");
                    return false;
                }

                if (component == null)
                {
                    await ShowAlert("Компонент не может быть null");
                    return false;
                }

                var json = JsonSerializer.Serialize(component, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}Create", content);

                return response.IsSuccessStatusCode;
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
            {
                await ShowAlert("Запрос был отменен");
                return false;
            }
            catch (TaskCanceledException)
            {
                await ShowAlert("Таймаут запроса (15 секунд)");
                return false;
            }
            catch (HttpRequestException ex)
            {
                //await ShowAlert($"Ошибка сети: {GetNetworkError(ex)}");
                return false;
            }
            catch (Exception ex)
            {
                await ShowAlert($"Неожиданная ошибка: {ex.Message}");
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

                return true;
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
            {
                //await ShowAlert("Запрос был отменен");
                return false;
            }
            catch (TaskCanceledException)
            {
                //await ShowAlert("Таймаут запроса (15 секунд)");
                return false;
            }
            catch (HttpRequestException ex)
            {
                //await ShowAlert($"Ошибка сети: {GetNetworkError(ex)}");
                return false;
            }
            catch (Exception ex)
            {
                //await ShowAlert($"Неожиданная ошибка: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteComponentAsync(string componentId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}Delete/{componentId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении компонента: {ex.Message}");
                return false;
            }
        }


        private string GetNetworkError(HttpRequestException ex)
        {
            return ex.InnerException?.Message ?? ex.Message;
        }

        private async Task ShowAlert(string message)
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Shell.Current.DisplayAlert("Ошибка", message, "OK"));
            }
            catch
            {
                // Фолбэк на случай проблем с UI потоком
                Console.WriteLine($"Не удалось показать alert: {message}");
            }
        }
    }
}
