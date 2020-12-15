using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor;
using MudBlazor.Services;
using SpotPG.Services;
using SpotPG.Services.Abstractions;
using SpotPG.Services.SpotifyCredentialsManager;

namespace SpotPG
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor()
                .AddHubOptions(o => { o.MaximumReceiveMessageSize = 102400000; });

            services.AddScoped<IClipboardService, ClipboardService>();

            services.AddSingleton<IConfigurationProviderService>(_ => ConfigurationProviderService.Instance);
            services.AddSingleton<IServiceConfiguration>(_ => ConfigurationProviderService.Instance.Config);

            services.AddSingleton<ISpotifyCredentialsManager, SpotifyCredentialsManager>();
            services.AddSingleton<ISpotifyClientProviderService, SpotifyClientProviderService>();

            services.AddSingleton<ISpotifyMetaInfoFetcherService, SpotifyMetaInfoFetcherService>();
            services.AddSingleton<ISpotifyPlaylistsManagerService, SpotifyPlaylistsManagerService>();

            services.AddSingleton<ISceneReleaseNameParserService, SceneReleaseNameParserService>();
            services.AddSingleton<ISpotifySearchQueryGeneratorService, SpotifySearchQueryGeneratorService>();

            services.AddMudBlazorDialog();
            services.AddMudBlazorSnackbar();
            services.AddMudBlazorResizeListener();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
}