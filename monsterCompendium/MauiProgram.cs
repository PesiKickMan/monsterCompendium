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
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddSingleton<IDiceService, DiceService>();
            builder.Services.AddSingleton<IDeviceSensorService, DeviceSensorService>();
            builder.Services.AddSingleton<IPhotoService, PhotoService>();

            // ViewModels
            builder.Services.AddTransient<MonstersViewModel>();
            builder.Services.AddTransient<MonsterDetailViewModel>();
            builder.Services.AddTransient<FavoritesViewModel>();
            builder.Services.AddTransient<DiceRollerViewModel>();

            // Views
            builder.Services.AddTransient<MonsterPage>();
            builder.Services.AddTransient<MonsterDetailPage>();
            builder.Services.AddTransient<FavoritePage>();
            builder.Services.AddTransient<DiceRollerPage>();

            // Shell
            builder.Services.AddSingleton<AppShell>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
