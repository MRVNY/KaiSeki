using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json.Linq;

namespace KaiSeki.XAML;

public partial class FixJsonPage : ContentPage
{
    private string jsonString;
    
    public FixJsonPage(string jsonString)
    {
        InitializeComponent();
        this.jsonString = jsonString;
        //replaec { with \n{\n
        this.jsonString = jsonString.Replace("{", "\n{\n").Replace("}", "\n}\n").Replace("\",", "\",\n");
        Editor.Text = this.jsonString;
        
        LinePicker.ItemsSource = Enumerable.Range(1, this.jsonString.Count(c => c == '\n') + 1).Select(i => i.ToString()).ToList();
        LinePicker.SelectedItem = "1";

        // LineNumbers.Text = string.Join("<br>", Enumerable.Range(1, jsonString.Count(c => c == '\n') + 1));
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        jsonString = Editor.Text;
        //parse json
        try
        {
            JObject jObject = JObject.Parse(jsonString);
            Navigation.PopAsync();
            AnalyzerPage.Instance.BuildSentence(jObject);
        }
        catch (Exception exception)
        {
            Toast.Make("Error: " + exception.Message).Show();
            //get string between "line " and " ,position" line 25, position 0.
            string[] tmp = exception.Message.Split(new string[] { "line " }, StringSplitOptions.None);
            string[] tmp2 = tmp[1].Split(new string[] { ", position" }, StringSplitOptions.None);
            int line = int.Parse(tmp2[0]);

            //show line on LinePicker
            LinePicker.SelectedItem = line.ToString();
        }

        //send string back

    }

    private void GoToLine(object? sender, EventArgs e)
    {
        Editor.Focus();
        int line = int.Parse(LinePicker.SelectedItem.ToString());
        //focus on line
        Editor.CursorPosition = string.Join("", Editor.Text.Split('\n').Take(line - 1)).Length + line - 1;
    }
}