using Newtonsoft.Json.Linq;

namespace Untitled_Japanese_App;

using Azure.AI.OpenAI;
using Azure;


public partial class MainPage : ContentPage
{
    private OpenAIClient _chatGptClient;
    private string openAIKey;
    private string geminiKey;
    private string openAIEndpoint = null;
    private int count = 0;

    public MainPage()
    {
        InitializeComponent();
        //read geminiKey of the json file with JObject
        var json = File.ReadAllText("../Keys.json");
        var jObject = JObject.Parse(json);
        // openAIKey = jObject.GetValue("openAIKey").ToString();
        geminiKey = jObject.GetValue("geminiKey").ToString();
        
        
        this.Loaded += MainPage_Loaded;
        count = 0;
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        Console.WriteLine("MainPage_Loaded");
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Button clicked");
        await AskGemini(SentenceEntry.Text);
    }


    private async Task AskGemini(string sentence)
    {
        // if (string.IsNullOrWhiteSpace(LocationEntry.Text))
        // {
        //     await DisplayAlert("Empty location", "Please enter a location (city or postal code)", "OK");
        //     return;
        // }

        string prompt = $"对于‘{sentence}’这个日语句子，请用平假名注音汉字，并用中文分析除了助词和名字以外的每个单词的语法，最后整句翻译。";
        // string prompt = "Hello this is a test, can you hear me?";

       
        try{
            string result = await GeminiREST.Ask(geminiKey, prompt);
            SmallLabel.Text = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        
        
        
        
    }
}

