using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;


namespace KaiSeki;

using DotnetGeminiSDK;
using Microsoft.Extensions.DependencyInjection;

public class GeminiManager
{
    // private JObject exampleJson;
    private string apiUrl;
    
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
        apiUrl = jObject.GetValue("apiUrl").ToString();


        // exampleJson = jObject.GetValue("example").ToObject<JObject>();
    }
    
    
    
    
    public async Task<string> RESTAsk(string question)
    {
        string jsonRequestBody = $"{{ \"name\": \"{question}\" }}";

        using (HttpClient client = new HttpClient())
        {
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        return null;
    }
}