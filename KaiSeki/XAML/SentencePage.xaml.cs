namespace KaiSeki.XAML;

public partial class SentencePage : ContentPage
{
    public SentencePage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
    
    public SentencePage(Sentence sentence)
    {
        InitializeComponent();
        
        SentenceView.BuildSentence(sentence.JObject);
        NavigationPage.SetHasNavigationBar(this, false);
    }
    
}