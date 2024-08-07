﻿using KaiSeki.XAML;
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
        list.ItemsSource =  WordManager.Instance.Refresh();
        // list.EndRefresh();
    }

    private void OnDelete(object? sender, EventArgs e)
    {
        SwipeItem swipeItem = (SwipeItem) sender;
        Sentence sentence = swipeItem.BindingContext as Sentence;
        WordManager.Instance.Sentences.Remove(sentence);
        List_OnRefreshing(null,null);
    }
}


