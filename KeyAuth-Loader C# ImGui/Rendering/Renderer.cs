using ClickableTransparentOverlay;
using ImGuiNET;
using KeyAuth.Credentials;
using KeyAuth.Renderer;
using KeyAuth.Utility;
using System.Numerics;

namespace KeyAuth.Rendering;

public class Renderer(api keyAuth,CredentialService credentialService) : Overlay
{
    private bool _isUpdateAvailable;
    private bool _isLoaderShown = true;
    private int _tab;

    private readonly DateTime _connectionTime = DateTime.Now;

    private readonly UpdatesUtils _updatesUtils = new(keyAuth);
    private readonly AuthUtils _authUtils = new(credentialService,keyAuth);

    private readonly string[] _tabNames = { "Credentials Login", "License Login", "Register User" };

    protected override Task PostInitialized()
    { 
        Style.SetStyle();
        ReplaceFont(@"C:\Windows\Fonts\NirmalaB.ttf", 16, FontGlyphRangeType.English);
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
        ImGui.SetNextWindowSize(new Vector2(650, 330), ImGuiCond.Once);
        ImGui.SetNextWindowPos(new Vector2(0, 0), ImGuiCond.Once);

        ImGui.Begin("KeyAuth - Loader C#", ref _isLoaderShown, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
        {
            if (!string.IsNullOrEmpty(_authUtils.systemMessage))
            {
                ImGui.Separator();
                ImGui.TextColored(Colors.defaultOrange, _authUtils.systemMessage);
                ImGui.Separator();
            }

            RenderStatusTab();

            ImGui.BeginChild("#1", new Vector2(200, -1), ImGuiChildFlags.Border);
            {
                if (!_isUpdateAvailable && !_authUtils.isLoginSuccessful)
                {   
                    VerticalTabBar.Render(_tabNames, ref _tab);  
                }
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 70);
                RenderInfoTab();
            }
            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("#2", new Vector2(0, -1), ImGuiChildFlags.Border);
            {
                if (_isUpdateAvailable)
                {
                    RenderUpdateTab();
                }
                else if (!_authUtils.isLoginSuccessful)
                {
                    if (_tab == 0)
                    {
                        RenderCredentialsTab();
                    }
                    else if (_tab == 1)
                    {
                        RenderLicenseTab();
                    }
                    else if (_tab == 2)
                    {
                        RenderRegisterTab();
                    }
                }

                if (_authUtils.isLoginSuccessful)
                {
                    RenderSuccessTab();
                }
            }
            ImGui.EndChild();

            if (_isLoaderShown == false)
            {
                this.Close();
            }
        }
        ImGui.End();
    }


    private void RenderInfoTab()
    {
        ImGui.Text($"Built at:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{_connectionTime}");
        ImGui.Text($"Client Version:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{keyAuth.version}");
        ImGui.Spacing();
    }

    private void RenderStatusTab()
    {
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
    }

    private void RenderCredentialsTab()
    {
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
        ImGui.Spacing();
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
        ImGui.TextColored(Colors.defaultGreen,"Client Authenticated Successfully");
        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 25);
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