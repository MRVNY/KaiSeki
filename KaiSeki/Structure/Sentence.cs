
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

public class Sentence
{
    public string Text { get; set; }
    public DateTime Date { get; set; }
    // public string Translation { get; set; }
    // public string Grammar { get; set; }
    //
    // public Phrase[] Phrases { get; set; }

    public JObject JObject { get; set; }
    
    public Sentence(JObject jObject)
    {
        Text = jObject.Properties().First().Name;
        JObject = jObject;
        Date = DateTime.Now;
    }
}