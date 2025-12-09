using Microsoft.Extensions.Logging;
using RawGames.Services;
using RawGames.ViewModel;
using RawGames.Views;

namespace RawGames
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

            // Registrar servicios
            builder.Services.AddSingleton<MoviesApiService>();

            // Registrar ViewModels
            builder.Services.AddSingleton<GameListViewModel>();

            // Registrar Views
            builder.Services.AddSingleton<GameListPage>();

            return builder.Build();
        }
    }
}
