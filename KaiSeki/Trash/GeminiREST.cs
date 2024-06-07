using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace KaiSeki;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class GeminiREST
{
    private JsonSerializerOptions _serializerOptions;
    public static async Task<string> Ask(string apiKey, string question)
    {
        string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.0-pro:generateContent?key=" + apiKey;

        string jsonRequestBody = @"{
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
}