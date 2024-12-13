using System;
using KaiSeki.XAML;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace KaiSeki;


public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        list.ItemsSource = SentenceController.Instance.Sentences;
        List_OnRefreshing(null,null);
        NavigatedTo += (sender, args) => list.ItemsSource = SentenceController.Instance.Sentences;
    }

    private void List_OnItemTapped(object? sender, ItemTappedEventArgs itemTappedEventArgs)
    {
        //alert
        var item = itemTappedEventArgs.Item as Sentence;
        
        //go to a new page
        Navigation.PushAsync(new SentencePage(item));
    }

    private void List_OnRefreshing(object? sender, EventArgs e)
    {
        list.ItemsSource = null;
        list.ItemsSource =  SentenceController.Instance.Refresh();
        InvalidateMeasure();
        list.EndRefresh();
    }

    private void OnDelete(object? sender, EventArgs e)
    {
        SwipeItem swipeItem = (SwipeItem) sender;
        Sentence sentence = swipeItem.BindingContext as Sentence;
        SentenceController.Instance.Sentences.Remove(sentence);
        List_OnRefreshing(null,null);
    }
}


