namespace KaiSeki;

public partial class AppShell : Shell
{
    public static AppShell Instance;
    public AppShell()
    {
        Instance = this;
        InitializeComponent();
        
        NavigationPage.SetHasNavigationBar(this, false);
    }
}