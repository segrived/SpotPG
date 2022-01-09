using Blazored.LocalStorage;
using MudBlazor.Services;
using SpotPG.Frontend.Services;
using SpotPG.Frontend.Services.Abstractions;
using SpotPG.Logger.Abstractions;
using SpotPG.Spotify.Abstractions;
using SpotPG.Spotify.Services;

namespace SpotPG.Frontend;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor()
            .AddHubOptions(o => { o.MaximumReceiveMessageSize = 102400000; });

        services.AddScoped<IClipboardService, ClipboardService>();

        services.AddSingleton<IConfigurationProviderService>(_ => ConfigurationProviderService.Instance);

        static ISpotifyClientProviderService SpotifyClientProviderServiceFactory(IServiceProvider provider) => new SpotifyClientProviderService(
            provider.GetService<ILoggerService>(),
            provider.GetService<IConfigurationProviderService>()?.Config.SpotifyAccess);

        services.AddSingleton(SpotifyClientProviderServiceFactory);

        services.AddSingleton<ISpotifyMetaInfoFetcherService, SpotifyMetaInfoFetcherService>();
        services.AddSingleton<ISpotifyPlaylistsManagerService, SpotifyPlaylistsManagerService>();
        services.AddSingleton<ISpotifyConnectionStateProviderService, SpotifyConnectionStateProviderService>();

        services.AddSingleton<ISceneReleaseNameParserService, SceneReleaseNameParserService>();
        services.AddSingleton<ISpotifySearchQueryGeneratorService, SpotifySearchQueryGeneratorService>();

        services.AddSingleton<ILoggerService, LoggerService>();

        services.AddMudServices();

        services.AddBlazoredLocalStorage();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // we need to create instance for logs collect from application start
        app.ApplicationServices.GetService<ILoggerService>();

        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints => {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/General/_Host");
        });
    }
}