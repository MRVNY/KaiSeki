using System.Collections;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using Newtonsoft.Json;

namespace KaiSeki;

public class SentenceManager
{
    public static SentenceManager Instance;

    private IEnumerable<char> romaji;
    private IEnumerable<char> hiragana;
    private IEnumerable<char> katakana;
    private IEnumerable<char> kanji;
    
    private static string appSupDir = FileSystem.Current.AppDataDirectory + "/Application Support/";
    
    public List<Sentence> Sentences { get; set; }
    
    public SentenceManager()
    {
        // romaji = GetCharsInRange(searchKeyword, 0x0020, 0x007E);
        // hiragana = GetCharsInRange(searchKeyword, 0x3040, 0x309F);
        // katakana = GetCharsInRange(searchKeyword, 0x30A0, 0x30FF);
        // kanji = GetCharsInRange(searchKeyword, 0x4E00, 0x9FBF);

        if(File.Exists(Path.Combine(appSupDir, "sentences.json")))
        {
            Sentences = Load();
        }
        else
        {
            Sentences = new List<Sentence>();
        }
        
        Instance = this;
    }
    
    private static IEnumerable<char> GetCharsInRange(string text, int min, int max)
    {
        return text.Where(e => e >= min && e <= max);
    }
    
    public static bool HasKanji(string text) { return text.Any(c => c >= 0x4E00 && c <= 0x9FBF); }
    public static bool HasHiragana(string text) { return text.Any(c => c >= 0x3040 && c <= 0x309F); }
    public static bool HasKatakana(string text) { return text.Any(c => c >= 0x30A0 && c <= 0x30FF); }
    public static bool HasRomaji(string text) { return text.Any(c => c >= 0x0020 && c <= 0x007E); }

    public void Save()
    {
        //serialize and save to file
        string json = JsonConvert.SerializeObject(Sentences);
        File.WriteAllText(Path.Combine(appSupDir, "sentences.json"), json);
    }
    
    private List<Sentence> Load()
    {
        //load from file and deserialize
        string json = File.ReadAllText(Path.Combine(appSupDir, "sentences.json"));
        return JsonConvert.DeserializeObject<List<Sentence>>(json);
    }

    public List<Sentence> Refresh()
    {
        Save();
        return Sentences;
    }
    
    public async Task Backup(CancellationToken cancellationToken)
    {
        // var folderPickerResult = await FolderPicker.PickAsync(appSupDir);
        using var stream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(Sentences)));
        string date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = "KaiSeki_Backup_"+date+".txt";
        var fileSaverResult = await FileSaver.SaveAsync(appSupDir, fileName, stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }
    }
    
    public async Task Restore(CancellationToken cancellationToken)
    {
        //pick file from storage
        var filePickerResult = await FilePicker.PickAsync();
        try
        {
            //serialize and save to file
            string json = File.ReadAllText(filePickerResult.FullPath);
            await Toast.Make("Success"+json).Show(cancellationToken);

            //ask for confirmation
            bool confirm = await Application.Current.MainPage.DisplayAlert("Restore", "Are you sure you want to restore?", "Yes", "No");
            
            if(confirm)
            {
                Sentences = JsonConvert.DeserializeObject<List<Sentence>>(json);
                Save();
                await Toast.Make("Restored").Show(cancellationToken);
            }
        }
        catch (Exception e)
        {
            //DisplayAlert
            //current page
            await Toast.Make("Error"+e.Message).Show(cancellationToken);
        }
        
    }
}