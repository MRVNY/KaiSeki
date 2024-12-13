using System;
using System.Threading;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class SettingsPage : ContentPage
{

    public SettingsPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        EntryKey.Text = GeminiController.GetGeminiKey();
        Switch.IsToggled = GeminiController.UseGeminiAPI;
    }

    private void OnRestore(object? sender, EventArgs e)
    {
        //cancelation token
        CancellationToken cancellationToken = new CancellationToken();
        SentenceController.Instance.Restore(cancellationToken);
    }

    private void OnBackup(object? sender, EventArgs e)
    {
        //cancelation token
        CancellationToken cancellationToken = new CancellationToken();
        SentenceController.Instance.Backup(cancellationToken);
    }

    private void OnSave(object? sender, EventArgs e)
    {
        GeminiController.SetGeminiKey(EntryKey.Text);
        Toast.Make("Gemini key saved").Show();
        EntryKey.Unfocus();
    }

    private void OnDelete(object? sender, EventArgs e)
    {
        EntryKey.Text = "";
        GeminiController.DeleteGeminiKey();
        Toast.Make("Gemini key deleted").Show();
        EntryKey.Unfocus();
    }

    private void Switch_OnToggled(object? sender, ToggledEventArgs e)
    {
        GeminiController.SetUseGeminiAPI(((Switch) sender).IsToggled);
        if(GeminiController.UseGeminiAPI && string.IsNullOrEmpty(GeminiController.GetGeminiKey()))
        {
            DisplayAlert("Gemini key is empty", "Please enter your Gemini key", "OK");
            (sender as Switch).IsToggled = false;
        }
    }
}

