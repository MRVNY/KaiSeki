using System.Windows.Input;
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
    
    //return
    // private ICommand ReturnCommand => new Command(OnButtonClicked);

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        //hide button
        // Button.IsVisible = false;
        
        if(SentenceEntry.Text == "")
        {
            SentenceEntry.Text = "面倒事が嫌いだから逆らいはしないものの、トワ自身は生活を改める気など全くなかった。";
        }
        
        SmallLabel.Text = SentenceEntry.Text;
        
        try{
            string result = await _Gemini.PromptText(SentenceEntry.Text);
            SmallLabel.Text = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async void OnEntryCompleted(object? sender, EventArgs e)
    {
        if(SentenceEntry.Text == "" || SentenceEntry.Text == null)
        {
            SentenceEntry.Text = "面倒事が嫌いだから逆らいはしないものの、トワ自身は生活を改める気など全くなかった。";
        }
        
        SmallLabel.Text = "Analyzing...";
        
        try{
            string result = await _Gemini.PromptText(SentenceEntry.Text);
            SmallLabel.Text = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

