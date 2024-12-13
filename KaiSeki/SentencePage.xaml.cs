// using MauiIcons.Core;
using Microsoft.Maui.Controls;

namespace KaiSeki.XAML;

public partial class SentencePage : ContentPage
{
    public SentencePage()
    {
        InitializeComponent();
        // _ = new MauiIcon();
        NavigationPage.SetHasNavigationBar(this, false);
    }
    
    public SentencePage(Sentence sentence)
    {
        InitializeComponent();
        
        SentenceView.BuildSentence(sentence.JObject);
        NavigationPage.SetHasNavigationBar(this, false);
    }
    
}