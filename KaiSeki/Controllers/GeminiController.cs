using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;


namespace KaiSeki;

public static class GeminiController
{
    private static string appSupDir = FileSystem.Current.AppDataDirectory + "/Application Support/";

    private static string urlGCP, urlSupabase, apiGemini, apiSupabase;
    private static JObject requestJson;
    public static JObject exampleJson { get; }

    public static bool UseGeminiAPI { get; set; }
    
    static GeminiController()
    {
        var rawPath = FileSystem.OpenAppPackageFileAsync("Keys.json").Result;
        var reader = new StreamReader(rawPath);
        var json = reader.ReadToEnd();
        var jObject = JObject.Parse(json);
        
        urlGCP = jObject.GetValue("urlGCP").ToString();
        urlSupabase = jObject.GetValue("urlSupabase").ToString();
        apiSupabase = jObject.GetValue("apiSupabase").ToString();
        
        exampleJson = jObject.GetValue("example").ToObject<JObject>();
        requestJson = jObject.GetValue("apiRequest").ToObject<JObject>();
        
        if(!File.Exists(appSupDir)) Directory.CreateDirectory(appSupDir);

        if(File.Exists(Path.Combine(appSupDir, "gemini.json")))
        {
            // SaveGemini();
            LoadGemini();
        }
        else
        {
            apiGemini = "";
            UseGeminiAPI = true;
        }
    }
    
    public static JObject GetExample() { return exampleJson; }
    public static string GetGeminiKey() { return apiGemini; }

    public static void SetGeminiKey(string key)
    {
        apiGemini = key; 
        SaveGemini();
    }
    
    public static void SaveGemini()
    {
        //serialize and save to file
        string json = JsonConvert.SerializeObject(new { apiGemini, UseGeminiAPI });
        File.WriteAllText(Path.Combine(appSupDir, "gemini.json"), json);
    }
    
    private static void LoadGemini()
    {
        string json = File.ReadAllText(Path.Combine(appSupDir, "gemini.json"));
        apiGemini = JObject.Parse(json).GetValue("apiGemini").ToString();
        UseGeminiAPI = JObject.Parse(json).GetValue("UseGeminiAPI").ToObject<bool>();
    }
    
    public static void DeleteGeminiKey()
    {
        SaveGemini();
    }
    
    public static void SetUseGeminiAPI(bool value)
    {
        UseGeminiAPI = value;
        SaveGemini();
    }
    
    public static async Task<string> PrivateAPI(string question, string apiChoice = "Supabase")
    {
        string jsonRequestBody = $"{{ \"name\": \"{question}\" }}";
        
        using (HttpClient client = new HttpClient())
        {
            try
            {
                if(apiChoice=="Supabase") client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiSupabase);
                
                string apiURL = apiChoice == "GCP" ? urlGCP : urlSupabase;
                HttpResponseMessage response = await client.PostAsync(apiURL, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));
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

    public static async Task<string> PublicAPI(string question)
    {
        string apiUrl =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + apiGemini;
        
        requestJson["contents"][0]["parts"][0]["text"] = question;
        
        using (HttpClient client = new HttpClient())
        {
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(result);
                    string output = (string)(jObject["candidates"][0]["content"]["parts"][0]["text"]);
                    output = output.Replace("```json", "").Replace("```", "");
                    return output;
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