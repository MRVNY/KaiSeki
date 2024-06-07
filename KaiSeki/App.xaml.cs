namespace KaiSeki;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        new WordManager();
        new FileManager();
        MainPage = new NavigationPage(new AppShell());
    }
}