using ImGuiNET;
using KeyAuth.Renderer;
using Newtonsoft.Json.Linq;

namespace KeyAuth.Utility
{
    public class AuthUtils
    {
        JsonUtils jsonUtils = new JsonUtils();
        public void CheckAndAutoLogin(KeyAuth_Renderer keyAuth)
        {
            string filePath = "credentials.json";

            if (File.Exists(filePath))
            {
                if (!jsonUtils.CheckIfJsonKeyExists(filePath, "username"))
                {
                    keyAuth.key = jsonUtils.ReadFromJson(filePath, "license");
                    Program.KeyAuthApp.license(keyAuth.key);

                    if (!Program.KeyAuthApp.response.success)
                    {
                        File.Delete(filePath);
                        keyAuth.systemMessage = "\nStatus: " + Program.KeyAuthApp.response.message;
                    }
                    else
                    {
                        Program.KeyAuthApp.response.message = "Logged In (Auto-Login)";
                        keyAuth.isLoginSuccessful = true;
                    }

                }
                else
                {
                    keyAuth.username = jsonUtils.ReadFromJson(filePath, "username");
                    keyAuth.password = jsonUtils.ReadFromJson(filePath, "password");

                    Program.KeyAuthApp.login(keyAuth.username, keyAuth.password);

                    if (!Program.KeyAuthApp.response.success)
                    {
                        File.Delete(filePath);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Program.KeyAuthApp.response.message = "Logged In (Auto-Login)";
                        keyAuth.isLoginSuccessful = true;
                    }
                }
            }
        }

        public void PerformLogin(KeyAuth_Renderer keyAuth)
        {
            Program.KeyAuthApp.login(keyAuth.username, keyAuth.password);

            if (!Program.KeyAuthApp.response.success)
            {
                ImGui.Text($"Status: {Program.KeyAuthApp.response.message}");
            }
            else
            {
                JObject userCred = new JObject(
                    new JProperty("username", keyAuth.username),
                    new JProperty("password", keyAuth.password),
                    new JProperty("license", keyAuth.key),
                    new JProperty("email", keyAuth.email)
                );

                string filePath = "credentials.json";
                File.WriteAllText(filePath, userCred.ToString());

                keyAuth.isLoginSuccessful = true;
            }
        }

        public void PerformLicenseLogin(KeyAuth_Renderer keyAuth)
        {
            Program.KeyAuthApp.license(keyAuth.key);

            if (!Program.KeyAuthApp.response.success)
            {
                ImGui.Text($"Status: {Program.KeyAuthApp.response.message}");
            }
            else
            {
                keyAuth.isLoginSuccessful = true;
            }
        }

        public void PerformRegisterUser(KeyAuth_Renderer keyAuth)
        {
            Program.KeyAuthApp.register(keyAuth.username, keyAuth.password, keyAuth.key, keyAuth.email);

            if (!Program.KeyAuthApp.response.success)
            {
                ImGui.Text($"Status: {Program.KeyAuthApp.response.message}");
            }
            else
            {
                keyAuth.tab = 0;
                Program.KeyAuthApp.response.message = "User Registered Successfully!";
            }
        }
    }
}
