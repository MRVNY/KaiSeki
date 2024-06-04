using JishoNET.Models;

namespace Untitled_Japanese_App;

public class Jisho
{
    private JishoClient client;
    
    public Jisho()
    {
        client = new JishoClient();
    }
    
    public async Task<JishoDefinition> Search(string query)
    {
        JishoResult<JishoDefinition[]> result = await client.GetDefinitionAsync(query);
        
        return result.Data[0];
    }
}