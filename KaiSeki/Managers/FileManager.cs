using System.Net;
using System.Text;
using CommunityToolkit.Maui.Storage;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace KaiSeki;

public class FileManager
{
    
    private static string appSupDir = FileSystem.Current.AppDataDirectory + "/Application Support/";
    private static string docDir = FileSystem.Current.AppDataDirectory + "/../Documents/";
    public FileManager()
    {
        // _ = AskSaveFile(new CancellationToken());
        // SaveFile();

        if(!File.Exists(appSupDir)) Directory.CreateDirectory(appSupDir);
        
        WriteToFile(Path.Combine(docDir, "test.txt"), "Hello from the Community Toolkit!");
        // ReadFromFile(Path.Combine(docDir, "test.txt"));
        
        // WriteToFile(Path.Combine(appSupDir, "test.txt"), "Hello from the Community Toolkit!");
        // ReadFromFile(Path.Combine(appSupDir, "test.txt"));
    }
    
    async Task AskSaveFile(CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes("Hello from the Community Toolkit!"));
        var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }
    }

    void SaveFile()
    {
        File.OpenWrite(Path.Combine(docDir, "test.txt")).Close();
        File.OpenWrite(Path.Combine(appSupDir, "test.txt")).Close();
        FileStream fileStream = File.Open(Path.Combine(docDir, "test.txt"), FileMode.Open);

    }

    void WriteToFile(string path, string text)
    {
        File.WriteAllText(path, text);
    }
    
    void ReadFromFile(string path)
    {
        string text = File.ReadAllText(path);
        Console.WriteLine(text);
    }
    
}