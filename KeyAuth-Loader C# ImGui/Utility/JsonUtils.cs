using Newtonsoft.Json.Linq;

namespace KeyAuth.Utility
{
    public class JsonUtils
    {
        public string ReadFromJson(string path, string section)
        {
            if (!File.Exists(path))
                return "";

            var jsonData = File.ReadAllText(path);
            var data = JObject.Parse(jsonData);

            return data[section]?.ToString()??"";
        }
    }
}
