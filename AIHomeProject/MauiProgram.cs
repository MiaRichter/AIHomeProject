using Microsoft.Extensions.Logging;
using AIHomeProject.Pages;
using AIHomeProject.Services;
using AIHomeProject.ViewModels;

namespace AIHomeProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Регистрация сервисов
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<ComponentFormViewModel>();
            builder.Services.AddTransient<ComponentFormPage>();
            builder.Services.AddTransient<MainPage>();
            return builder.Build();
        }
    }
}
