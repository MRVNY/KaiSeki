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
    
    string en_1 = "Translate the given Japanese sentence, and parse and analyze the grammar of every word. Just plain text. No descriptions and explanations. Return only raw JSON object without markdown. No markdown format.\nAll the grammar, except for the quoted, should be in English and should include some insights\nThe parsing for words and phrases should not fragment too much\n [Structure of the JSON object] \n ";
    string cn_1 =
        "我给的日语句子，先切分成短句，再把短句切成单词，并分析每个短句和单词。请给我JSON格式的文字，不要有markdown.\n所有分析请用中文，不要把词和短语切得太细\n [JSON的结构] \n ";

    string en_2 =
        "{{\"<original Japanese sentence>\": {{\"translation\": \"<translation of the sentence>\",\"grammar\": \"<complete grammar analysis of the sentence, taking account the structure and context>\",\"phrases\": {{\"<phrase1>\": {{\"translation\": \"<translation of the phrase>\",\"grammar\": \"<grammar of the phrase>\",\"words\":{{\"<word1>\":{{\"definition\": \"<definition of the word>\", \"furigana\": \"<pronunciation of the word>\", \"original_form\": \"<original form of the word>\", \"type\": \"<type of the word (verb, noun, etc.)>\", \"grammar\": \"<what form it is, function of the word in the sentence, explanation of the informal form, etc.>\"}},\"<word2>\": \"etc.\"}}}},\"<phrase2>\": \"etc.\"}}}}}}";
    string en2_2 =
        "{{\"<original Japanese sentence>\": {{\"translation\": \"<translation of the sentence>\",\"phrases\": {{\"<phrase1>\": {{\"translation\": \"<translation of the phrase>\",\"words\":{{\"<word1>\":{{\"definition\": \"<definition of the word>\", \"furigana\": \"<pronunciation of the word>\", \"original_form\": \"<original form of the word>\", \"type\": \"<type of the word (verb, noun, etc.)>\",}},\"<word2>\": \"etc.\"}}}},\"<phrase2>\": \"etc.\"}}}}}}";
    string cn_2 =
        "{{\"<原句>\": {{\"translation\": \"string\",\"grammar\": \"<整句语法>\",\"phrases\": {{\"<phrase1>\": {{\"translation\": \"string\",\"grammar\": \"string\",\"words\":{{\"<word1>\":{{\"definition\": \"string\", \"furigana\": \"string\", \"original_form\": \"string\", \"type\": \"<词性>\", \"grammar\": \"<是原型的什么变位，是否是口语形式>\"}}}}}}}}}}}}\n";
    
    public GeminiManager()
    {
        var rawPath = FileSystem.OpenAppPackageFileAsync("Keys.json").Result;
        var reader = new StreamReader(rawPath);
        var json = reader.ReadToEnd();
        var jObject = JObject.Parse(json);
        geminiKey = jObject.GetValue("geminiKey").ToString();

        GoogleGeminiConfig config = new GoogleGeminiConfig
        {
            ApiKey = geminiKey,
            // ImageBaseUrl = "CURRENTLY_IMAGE_BASE_URL",
            TextBaseUrl = "https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash-001"
        };
        
        _geminiClient = new GeminiClient(config);

        exampleJson = jObject.GetValue("example").ToObject<JObject>();
    }
    

    public async Task<string> PromptText(string text)
    {
        string en_3 = $"Sentence: \"{text}\"";
        string cn_3 = $"句子: \"{text}\"";
        string prompt = en_1 + en_2 + en_3;
        GeminiMessageResponse response = await _geminiClient.TextPrompt(prompt);
        return response.Candidates[0].Content.Parts[0].Text;
    }
    
    public async Task<string> PromptTextSimple(string text)
    {
        string en_3 = $"Sentence: \"{text}\"";
        string cn_3 = $"句子: \"{text}\"";
        string prompt = en_1 + en2_2 + en_3;
        GeminiMessageResponse response = await _geminiClient.TextPrompt(prompt);
        return response.Candidates[0].Content.Parts[0].Text;;
    }
    
    
    
    public static async Task<string> RESTAsk(string apiKey, string question)
    {
        string apiUrl = "generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key=" + apiKey;

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