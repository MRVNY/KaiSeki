using KaiSeki.XAML;
using MauiIcons.Core;
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
        _ = new MauiIcon();
        
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
        // Navigation.PushAsync(new FixJsonPage("\"用心棒たちが見張ってると思ったら誰もいないな\": {\n  \"translation\": \"I thought the bodyguards were watching, but there's nobody there.\",\n  \"grammar\": \"This sentence is a combination of two clauses. The first clause \\\"用心棒たちが見張ってる\\\" is a declarative sentence with a subject \\\"用心棒たち\\\" and a verb \\\"見張ってる\\\". The second clause \\\"と思ったら誰もいないな\\\" is a conjunction phrase with a verb \\\"と思ったら\\\" and a subject \\\"誰も\\\", followed by a predicate \\\"いないな\\\". The conjunction phrase \\\"と思ったら\\\" is a sentence modifier that connects the two clauses, indicating a change of thought or realization.\",\n  \"phrases\": {\n    \"用心棒たちが見張ってる\": {\n      \"translation\": \"The bodyguards are watching.\",\n      \"grammar\": \"Declarative sentence, Subject + Verb. The subject \\\"用心棒たち\\\" is a noun phrase consisting of the noun \\\"用心棒\\\" and the plural marker \\\"たち\\\". The verb \\\"見張ってる\\\" is in the te-form, indicating continuous action.\",\n      \"words\": {\n        \"用心棒\": {\n          \"definition\": \"bodyguard\",\n          \"furigana\": \"ようじんぼう\",\n          \"original_form\": \"用心棒\",\n          \"type\": \"noun\",\n          \"grammar\": \"Noun, subject of the sentence\"\n        },\n        \"たち\": {\n          \"definition\": \"plural marker\",\n          \"furigana\": \"たち\",\n          \"original_form\": \"たち\",\n          \"type\": \"particle\",\n          \"grammar\": \"Plural marker, attached to the noun \\\"用心棒\\\"\"\n        },\n        \"見張る\": {\n          \"definition\": \"to watch over, to keep an eye on\",\n          \"furigana\": \"みはる\",\n          \"original_form\": \"見張る\",\n          \"type\": \"verb\",\n          \"grammar\": \"Verb, in the te-form, indicating continuous action\"\n        }\n      }\n    },\n    \"と思ったら\": {\n      \"translation\": \"but then I realized\",\n      \"grammar\": \"Conjunction phrase, indicating a change of thought or realization. It connects the two clauses in the sentence, expressing a shift in the speaker's understanding.\",\n      \"words\": {\n        \"と思ったら\": {\n          \"definition\": \"but then I realized\",\n          \"furigana\": \"と  おもったら\",\n          \"original_form\": \"思う\",\n          \"type\": \"verb\",\n          \"grammar\": \"Verb \\\"思う\\\" in the te-form + particle \\\"たら\\\" indicating a change of thought\"\n        }\n      }\n    },\n    \"誰もいないな\": {\n      \"translation\": \"There's nobody there.\",\n      \"grammar\": \"Declarative sentence, Subject + Predicate. The subject \\\"誰も\\\" is a pronoun meaning \\\"nobody\\\". The predicate \\\"いないな\\\" is a negative form of the verb \\\"いる\\\", indicating absence. The particle \\\"な\\\" is a sentence-ending particle used for casual speech.\",\n      \"words\": {\n        \"誰も\": {\n          \"definition\": \"nobody\",\n          \"furigana\": \"だれも\",\n          \"original_form\": \"誰も\",\n          \"type\": \"pronoun\",\n          \"grammar\": \"Pronoun, subject of the sentence\"\n        },\n        \"いない\": {\n          \"definition\": \"to be not\",\n          \"furigana\": \"いない\",\n          \"original_form\": \"いる\",\n          \"type\": \"verb\",\n          \"grammar\": \"Verb \\\"いる\\\" in the negative form, indicating absence\"\n        },\n        \"な\": {\n          \"definition\": \"sentence-ending particle\",\n          \"furigana\": \"な\",\n          \"original_form\": \"な\",\n          \"type\": \"particle\",\n          \"grammar\": \"Sentence-ending particle, used in casual speech\"\n        }\n      }\n    }\n  }\n"));
        // return;
        
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
        
        string result = await _geminiManager.RESTAsk(SentenceEntry.Text);
        Indicator.IsVisible = false;
        jObject = null;
        
        try{
            jObject = JSONParse(result);
            // SentenceView.IsVisible = true;
            SentenceView.BuildSentence(jObject);
        }
        catch (Exception ex)
        {
            SmallLabel.Text = "Failed, trying one more time \n" + ex.Message;
            LabelScroll.IsVisible = true;
            Indicator.IsVisible = true;

            result = await _geminiManager.RESTAsk(SentenceEntry.Text);
            
            Indicator.IsVisible = false;
            LabelScroll.IsVisible = false;
                
            jObject = null;
            try
            {
                jObject = JSONParse(result);
                // SentenceView.IsVisible = true;
                SentenceView.BuildSentence(jObject);
            }
            catch (Exception exception)
            {
                LabelScroll.IsVisible = true;
                SmallLabel.Text = "Failed \n" + exception.Message;
                
                bool ask = await DisplayAlert("Fixer", "Would you like to manually fix the error?", "Yes", "No");
                if(ask && result!=null) Navigation.PushAsync(new FixJsonPage(result));
            }
        }
    }
    
    public void BuildSentence(JObject jObject)
    {
        Indicator.IsVisible = false;
        LabelScroll.IsVisible = false;
        SentenceView.BuildSentence(jObject);
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

    // private void OnMenuClicked(object? sender, EventArgs e)
    // {
    //     //open menu
    //    
    //     //open menu
    //     AppShell.Instance.MenuBarItems.Add(new MenuBarItem()
    //     {
    //         Text = "Clear",
    //
    //     });
    // }
}

