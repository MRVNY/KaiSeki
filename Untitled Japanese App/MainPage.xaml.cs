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
        openAIKey = jObject.GetValue("openAIKey").ToString();
        geminiKey = jObject.GetValue("geminiKey").ToString();
        Console.WriteLine("openAIKey: " + openAIKey);
        
        this.Loaded += MainPage_Loaded;
        count = 0;
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        _chatGptClient = !string.IsNullOrWhiteSpace(openAIEndpoint)
            ? new OpenAIClient(
                new Uri(openAIEndpoint),
                new AzureKeyCredential(openAIKey))
            : new OpenAIClient(openAIKey);
        
        Console.WriteLine("MainPage_Loaded");
        // await AskChatGPT("Hello, how are you?");
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        count++;
        Console.WriteLine("Button clicked");
        SmallLabel.Text = "Hello, Maui " + count.ToString();
        // await AskChatGPT(SentenceEntry.Text);
    }

    private async Task AskChatGPT(string sentence)
    {
        // if (string.IsNullOrWhiteSpace(LocationEntry.Text))
        // {
        //     await DisplayAlert("Empty location", "Please enter a location (city or postal code)", "OK");
        //     return;
        // }

        // string prompt = $"对于‘{sentence}’这个日语句子，请用平假名注音汉字，并用中文分析除了助词和名字以外的每个单词的语法，最后整句翻译。";
        string prompt = "Hello this is a test, can you hear me?";

        // DeploymentName must match your custom deployment name (Azure OpenAI)
        // Or a default deployment name (such as OpenAI's GPT-3.5-turbo-0125) can be used
        ChatCompletionsOptions options = new()
        {
            DeploymentName = "gpt-3.5-turbo-0125",
            Messages =
            {
                new ChatRequestUserMessage(prompt)
            },
            ChoiceCount = 1,
            MaxTokens = 100,
        };
        

        var message = new ChatRequestUserMessage(prompt);
        Console.WriteLine("Sending message...");
        // Response<ChatCompletions> response = await _chatGptClient.GetChatCompletionsAsync(options);
        // Console.WriteLine("Waiting for response...");
        // Console.WriteLine(response.Value.Choices[0].Message.Content);
        // SmallLabel.Text = response.Value.Choices[0].Message.Content;
        
        try
        {
            Console.WriteLine("Sending message...");
            options.Messages.Add(message);
            Response<ChatCompletions> response = await _chatGptClient.GetChatCompletionsAsync(options);
            Console.WriteLine("Received response...");
            Console.WriteLine(response.Value.Choices[0].Message.Content);
            SmallLabel.Text = response.Value.Choices[0].Message.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

