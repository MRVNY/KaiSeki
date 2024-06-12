using Newtonsoft.Json.Linq;
using System.Text;


namespace KaiSeki;

public class GeminiManager
{
    private string apiUrl;
    private JObject exampleJson;

    public GeminiManager()
    {
        var rawPath = FileSystem.OpenAppPackageFileAsync("Keys.json").Result;
        var reader = new StreamReader(rawPath);
        var json = reader.ReadToEnd();
        var jObject = JObject.Parse(json);
        apiUrl = jObject.GetValue("apiUrl").ToString();
        
        exampleJson = jObject.GetValue("example").ToObject<JObject>();
    }
    
    public JObject GetExample()
    {
        return exampleJson;
    }
    
    public async Task<string> RESTAsk(string question)
    {
        string jsonRequestBody = $"{{ \"name\": \"{question}\" }}";

        using (HttpClient client = new HttpClient())
        {
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