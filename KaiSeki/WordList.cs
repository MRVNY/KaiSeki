namespace KaiSeki;

public class WordList
{
    public static WordList Instance;
    
    public List<Word> Words { get; set; }
    
    public WordList()
    {
        Words = new List<Word>()
        {
            //some japanese words
            new Word { Hiragana = "あい", Kanji = "愛", Romanji = "ai" },
            new Word { Hiragana = "あいさつ", Kanji = "挨拶", Romanji = "aisatsu" },
            new Word { Hiragana = "あいじょう", Kanji = "愛情", Romanji = "aijou" },
            new Word { Hiragana = "あいする", Kanji = "愛する", Romanji = "aisuru" },
            new Word { Hiragana = "あいず", Kanji = "愛好", Romanji = "aizu" },
            new Word { Hiragana = "あいだ", Kanji = "間", Romanji = "aida" },
            new Word { Hiragana = "あいて", Kanji = "相手", Romanji = "aite" },
            new Word { Hiragana = "あいにく", Kanji = "愛憎", Romanji = "ainiku" },
            new Word { Hiragana = "あいびき", Kanji = "愛嬌", Romanji = "aibiki" },
            new Word { Hiragana = "あいぼう", Kanji = "愛妨", Romanji = "aibou" },
            new Word { Hiragana = "あいまい", Kanji = "曖昧", Romanji = "aimai" },
            new Word { Hiragana = "あいりょう", Kanji = "愛憐", Romanji = "airyou" },
            new Word { Hiragana = "あいわ", Kanji = "愛話", Romanji = "aiwa" },
            new Word { Hiragana = "あお", Kanji = "青", Romanji = "ao" },
            new Word { Hiragana = "あおい", Kanji = "青い", Romanji = "aoi" },
            new Word { Hiragana = "すい", Kanji = "水", Romanji = "sui" },
            new Word { Hiragana = "すいか", Kanji = "西瓜", Romanji = "suika" },
            new Word { Hiragana = "すいかい", Kanji = "水海", Romanji = "suikai" },
            new Word { Hiragana = "すいかん", Kanji = "水管", Romanji = "suikan" },
            new Word { Hiragana = "すいき", Kanji = "水気", Romanji = "suiki" },
        };
        
        Instance = this;
    }
}

public class Word
{
    public string Hiragana { get; set; }
    public string Kanji { get; set; }
    public string Romanji { get; set; }
    
    public string Gemini { get; set; }
}