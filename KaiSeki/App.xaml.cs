using Microsoft.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;

namespace KaiSeki;

public partial class App
{
    public App()
    {
        InitializeComponent();
        new SentenceController();
        MainPage = new NavigationPage(new AppShell());
        
        GlobalExceptionHandler.UnhandledException += GlobalExceptionHandler_UnhandledException;
        
        //event when app activated and analyzer activated
        // LifecycleEvents.iOS.OnActivated((app) => OnActivated(app));
    }
    
    private void GlobalExceptionHandler_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as System.Exception;
        // Log this exception or whatever
        // Display an error message
        Shell.Current.DisplayAlert("Error", exception.Message, "OK");
    }
}