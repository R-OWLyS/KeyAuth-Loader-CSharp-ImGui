using KeyAuth.Rendering;
using KeyAuth.Utility;
using Newtonsoft.Json.Linq;

namespace KeyAuth.Credentials;

public class CredentialService
{
    public string username;
    public string password;
    public string key;
    public string email;
    private const string FilePath = "credentials.json";
    
    
    public CredentialService()
    {
        var jsonUtils = new JsonUtils();

        key = jsonUtils.ReadFromJson(FilePath, "license");
        username = jsonUtils.ReadFromJson(FilePath, "username");
        password = jsonUtils.ReadFromJson(FilePath, "password");
        email = jsonUtils.ReadFromJson(FilePath, "email");
    }
    
    public void SaveCredentials()
    {
        var userCred = new JObject(
            new JProperty("username", username),
            new JProperty("password", password),
            new JProperty("license", key),
            new JProperty("email", email)
        );
        File.WriteAllText(FilePath, userCred.ToString());
    }
    public void ClearCredentials()
    {
        if (File.Exists(FilePath)) 
        {
            File.Delete(FilePath);
            Renderer.SystemMessage = $"Completed: file {FilePath} deleted successfully";
        }
        else
        {
            Renderer.SystemMessage = $"Error: unable to find {FilePath}";
        }
    }
    
}