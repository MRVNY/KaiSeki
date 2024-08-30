using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class SettingsPage : ContentPage
{

    public SettingsPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void OnRestore(object? sender, EventArgs e)
    {
        //cancelation token
        CancellationToken cancellationToken = new CancellationToken();
        SentenceManager.Instance.Restore(cancellationToken);
    }

    private void OnBackup(object? sender, EventArgs e)
    {
        //cancelation token
        CancellationToken cancellationToken = new CancellationToken();
        SentenceManager.Instance.Backup(cancellationToken);
    }
}

