using KaiSeki.XAML;
using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
        list.ItemsSource = WordManager.Instance.Sentences;
        NavigatedTo += (sender, args) => list.ItemsSource = WordManager.Instance.Sentences;
    }

    private void List_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
        //alert
        var item = (Sentence) e.Item;
        // DisplayAlert(word.Kanji, $"Hiragana: {word.Hiragana}\nRomanji: {word.Romanji}", "OK");
        
        //go to a new page
        Navigation.PushAsync(new SentencePage(item));
    }

    private void List_OnRefreshing(object? sender, EventArgs e)
    {
        list.ItemsSource =  WordManager.Instance.Refresh();
        list.EndRefresh();
    }
}


