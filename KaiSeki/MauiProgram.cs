using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace KaiSeki;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // .ConfigureFonts(fonts =>
            // {
            //     fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            //     fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            // })
            
#if IOS
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
                handlers.AddHandler<CarouselView, Microsoft.Maui.Controls.Handlers.Items2.CarouselViewHandler2>();
            })
#endif
            
            .UseMauiCommunityToolkit()
            
            // .UseCupertinoMauiIcons()
            
            
#if IOS
            .ConfigureLifecycleEvents(events =>
            {
                try{
                    events.AddiOS(ios => ios
                        .OnActivated(app =>
                            {
                                LogEvent(nameof(iOSLifecycle.OnActivated));
                                //if page header is Analayzer
                                if(AppShell.Instance != null && AppShell.Instance.CurrentPage == AnalyzerPage.Instance)
                                {
                                    AnalyzerPage.Instance.GetClipboard(null, null);
                                }
                            }));

                    static bool LogEvent(string eventName, string type = null)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                        return true;
                    }
                }catch(Exception e)
                {
                    //display alert
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            })
#endif
            ;
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}