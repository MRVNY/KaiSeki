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
        GeminiMessageResponse response = await _geminiClient.TextPrompt(text);

        if (response == null)
        {
            throw new Exception("PromptText failed to receive a response from Gemini service."); // Or handle as needed for UI
        }

        return response.Candidates[0].Content.Parts[0].Text;

    }

}