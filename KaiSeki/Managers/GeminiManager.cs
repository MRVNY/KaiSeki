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

public class GeminiManager
{
    private string geminiKey;
    private readonly IGeminiClient _geminiClient;
    private JObject exampleJson;
    
    public GeminiManager()
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

        exampleJson = jObject.GetValue("example").ToObject<JObject>();
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
        
        WordManager.Instance.Words.Insert(0,new Word{Hiragana = "ひらがな", Kanji = text});

        return result;

    }
    
    public static async Task<string> RESTAsk(string apiKey, string question)
    {
        string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + apiKey;

        string jsonRequestBody = @"{
            ""system_instruction"":
                    {""parts"": {
                       ""text"": ""Translate the given Japanese sentence and then apply furigana to all the kanji, and then parse and analyze the grammar of every word? Just plain text. No markdown format""}},
            ""contents"": [{
                ""parts"":[{
                    ""text"": """ + question + @"""
                }]
            }]
        }";

        using (HttpClient client = new HttpClient())
        {
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(result);
                    Console.WriteLine(jObject);
                    
                    return (string)(jObject["candidates"][0]["content"]["parts"][0]["text"]);
                    // return result.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        return null;
    }
    
    public JObject GetExample()
    {
        return exampleJson;
    }
    

}