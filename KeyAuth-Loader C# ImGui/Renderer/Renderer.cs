using ClickableTransparentOverlay;
using ImGuiNET;
using KeyAuth.Utility;

namespace KeyAuth.Renderer
{
    public class KeyAuth_Renderer : Overlay
    {
        public bool isLoginSuccessful = false;
        public bool isUpdateAvailable = false;
        private bool isLoaderShown = true;

        public int tab = 0;

        public string username = "";
        public string password = "";
        public string key = "";
        public string email = "";

        public string systemMessage = "";

        private DateTime connectionTime;

        UpdatesUtils updatesUtils = new UpdatesUtils();
        AuthUtils authUtils = new AuthUtils();

        public KeyAuth_Renderer()
        {
            connectionTime = DateTime.Now;
        }

        protected override Task PostInitialized()
        { 
            Style.SetStyle();
            ReplaceFont(@"C:\Windows\Fonts\segoeuib.ttf", 16, FontGlyphRangeType.English);
            VSync = true;

            Program.KeyAuthApp.init();

            updatesUtils.autoUpdate(ref isUpdateAvailable);
            authUtils.CheckAndAutoLogin(this);

            return Task.CompletedTask;
        }

        protected override void Render()
        {
            ImGui.Begin("KeyAuth - Loader C#", ref isLoaderShown,ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse);

            ImGui.Text($"Built at:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, $"{connectionTime}");
            ImGui.Text($"Client Version:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, $"{Program.KeyAuthApp.version}");
            

            if (!Program.KeyAuthApp.response.success)
            {
                ImGui.Text($"Status:");
                ImGui.SameLine();
                ImGui.TextColored(Colors.defaultRed, Program.KeyAuthApp.response.message);
            }
            else
            {
                ImGui.Text($"Status:");
                ImGui.SameLine();
                ImGui.TextColored(Colors.defaultGreen, Program.KeyAuthApp.response.message);
            }

            if (!string.IsNullOrEmpty(systemMessage))
            {
                ImGui.Separator();
                ImGui.TextColored(Colors.defaultOrange, systemMessage);
                ImGui.Separator();
            }

            if (!isUpdateAvailable)
            {
                RenderMenuButtons();
            }
            else
            {
                RenderUpdateTab();
            }

            if (tab == 1)
            {
                RenderCredentialsTab();
            }

            if (tab == 2)
            {
                RenderLicenseTab();
            }

            if (tab == 3)
            {
                RenderRegisterTab();
            }

            if (isLoginSuccessful)
            {
                tab = 0;
                RenderSuccessTab();
            }
            if (isLoaderShown == false)
            {
                this.Close();
            }
        }

        private void RenderMenuButtons()
        {
            if (isLoginSuccessful)
            {
                return;
            }

            if (ImGui.Button("Credentials Login"))
            {
                tab = 1;
            }

            ImGui.SameLine();

            if (ImGui.Button("License Only"))
            {
                tab = 2;
            }

            ImGui.SameLine();

            if (ImGui.Button("Register User"))
            {
                tab = 3;
            }
        }

        private void RenderCredentialsTab()
        {
            ImGui.NewLine();
            ImGui.SeparatorText("Credentials Login");
            ImGui.Spacing();
            ImGui.Text("Username");
            ImGui.InputText("##Username", ref username, 100);
            ImGui.Text("Password");
            ImGui.InputText("##Password", ref password, 100, ImGuiInputTextFlags.Password);
            ImGui.Spacing();
            if (ImGui.Button("Login"))
            {
                authUtils.PerformLogin(this);
            }
        }

        private void RenderLicenseTab()
        {
            ImGui.NewLine();
            ImGui.SeparatorText("License Login");
            ImGui.Spacing();
            ImGui.Text("License");
            ImGui.InputText("##Password", ref key, 100);
            ImGui.Spacing();
            if (ImGui.Button("License Login"))
            {
                authUtils.PerformLicenseLogin(this);
            }
        }           
        
        private void RenderRegisterTab()
        {
            ImGui.NewLine();
            ImGui.SeparatorText("Register User");
            ImGui.Spacing();
            ImGui.Text("Username");
            ImGui.InputText("##Username", ref username, 100);
            ImGui.Text("Password");
            ImGui.InputText("##Password", ref password, 100, ImGuiInputTextFlags.Password);            
            ImGui.Text("License");
            ImGui.InputText("##License", ref key, 100);            
            ImGui.Text("Email");
            ImGui.InputText("##Email", ref email, 100);
            ImGui.Spacing();
            if (ImGui.Button("Register Now"))
            {
                authUtils.PerformRegisterUser(this);
            }
        }        
        
        private void RenderUpdateTab()
        {
            ImGui.NewLine();
            ImGui.SeparatorText("KeyAuth - Client Update");
            ImGui.TextColored(Colors.defaultGreen, "New Update Available!");
            ImGui.Spacing();
            ImGui.Text("The current client version is obsolete.\nPlease download the new version to be able to connect again.");
            ImGui.Spacing();
            if (ImGui.Button("Download Manually"))
            {
                updatesUtils.PerformUpdate();
            }

            ImGui.SameLine();

            if (ImGui.Button("Auto-Update"))
            {
                updatesUtils.PerformAutoUpdate();
            }
        }


        private void RenderSuccessTab()
        {
            ImGui.Spacing();
            ImGui.TextColored(Colors.defaultGreen,"Client Authenticated");
            ImGui.SeparatorText("User Details");
            ImGui.Text($"Username:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, $"{Program.KeyAuthApp.user_data.username}");
            ImGui.Text($"IP address: {Program.KeyAuthApp.user_data.ip}");
            ImGui.Text($"Hardware-Id: {Program.KeyAuthApp.user_data.hwid}");
            ImGui.Text("Your subscription(s):");

            if (Program.KeyAuthApp.user_data.subscriptions != null)
            {
                for (var i = 0; i < Program.KeyAuthApp.user_data.subscriptions.Count; i++)
                {
                    ImGui.Text($"Subscription name: {Program.KeyAuthApp.user_data.subscriptions[i].subscription} - Expires at: {TimeClock.UnixTimeToDateTime(long.Parse(Program.KeyAuthApp.user_data.subscriptions[i].expiry))}");
                }
            }
            else
            {
                ImGui.Text("No subscriptions found.");
            }
            ImGui.Spacing();
            if (ImGui.Button("Run Cheat"))
            {
                Environment.Exit(0);
            }
        }

    }
}

