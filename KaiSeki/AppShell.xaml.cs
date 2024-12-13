using Microsoft.Maui.Controls;

namespace KaiSeki;

public partial class AppShell
{
    public static AppShell Instance;
    public AppShell()
    {
        Instance = this;
        InitializeComponent();
        
        NavigationPage.SetHasNavigationBar(this, false);
    }
    
    public void SetTab(int index)
    {
        TabBar.CurrentItem = TabBar.Items[index];
    }
}