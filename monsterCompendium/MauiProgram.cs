using Microsoft.Extensions.Logging;
using monsterCompendium.Services;
using monsterCompendium.ViewModels;
using monsterCompendium.Views;

namespace monsterCompendium
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

            // HttpClient
            builder.Services.AddSingleton<HttpClient>();

            // Services
            builder.Services.AddSingleton<IMonsterApiService, MonsterApiService>();

            // ViewModels
            builder.Services.AddTransient<MonstersViewModel>();

            // Views
            builder.Services.AddTransient<MonsterPage>();

            // Shell
            builder.Services.AddSingleton<AppShell>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
