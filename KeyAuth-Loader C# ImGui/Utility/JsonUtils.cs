using Newtonsoft.Json.Linq;

namespace KeyAuth.Utility
{
    public class JsonUtils
    {
        public string ReadFromJson(string path, string section)
        {
            if (!File.Exists(path))
                return "File Not Found";

            string jsonData = File.ReadAllText(path);
            JObject data = JObject.Parse(jsonData);

            return (string)data[section];
        }

        public bool CheckIfJsonKeyExists(string path, string section)
        {
            if (!File.Exists(path))
                return false;

            string jsonData = File.ReadAllText(path);
            JObject data = JObject.Parse(jsonData);

            return data.ContainsKey(section);
        }
    }
}
