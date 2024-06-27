using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json.Linq;

namespace KaiSeki.XAML;

public partial class SentenceView : ContentView
{
    private Color wordColor = Color.FromArgb("#512BD4");
    private Color sentenceColor = Color.FromArgb("#DFD8F7");
    private Color phraseColor = Color.FromArgb("#ac99ea");
    
    private string[] leaves = new[] { "translation", "grammar", "definition", "furigana", "original_form", "type"};
    private string[] skips = new[] { "phrases", "words" };

    private VerticalStackLayout selected = null;
    private Color oldColor;
    
    public void BuildSentence(JObject jObject)
    {
        SentencePanel.Children.Clear();
        string sentence = jObject.Properties().First().Name;
        
        VerticalStackLayout sentenceLayout = new VerticalStackLayout();
        sentenceLayout.BackgroundColor = sentenceColor;
        sentenceLayout.Padding = new Thickness(10, 30, 5, 5);;
        
        TapGestureRecognizer sentenceTap = new TapGestureRecognizer();
        TappedEventArgs sentenceArgs = new TappedEventArgs(jObject[sentence]);
        sentenceTap.Tapped += (sender, _) => OnTapped(sender, sentenceArgs, "Sentence");
        sentenceLayout.GestureRecognizers.Add(sentenceTap);
        
        JObject phrases = (JObject)jObject[sentence]["phrases"];
        VerticalStackLayout phraseLayout;
        foreach (JProperty i in phrases.Properties())
        {
            string phrase = i.Name;
            JObject phrasevalues = (JObject)phrases[phrase];
            
            phraseLayout = new VerticalStackLayout
            {
                BackgroundColor = phraseColor,
                Padding = new Thickness(10, 30, 5, 5),
                Margin = 4,
            };

            TapGestureRecognizer phraseTap = new TapGestureRecognizer();
            TappedEventArgs phraseArgs = new TappedEventArgs(phrasevalues);
            phraseTap.Tapped += (sender, _) => OnTapped(sender, phraseArgs, "Phrase");
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
                wordTap.Tapped += (sender, _) => OnTapped(sender, wordArgs, word);
                wordLayout.GestureRecognizers.Add(wordTap);
                
                wordLayout.Children.Add(new Label
                {
                    BackgroundColor = wordColor,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Colors.White,
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
        
        OnTapped(sentenceLayout, sentenceArgs,"Sentence");

        if(WordManager.Instance.Sentences.Any(s => s.Text == sentence))
        {
            return;
        }
        WordManager.Instance.Sentences.Insert(0,new Sentence(jObject));
        WordManager.Instance.Save();
    }
    
    public void Clear()
    {
        SentencePanel.Children.Clear();
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

    private void OnTapped(object? sender, TappedEventArgs e, string title)
    {
        if (selected != null)
        {
            selected.BackgroundColor = oldColor;
        }
        selected = sender as VerticalStackLayout;
        oldColor = selected.BackgroundColor;
        selected.BackgroundColor = Colors.Gray;
        
        InfoPanel.Children.Clear();
        InfoPanel.Children.Add(new Label
        {
            Text = title,
            FontSize = 40, 
            HorizontalTextAlignment = TextAlignment.Center,
            BackgroundColor = sentenceColor
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
                        BackgroundColor = phraseColor,
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
    public SentenceView()
    {
        InitializeComponent();
    }
}