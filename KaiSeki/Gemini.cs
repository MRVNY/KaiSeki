using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model.Response;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;


namespace KaiSeki;

using DotnetGeminiSDK;
using Microsoft.Extensions.DependencyInjection;

public class Gemini
{
    private string geminiKey;
    private readonly IGeminiClient _geminiClient;
    
    public Gemini()
    {
        var json = File.ReadAllText("./Keys.json");
        var jObject = JObject.Parse(json);
        var tmp = jObject.ToString();
        // openAIKey = jObject.GetValue("openAIKey").ToString();
        geminiKey = jObject.GetValue("geminiKey").ToString();

        GoogleGeminiConfig config = new GoogleGeminiConfig
        {
            ApiKey = geminiKey,
            // ImageBaseUrl = "CURRENTLY_IMAGE_BASE_URL",
            TextBaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash"
        };
        
        _geminiClient = new GeminiClient(config);
    }
    

    public async Task<string> PromptText(string text)
    {
        // string prompt = $"对于‘{text}’这个日语句子，请用平假名注音汉字，并用中文分析除了助词和名字以外的每个单词的语法，最后整句翻译。";
        string prompt = $"Could you translate this Japanese sentence and then apply furigana to the kanji, and then parse and analyse the grammar of every word? Just plain text. No markdown format. No markdown format: \"{text}\" No markdown format";
        
        GeminiMessageResponse response = await _geminiClient.TextPrompt(prompt);

        if (response == null)
        {
            throw new Exception("PromptText failed to receive a response from Gemini service."); // Or handle as needed for UI
        }
        
        string result = response.Candidates[0].Content.Parts[0].Text;
        
        WordList.Instance.Words.Insert(0,new Word{Hiragana = "ひらがな", Kanji = text, Romanji = "Romanji", Gemini = result});

        return result;

    }

}