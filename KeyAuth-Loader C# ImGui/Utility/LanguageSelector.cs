using ImGuiNET;
using Newtonsoft.Json;

namespace KeyAuth.Utility
{
    public class LanguageSelector
    {
        private int _currentLanguageIndex;
        private readonly string[] _languageOptions = new[] { "English", "Italian", "Spanish", "German", "Chinese", "Polish", "Russian", "Swedish", "Japanese" };
        private readonly string[] _languageCodes = new[] { "EN", "IT", "ES", "DE", "ZH", "PL", "RU", "SV", "JA" };
        private static string _currentLanguage = string.Empty;

        private static readonly Dictionary<string, Dictionary<string, string>?> Translations = new();

        private string _defaultLanguage = "EN";

        private void LoadDefaultFromJson()
        {
            try
            {
                var jsonPath = Path.Combine("Config", "language.json");
                var jsonString = File.ReadAllText(jsonPath);
                dynamic configObject = JsonConvert.DeserializeObject(jsonString)!;
                
                _defaultLanguage = configObject.Language;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to read default language from language.json: {ex.Message}");
            }
        }

        public LanguageSelector()
        {
            LoadTranslations();
            LoadDefaultFromJson();
            SetLanguage(_defaultLanguage);
        }

        private void LoadTranslations()
        {
            foreach (var languageCode in _languageCodes)
            {
                LoadLanguageStrings(languageCode);
            }
        }

        private void LoadLanguageStrings(string languageCode)
        {
            try
            {
                var jsonPath = Path.Combine("Translations", $"{languageCode}.json");
                var jsonString = File.ReadAllText(jsonPath);
                Translations[languageCode] = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading strings for translations {languageCode}: {ex.Message}");
            }
        }

        private void SetLanguage(string languageCode)
        {
            _currentLanguage = Translations.ContainsKey(languageCode) ? languageCode : _defaultLanguage;
        }

        public void RenderLanguageSelector()
        {
            _currentLanguageIndex = Array.IndexOf(_languageCodes, _currentLanguage);
            ImGui.Text(GetString("Language:"));
            ImGui.SameLine();
            if (ImGui.Combo("##LanguageCombo", ref _currentLanguageIndex, _languageOptions, _languageOptions.Length))
            {
                SetLanguage(_languageCodes[_currentLanguageIndex]);
            }
        }

        public string GetString(string key)
        {
            return Translations.TryGetValue(_currentLanguage, out var languageStrings) && languageStrings!.TryGetValue(key, out var value)
                ? value
                : $"MISSING TRANSLATION: {key}";
        }

        public string[] GetArray(IEnumerable<string> keys)
        {
            return keys.Select(GetString).ToArray();
        }
    }
}
