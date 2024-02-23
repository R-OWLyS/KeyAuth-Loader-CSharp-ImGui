using ClickableTransparentOverlay;
using ImGuiNET;
using KeyAuth.Rendering;
using Newtonsoft.Json;

public class LanguageSelector
{
    private int _currentLanguageIndex;
    private readonly string[] _languageOptions = { "English", "Italian" ,"Spanish", "German", "Chinese", "Polish", "Russian" };
    private readonly string[] _languageCodes = { "EN", "IT", "ES", "DE", "ZH", "PL", "RU"};
    private string _currentLanguage = string.Empty;

    private readonly Dictionary<string, Dictionary<string, string>?> _translations = new();

    private const string DefaultLanguage = "EN";
    
    public LanguageSelector()
    {
        LoadTranslations();
        SetLanguage(DefaultLanguage);
    }

    private void LoadTranslations()
    {
        LoadLanguageStrings("EN");
        LoadLanguageStrings("IT");
        LoadLanguageStrings("ES");
        LoadLanguageStrings("DE");
        LoadLanguageStrings("ZH");
        LoadLanguageStrings("PL");
        LoadLanguageStrings("RU");
    }

    private void LoadLanguageStrings(string languageCode)
    {
        try
        {
            var jsonPath = Path.Combine("Translations", $"{languageCode}.json");
            Console.WriteLine($"-> {jsonPath}");
            var jsonString = File.ReadAllText(jsonPath);
            _translations[languageCode] = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading strings for translations {languageCode}: {ex.Message}");
        }
    }

    private void SetLanguage(string languageCode)
    {
        _currentLanguage = _translations.ContainsKey(languageCode) ? languageCode : DefaultLanguage;
    }

    public void RenderLanguageSelector()
    {
        ImGui.Text(GetString("Language:"));
        ImGui.SameLine();
        if (ImGui.Combo("##LanguageCombo", ref _currentLanguageIndex, _languageOptions, _languageOptions.Length))
        {
            SetLanguage(_languageCodes[_currentLanguageIndex]);
        }
    }

    public string GetString(string key)
    {
        return _translations.TryGetValue(_currentLanguage, out var languageStrings) && languageStrings!.TryGetValue(key, out var value)
            ? value
            : $"MISSING TRANSLATION: {key}";
    }
    
    public string[] GetArray(IEnumerable<string> keys)
    {
        return keys.Select(GetString).ToArray();
    }
}
