using ImGuiNET;
using KeyAuth.Credentials;

namespace KeyAuth.Utility
{
    public class AuthUtils(CredentialService credentialService, api keyAuth)
    {
        public bool isLoginSuccessful;
        public string systemMessage = "";
        public void CheckAndAutoLogin()
        {
            if (!string.IsNullOrEmpty(credentialService.key))
            {
                keyAuth.license(credentialService.key);
            }
            else if (!string.IsNullOrEmpty(credentialService.username) && !string.IsNullOrEmpty(credentialService.password))
            {
                keyAuth.login(credentialService.username, credentialService.password);
            }
            else
            {
                return;
            }


            if (!keyAuth.response.success)
            {
                credentialService.ClearCredentials();
                systemMessage = "\nStatus: " + keyAuth.response.message;
            }
            else
            {
                keyAuth.response.message = "Logged In (Auto-Login)";
                isLoginSuccessful = true;
            }
        }

        public void PerformLogin()
        {
            keyAuth.login(credentialService.username, credentialService.password);

            if (!keyAuth.response.success)
            {
                ImGui.Text($"Status: {keyAuth.response.message}");
            }
            else
            {
                credentialService.SaveCredentials();
                isLoginSuccessful = true;
            }
        }

        public void PerformLicenseLogin()
        {
            keyAuth.license(credentialService.key);

            if (!keyAuth.response.success)
            {
                ImGui.Text($"Status: {keyAuth.response.message}");
            }
            else
            {
                isLoginSuccessful = true;
                credentialService.SaveCredentials();
            }
        }

        public bool PerformRegisterUser()
        {
            keyAuth.register(credentialService.username, credentialService.password, credentialService.key, credentialService.email);

            if (!keyAuth.response.success)
            {
                ImGui.Text($"Status: {keyAuth.response.message}");
                return false;
            }
            
            keyAuth.response.message = "User Registered Successfully!";
            credentialService.SaveCredentials();
            return true;
            
        }
    }
}
