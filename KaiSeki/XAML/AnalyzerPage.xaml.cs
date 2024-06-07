using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using DotnetGeminiSDK.Model.Response;
using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class AnalyzerPage : ContentPage
{
    private GeminiManager _geminiManager;

    private string[] leaves = new[]
    {
        "translation",
        "grammar",
        "definition",
        "furigana",
        "original_form",
        "type"
    };

    private string[] skips = new[] { "phrases", "words" };

    public AnalyzerPage()
    {
        InitializeComponent();
        this.Loaded += MainPage_Loaded;

        // ResultStack.Children.Add(Rec_BuildExpander(_geminiManager.GetExample(), 1));
        _geminiManager = new GeminiManager();
        // BuildSentencePanel(_geminiManager.GetExample());
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        BuildSentencePanel(_geminiManager.GetExample());
    }
    
    private async void OnEntryCompleted(object? sender, EventArgs e)
    {
        if(SentenceEntry.Text == "" || SentenceEntry.Text == null)
        {
            SentenceEntry.Text = "面倒事が嫌いだから逆らいはしないものの、トワ自身は生活を改める気など全くなかった。";
        }
        
        // SmallLabel.Text = "Analyzing...";
        
        try{
            string result = await _geminiManager.PromptText(SentenceEntry.Text);
            // SmallLabel.Text = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    

    public Expander Rec_BuildExpander(JObject jObject, int level)
    {
        Expander expander = new Expander();
        string key = jObject.Properties().First().Name;
        //show a label on the header
        // string tab = level == 1 ? "" : new string(' ', level - 1);
        
        if(leaves.Contains(key))
        {
            Label nonExpandable = new Label { Text = key+" : " + jObject[key], FontSize = 15};
            nonExpandable.Padding = new Thickness(level*20, 0, 0, 0);
            expander.Header = nonExpandable;
        }
        else
        {
            Color shadeLevel = new Color(1f - 0.1f*level, 1f - 0.1f*level, 1f - 0.1f*level);
            Label expandable = new Label { Text = key + " >", FontSize = 30 - 2*level, BackgroundColor = shadeLevel};
            
            expander.ExpandedChanged += OnExpandedChanged;
            expandable.Padding = new Thickness(level*20, 0, 0, 0);
            expander.Header = expandable;
            StackLayout tmp = new StackLayout();

            var list = ((JObject)jObject[key]).Properties();
            foreach (JProperty item in list.Reverse())
            {
                if (leaves.Contains(item.Name)) tmp.Children.Add(Rec_BuildExpander(new JObject(item), level + 1));
                else tmp.Children.Insert(0, Rec_BuildExpander(new JObject(item), level + 1));
            }
            expander.Content = tmp;
        }

        return expander;
    }

    private void BuildSentencePanel(JObject jObject)
    {
        SentencePanel.Children.Clear();
        string sentence = jObject.Properties().First().Name;
        
        VerticalStackLayout sentenceLayout = new VerticalStackLayout();
        sentenceLayout.BackgroundColor = Color.FromArgb("#DFD8F7");
        sentenceLayout.Padding = new Thickness(10, 30, 5, 5);;
        
        TapGestureRecognizer sentenceTap = new TapGestureRecognizer();
        TappedEventArgs sentenceArgs = new TappedEventArgs(jObject[sentence]);
        sentenceTap.Tapped += (sender, _) => OnTapped("Sentence", sentenceArgs);
        sentenceLayout.GestureRecognizers.Add(sentenceTap);
        
        JObject phrases = (JObject)jObject[sentence]["phrases"];
        VerticalStackLayout phraseLayout;
        foreach (JProperty i in phrases.Properties())
        {
            string phrase = i.Name;
            JObject phrasevalues = (JObject)phrases[phrase];
            
            phraseLayout = new VerticalStackLayout
            {
                BackgroundColor = Color.FromArgb("#ac99ea"),
                Padding = new Thickness(10, 30, 5, 5),
                Margin = 4
            };

            TapGestureRecognizer phraseTap = new TapGestureRecognizer();
            TappedEventArgs phraseArgs = new TappedEventArgs(phrasevalues);
            phraseTap.Tapped += (sender, _) => OnTapped("Phrase", phraseArgs);
            phraseLayout.GestureRecognizers.Add(phraseTap);
            
            JObject words = (JObject)phrasevalues["words"];
            VerticalStackLayout wordLayout = null;
            foreach (JProperty j in words.Properties())
            {
                string word = j.Name;
                JObject wordValues = (JObject)words[word];
                
                wordLayout = new VerticalStackLayout();
                
                TapGestureRecognizer wordTap = new TapGestureRecognizer();
                TappedEventArgs wordArgs = new TappedEventArgs(wordValues);
                wordTap.Tapped += (sender, _) => OnTapped(word, wordArgs);
                wordLayout.GestureRecognizers.Add(wordTap);
                
                wordLayout.Children.Add(new Label
                {
                    BackgroundColor = Color.FromArgb("#512BD4"),
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.FromArgb("#FFFFFF"),
                    Padding = 2, Margin = 4,
                    FontSize = 20,
                    TextType = TextType.Html,
                    Text = string.Join("<br>", word.ToCharArray())
                });
                
                phraseLayout.Children.Add(wordLayout);
            }
            
            sentenceLayout.Children.Add(phraseLayout);
        }
        SentencePanel.Children.Add(sentenceLayout);
        
        OnTapped("Sentence", sentenceArgs);
    }

    private void OnExpandedChanged(object? sender, ExpandedChangedEventArgs e)
    {
        Expander expander = (Expander)sender;
        Label header = (Label)expander.Header;
        if (e.IsExpanded)
        {
            header.Text = header.Text.Replace("◃", "▿");
        }
        else
        {
            header.Text = header.Text.Replace("▿", "◃");
        }
    }

    // private void OnTapped(object? sender, TappedEventArgs e)
    // {
    //     InfoPanel.Children.Clear();
    //     JObject values = (JObject)e.Parameter;
    //     string s_grammer = values["grammar"].ToString();
    //     string s_translation = values["translation"].ToString();
    //     
    //     InfoPanel.Children.Add(new Expander
    //     {
    //         IsExpanded = true,
    //         Header = new Label { Text = "Grammar" },
    //         Content = new Label { Text = s_grammer }
    //     });
    //     
    //     InfoPanel.Children.Add(new Expander
    //     {
    //         IsExpanded = true,
    //         Header = new Label { Text = "Translation" },
    //         Content = new Label { Text = s_translation }
    //     });
    // }
    

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        InfoPanel.Children.Clear();
        InfoPanel.Children.Add(new Label
        {
            Text = (string)sender,
            FontSize = 40, 
            HorizontalTextAlignment = TextAlignment.Center,
            BackgroundColor = Color.FromArgb("#DFD8F7")
        });
        JObject values = (JObject)e.Parameter;
        foreach (JProperty item in values.Properties())
        {
            if (leaves.Contains(item.Name))
            {
                Expander expander = new Expander
                {
                    Margin = new Thickness(0, 0, 0, 0),
                    IsExpanded = true,
                    Header = new Label { 
                        Text = item.Name + "▿", FontSize = 25, 
                        BackgroundColor = Color.FromArgb("#ac99ea"),
                        Padding = 5
                    },
                    Content = new Label
                    {
                        Text = item.Value.ToString(), FontSize = 20,
                        Padding = 5
                    },
                };
                expander.ExpandedChanged += OnExpandedChanged;
                
                InfoPanel.Children.Add(expander);
            }
        }
    }
}

