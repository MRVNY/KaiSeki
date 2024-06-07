using JishoNET.Models;

namespace KaiSeki;

public class JishoManager
{
    private JishoClient client;
    
    public JishoManager()
    {
        client = new JishoClient();
    }
    
    public async Task<JishoDefinition> Search(string query)
    {
        JishoResult<JishoDefinition[]> result = await client.GetDefinitionAsync(query);
        
        return result.Data[0];
    }
}