using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using MauiIcons.Cupertino;
using Microsoft.Maui.LifecycleEvents;

namespace KaiSeki;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        })
            .UseMauiCommunityToolkit()
            .UseCupertinoMauiIcons()
            .ConfigureLifecycleEvents(events =>
            {
#if IOS
                events.AddiOS(ios => ios
                    .OnActivated(app =>
                        {
                            LogEvent(nameof(iOSLifecycle.OnActivated));
                            //if page header is Analayzer
                            if(AppShell.Instance.CurrentPage == AnalyzerPage.Instance)
                            {
                                AnalyzerPage.Instance.GetClipboard(null, null);
                            }
                        }));
#endif
                static bool LogEvent(string eventName, string type = null)
                {
                    System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                    return true;
                }
                
                
            });
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}