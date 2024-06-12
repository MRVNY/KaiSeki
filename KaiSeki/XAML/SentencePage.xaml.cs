namespace KaiSeki.XAML;

public partial class SentencePage : ContentPage
{
    public SentencePage()
    {
        InitializeComponent();
    }
    
    public SentencePage(Sentence sentence)
    {
        InitializeComponent();
        
        SentenceView.BuildSentence(sentence.JObject);
    }
    
}