using DotnetGeminiSDK.Model.Response;
using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
        list.ItemsSource = WordManager.Instance.Words;
    }

    private void List_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
        //alert
        var word = (Word) e.Item;
        // DisplayAlert(word.Kanji, $"Hiragana: {word.Hiragana}\nRomanji: {word.Romanji}", "OK");
        
        //go to a new page
        Navigation.PushAsync(new WordPage(word));
    }

    private void List_OnRefreshing(object? sender, EventArgs e)
    {
        list.ItemsSource = WordManager.Instance.Words;
    }
}


