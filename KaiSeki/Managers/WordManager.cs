namespace KaiSeki;

public class WordManager
{
    public static WordManager Instance;

    private IEnumerable<char> romaji;
    private IEnumerable<char> hiragana;
    private IEnumerable<char> katakana;
    private IEnumerable<char> kanji;
    
    public List<Word> Words { get; set; }
    
    public WordManager()
    {
        // romaji = GetCharsInRange(searchKeyword, 0x0020, 0x007E);
        // hiragana = GetCharsInRange(searchKeyword, 0x3040, 0x309F);
        // katakana = GetCharsInRange(searchKeyword, 0x30A0, 0x30FF);
        // kanji = GetCharsInRange(searchKeyword, 0x4E00, 0x9FBF);
    
        Words = new List<Word>()
        {
            //some japanese words
            new Word { Hiragana = "あい", Kanji = "愛" },
            new Word { Hiragana = "あいさつ", Kanji = "挨拶" },
            new Word { Hiragana = "あいじょう", Kanji = "愛情" },
            new Word { Hiragana = "あいする", Kanji = "愛する" },
            new Word { Hiragana = "あいず", Kanji = "愛好" },
            new Word { Hiragana = "あいだ", Kanji = "間" },
            new Word { Hiragana = "あいて", Kanji = "相手" },
            new Word { Hiragana = "あいにく", Kanji = "愛憎" },
            new Word { Hiragana = "あいびき", Kanji = "愛嬌" },
            new Word { Hiragana = "あいぼう", Kanji = "愛妨" },
            new Word { Hiragana = "あいまい", Kanji = "曖昧" },
            new Word { Hiragana = "あいりょう", Kanji = "愛憐" },
            new Word { Hiragana = "あいわ", Kanji = "愛話" },
            new Word { Hiragana = "あお", Kanji = "青" },
            new Word { Hiragana = "あおい", Kanji = "青い" },
            new Word { Hiragana = "すい", Kanji = "水" },
            new Word { Hiragana = "すいか", Kanji = "西瓜" },
            new Word { Hiragana = "すいかい", Kanji = "水海" },
            new Word { Hiragana = "すいかん", Kanji = "水管" },
            new Word { Hiragana = "すいき", Kanji = "水気" },
        };
        
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


}