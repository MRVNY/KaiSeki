using DotnetGeminiSDK.Model.Response;
using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class MainPage : ContentPage
{
    private int count = 0;
    private Gemini _Gemini;

    public MainPage()
    {
        InitializeComponent();
        _Gemini = new Gemini();

        this.Loaded += MainPage_Loaded;
        count = 0;

        // _Gemini.Test();
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        Console.WriteLine("MainPage_Loaded");
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        Button.IsEnabled = false;
        if(SentenceEntry.Text == null || SentenceEntry.Text == "")
        {
            SentenceEntry.Text = "面倒事が嫌いだから逆らいはしないものの、トワ自身は生活を改める気など全くなかった。";
        }
        string prompt = $"对于‘{SentenceEntry.Text}’这个日语句子，请用平假名注音汉字，并用中文分析除了助词和名字以外的每个单词的语法，最后整句翻译。";
       
        try{
            string result = await _Gemini.PromptText(prompt);
            SmallLabel.Text = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

