using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class AnalyzerPage : ContentPage
{
    public static AnalyzerPage Instance;
    private GeminiManager _geminiManager;
    private Task parsing;
    private JObject jObject;

    public AnalyzerPage()
    {
        InitializeComponent();
        this.Loaded += MainPage_Loaded;
        
        NavigationPage.SetHasNavigationBar(this, false);

        // SentenceView.IsVisible = false;
        _geminiManager = new GeminiManager();
        // BuildSentencePanel(_geminiManager.GetExample());
        // Appearing += GetClipboard;
        // NavigatedTo += GetClipboard;
        
        Instance = this;
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        // SentenceView.BuildSentence(_geminiManager.GetExample());
    }
    
    public async void GetClipboard(object? sender, EventArgs e)
    {
        // OnEntryCompleted(null, null);
        // SentenceView.BuildSentence(_geminiManager.GetExample());
        string? clipboardText = await Clipboard.GetTextAsync();
        if (clipboardText != SentenceEntry.Text)
        {
            SentenceEntry.Text = clipboardText;
            //activate keyboard on entry
            SentenceEntry.Focus();
        }
    }
    
    private async void OnEntryCompleted(object? sender, EventArgs e)
    {
        SentenceEntry.Unfocus();
        if(SentenceEntry.Text == "" || SentenceEntry.Text == null)
        {
            // SentenceEntry.Text = "カイセキは、日本語の解析します。";
            SentenceEntry.Text = "日本語の解析します。";
        }
        
        Indicator.IsVisible = true;
        Indicator.IsRunning = true;
        LabelScroll.IsVisible = false;
        // SentenceView.IsVisible = false;
        SentenceView.Clear();
        
        
        try{
            string result = await _geminiManager.RESTAsk(SentenceEntry.Text);
            Indicator.IsVisible = false;
            
            jObject = null;
            jObject = JSONParse(result);
            // SentenceView.IsVisible = true;
            SentenceView.BuildSentence(jObject);
        }
        catch (Exception ex)
        {
            try
            {
                SmallLabel.Text = "Failed, trying one more time \n" + ex.Message;
                LabelScroll.IsVisible = true;
                Indicator.IsVisible = true;

                string result = await _geminiManager.RESTAsk(SentenceEntry.Text);
                
                Indicator.IsVisible = false;
                LabelScroll.IsVisible = false;
                
                jObject = null;
                jObject = JSONParse(result);
                // SentenceView.IsVisible = true;
                SentenceView.BuildSentence(jObject);
            }
            catch (Exception exception)
            {
                LabelScroll.IsVisible = true;
                SmallLabel.Text = "Failed \n" + exception.Message;
            }
        }
    }

    JObject JSONParse(string result)
    {
        JObject jObject = null;
        try
        {
            jObject = JObject.Parse(result);
        }
        catch (Exception exception)
        {
            result += "}";
            try
            {
                jObject = JObject.Parse(result);
            }
            catch (Exception e)
            {
                //minus last two characters
                result = result.Substring(0, result.Length - 2);
                jObject = JObject.Parse(result);
            }
        }

        return jObject;
    }

    private void SwipeGestureRecognizer_OnSwiped(object? sender, SwipedEventArgs e)
    {
        SentenceEntry.Unfocus();
    }

}

