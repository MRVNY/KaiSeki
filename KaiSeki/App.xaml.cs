using Microsoft.Maui.LifecycleEvents;

namespace KaiSeki;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        new SentenceManager();
        new FileManager();
        MainPage = new NavigationPage(new AppShell());
        
        //event when app activated and analyzer activated
        // LifecycleEvents.iOS.OnActivated((app) => OnActivated(app));
    }
}