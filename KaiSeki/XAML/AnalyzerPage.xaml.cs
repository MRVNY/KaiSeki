using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class AnalyzerPage : ContentPage
{
    public static AnalyzerPage Instance;
    private GeminiManager _geminiManager;

    public AnalyzerPage()
    {
        InitializeComponent();
        this.Loaded += MainPage_Loaded;

        _geminiManager = new GeminiManager();
        // BuildSentencePanel(_geminiManager.GetExample());
        // Appearing += GetClipboard;
        // NavigatedTo += GetClipboard;
        
        Instance = this;
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        // BuildSentencePanel(_geminiManager.GetExample());
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
        if(SentenceEntry.Text == "" || SentenceEntry.Text == null)
        {
            // SentenceEntry.Text = "カイセキは、日本語の解析します。";
            SentenceEntry.Text = "日本語の解析します。";
        }
        
        LabelScroll.IsVisible = true;
        SmallLabel.Text = "Analyzing...";
        
        try{
            string result = await _geminiManager.RESTAsk(SentenceEntry.Text);
            LabelScroll.IsVisible = false;
            SmallLabel.Text = result;
            // Console.WriteLine(result);
            JObject jObject = JObject.Parse(result);
            
            SentenceView.BuildSentence(jObject);
        }
        catch (Exception ex)
        {
            try
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                LabelScroll.IsVisible = true;
                SmallLabel.Text = "Failed, trying the simplified version \n" + ex.Message + "\n" + SmallLabel.Text;
                string result = await _geminiManager.RESTAsk(SentenceEntry.Text);
                Console.WriteLine(result);
                JObject jObject = JObject.Parse(result);
                
                SmallLabel.Text = "Analyzing...";
                LabelScroll.IsVisible = false;
                
                SentenceView.BuildSentence(jObject);
            }
            catch (Exception exception)
            {
                SmallLabel.Text = ex.Message + "\n" + SmallLabel.Text;
                Console.WriteLine(exception);
                // throw;
            }
        }
    }
    
    private void SwipeGestureRecognizer_OnSwiped(object? sender, SwipedEventArgs e)
    {
        SentenceEntry.Unfocus();
    }

}

