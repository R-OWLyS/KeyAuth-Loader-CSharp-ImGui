using ClickableTransparentOverlay;
using ImGuiNET;
using KeyAuth.Credentials;
using KeyAuth.Renderer;
using KeyAuth.Utility;

namespace KeyAuth.Rendering;

public class Renderer(api keyAuth,CredentialService credentialService) : Overlay
{
    private bool _isUpdateAvailable;
    private bool _isLoaderShown = true;
    private int _tab;

    private readonly DateTime _connectionTime = DateTime.Now;

    private readonly UpdatesUtils _updatesUtils = new(keyAuth);
    private readonly AuthUtils _authUtils = new(credentialService,keyAuth);
    private static readonly string[] tabNames = { "Main", "Credentials", "License", "Register", "Update" };

    protected override Task PostInitialized()
    { 
        Style.SetStyle();
        ReplaceFont(@"C:\Windows\Fonts\segoeuib.ttf", 16, FontGlyphRangeType.English);
        keyAuth.init();
        _isUpdateAvailable = _updatesUtils.AutoUpdate();
        if (!_isUpdateAvailable)
        {
            _authUtils.CheckAndAutoLogin();
        }
        return Task.CompletedTask;
    }

    protected override void Render()
    {
        ImGui.Begin("KeyAuth - Loader C#", ref _isLoaderShown,ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse);

        _tab = VerticalTabBar.Render(tabNames);

        ImGui.Text($"Built at:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{_connectionTime}");
        ImGui.Text($"Client Version:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{keyAuth.version}");
            

        if (!keyAuth.response.success)
        {
            ImGui.Text($"Status:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultRed, keyAuth.response.message);
        }
        else
        {
            ImGui.Text($"Status:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, keyAuth.response.message);
        }
        ImGui.Spacing();
        if (!string.IsNullOrEmpty(_authUtils.systemMessage))
        {
            ImGui.Separator();
            ImGui.TextColored(Colors.defaultOrange, _authUtils.systemMessage);
            ImGui.Separator();
        }

        switch (_tab)
        {
            case 1:
                RenderCredentialsTab();
                break;
            case 2:
                RenderLicenseTab();
                break;
            case 3:
                RenderRegisterTab();
                break;
            case 4:
                RenderUpdateTab();
                break;
            default:
                RenderMenuButtons();
                break;
        }

        if (_authUtils.isLoginSuccessful)
        {
            _tab = 0;
            RenderSuccessTab();
        }
        if (_isLoaderShown == false)
        {
            this.Close();
        }
    }

    private void RenderMenuButtons()
    {
        if (_authUtils.isLoginSuccessful)
        {
            return;
        }

        if (ImGui.Button("Credentials Login"))
        {
            _tab = 1;
        }

        ImGui.SameLine();

        if (ImGui.Button("License Only"))
        {
            _tab = 2;
        }

        ImGui.SameLine();

        if (ImGui.Button("Register User"))
        {
            _tab = 3;
        }
    }

    private void RenderCredentialsTab()
    {
        ImGui.NewLine();
        ImGui.SeparatorText("Credentials Login");
        ImGui.Spacing();
        ImGui.Text("Username");
        ImGui.InputText("##Username", ref credentialService.username, 100);
        ImGui.Text("Password");
        ImGui.InputText("##Password", ref credentialService.password, 100, ImGuiInputTextFlags.Password);
        ImGui.Spacing();
        if (ImGui.Button("Login"))
        {
            _authUtils.PerformLogin();
        }
    }

    private void RenderLicenseTab()
    {
        ImGui.NewLine();
        ImGui.SeparatorText("License Login");
        ImGui.Spacing();
        ImGui.Text("License");
        ImGui.InputText("##Password", ref credentialService.key, 100);
        ImGui.Spacing();
        if (ImGui.Button("License Login"))
        {
            _authUtils.PerformLicenseLogin();
        }
    }           
        
    private void RenderRegisterTab()
    {
        ImGui.NewLine();
        ImGui.SeparatorText("Register User");
        ImGui.Spacing();
        ImGui.Text("Username");
        ImGui.InputText("##Username", ref credentialService.username, 100);
        ImGui.Text("Password");
        ImGui.InputText("##Password", ref credentialService.password, 100, ImGuiInputTextFlags.Password);            
        ImGui.Text("License");
        ImGui.InputText("##License", ref credentialService.key, 100);            
        ImGui.Text("Email");
        ImGui.InputText("##Email", ref credentialService.email, 100);
        ImGui.Spacing();
        if (!ImGui.Button("Register Now")) return;
        var success = _authUtils.PerformRegisterUser();
        if (success) _tab = 0;
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
            _updatesUtils.PerformUpdate();
        }

        ImGui.SameLine();

        if (ImGui.Button("Auto-Update"))
        {
            _updatesUtils.PerformAutoUpdate();
        }
    }


    private void RenderSuccessTab()
    {
        ImGui.Spacing();
        ImGui.TextColored(Colors.defaultGreen,"Client Authenticated");
        ImGui.SeparatorText("User Details");
        ImGui.Text($"Username:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{keyAuth.user_data.username}");
        ImGui.Text($"IP address: {keyAuth.user_data.ip}");
        ImGui.Text($"Hardware-Id: {keyAuth.user_data.hwid}");
        ImGui.Text("Your subscription(s):");

        foreach (var t in keyAuth.user_data.subscriptions)
        {
            ImGui.Text($"Subscription name: {t.subscription} - Expires at: {TimeClock.UnixTimeToDateTime(long.Parse(t.expiry))}");
        }
        ImGui.Spacing();
        if (ImGui.Button("Run Cheat"))
        {
            Environment.Exit(0);
        }
    }

}