namespace KaiSeki;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        new WordList();
        MainPage = new NavigationPage(new AppShell());
    }
}