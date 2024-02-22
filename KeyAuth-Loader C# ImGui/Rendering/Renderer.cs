using ClickableTransparentOverlay;
using ImGuiNET;
using KeyAuth.Credentials;
using KeyAuth.Utility;
using System.Numerics;
using KeyAuth.Rendering.Theme;

namespace KeyAuth.Rendering;

public class Renderer(api keyAuth,CredentialService credentialService) : Overlay
{
    private bool _isUpdateAvailable;
    private bool _showCheatListTab;
    private bool _isLoaderShown = true;
    private bool _showFps = false;
    private int _cheat;
    private int _tab;

    private readonly DateTime _connectionTime = DateTime.Now;

    private readonly UpdatesUtils _updatesUtils = new(keyAuth);
    private readonly ThemeSelector _themeSelector = new ThemeSelector();
    private readonly CheckProcess _checkProcess = new CheckProcess();
    private readonly AuthUtils _authUtils = new(credentialService,keyAuth);

    private readonly string[] _tabNames = new[] { "Credentials Login", "License Login", "Register User", "Settings" };
    private readonly string[] _cheatNames = new[] { "CS2", "Dead Island 2", "RoboQuest" };

    private readonly string _fontPath = $"{AppDomain.CurrentDomain.BaseDirectory}Fonts\\LEMONMILKLight.otf";

    protected override unsafe Task PostInitialized()
    { 
        keyAuth.init();
        _themeSelector.SetSelectedTheme();
        _isUpdateAvailable = _updatesUtils.AutoUpdate();
        
        ReplaceFont(config =>
        {
            if (!File.Exists(_fontPath))
            {
                ReplaceFont(@"C:\Windows\Fonts\NirmalaB.ttf", 16, FontGlyphRangeType.English);
            }
            else
            {
                ImGui.GetIO().Fonts.AddFontFromFileTTF(_fontPath, 13.5f, config, ImGui.GetIO().Fonts.GetGlyphRangesDefault());
            }
        });
        
        if (!_isUpdateAvailable)
        {
            _authUtils.CheckAndAutoLogin();
        }
       
        _checkProcess.CheckCurrentProcess();
        return Task.CompletedTask;
    }
    
    protected override void Render()
    {

        ImGui.SetNextWindowSize(new Vector2(670, 400), ImGuiCond.Once);
        ImGui.SetNextWindowPos(new Vector2(0, 0), ImGuiCond.Once);
        
        ImGui.Begin("KeyAuth - Loader C#", ref _isLoaderShown, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
        {
            RenderStatusTab();

            ImGui.BeginChild("#1", new Vector2(200, -29), ImGuiChildFlags.Border);
            {
                if (!_isUpdateAvailable && !_authUtils.isLoginSuccessful &&!_showCheatListTab)
                {   
                    VerticalTabBar.Render(_tabNames, ref _tab);  
                }
                
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 95);
                RenderInfoTab();
            }
            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("#2", new Vector2(0, -29), ImGuiChildFlags.Border);
            {
                if (_isUpdateAvailable)
                {
                    RenderUpdateTab();
                }
                else if (!_authUtils.isLoginSuccessful)
                {
                    switch (_tab)
                    {
                        case 0:
                            RenderCredentialsTab();
                            break;
                        case 1:
                            RenderLicenseTab();
                            break;
                        case 2:
                            RenderRegisterTab();
                            break;
                        case 3:
                            RenderSettingsTab();
                            break;
                    }
                }

                if (_showCheatListTab)
                {
                    RenderCheatListTab();
                }
                
                if (_authUtils.isLoginSuccessful)
                {
                    RenderSuccessTab();
                }
                
            }
            ImGui.EndChild();
            
            _themeSelector.Selector();
            
            if (_isLoaderShown == false)
            {
                this.Close();
            }

        }
        ImGui.End();
    }
    
    private void RenderInfoTab()
    {
        if (_showFps)
        {
            ImGui.Text($"FPS:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, $"{ImGui.GetIO().Framerate}");
        }
        ImGui.Spacing();
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

    private void RenderSettingsTab()
    {
        ImGui.Checkbox("Toggle FPS", ref _showFps);
        ImGui.SameLine();
        if (ImGui.Button("Restart Loader"))
        {
            _updatesUtils.RestartApplication();
        }
        ImGui.SameLine();
        if (ImGui.Button("Delete Saved Creds"))
        {
            credentialService.ClearCredentials();
        }
        ImGui.SameLine();
        if (ImGui.Button("Check Session"))
        {
            _authUtils.CheckSession();
            ImGui.SameLine();
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
        ImGui.InputText("##LicenseKey", ref credentialService.key, 100, ImGuiInputTextFlags.Password);
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
        ImGui.InputText("##License", ref credentialService.key, 100, ImGuiInputTextFlags.Password);            
        ImGui.Text("Email");
        ImGui.InputText("##Email", ref credentialService.email, 100);
        ImGui.Spacing();
        if (!ImGui.Button("Register Now")) return;
        _authUtils.PerformRegisterUser();
        _tab = 0;
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
        ImGui.Spacing();
        ImGui.Text("Created at:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen,$"{TimeClock.UnixTimeToDateTime(long.Parse(keyAuth.user_data.createdate))}");        
        ImGui.Text("Last login at:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen,$"{TimeClock.UnixTimeToDateTime(long.Parse(keyAuth.user_data.lastlogin))}");
        
        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 25);
        ImGui.Text("Username:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultOrange, $"{keyAuth.user_data.username}");
        ImGui.Text("IP Address:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultOrange, $"{keyAuth.user_data.ip}");
        ImGui.Text("Hardware ID:");
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultOrange, $"{keyAuth.user_data.hwid}");

        ImGui.NewLine();
        ImGui.Text("Your subscription(s):");
        ImGui.Spacing();
        ImGui.SameLine();
        ImGui.Spacing();
        
        foreach (var t in keyAuth.user_data.subscriptions)
        {
            ImGui.Text($"Subscription name:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultCyan, $"{t.subscription}");
            ImGui.SameLine();
            ImGui.Text("Expires at:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultRed, $"{TimeClock.UnixTimeToDateTime(long.Parse(t.expiry))}");
        }
        ImGui.Spacing();
        if (ImGui.Button("Choose Cheat"))
        {
            _authUtils.isLoginSuccessful = false;
            _showCheatListTab = true;
            _tab = 4;
            _cheat = 4;
        }
        ImGui.SameLine();
        if (!ImGui.Button("Logout")) return;
        _authUtils.isLoginSuccessful = false;
        _authUtils.Logout();
        _tab = 1;
    }

    private async void RenderCheatListTab()
    {
        ImGui.TextColored(Colors.defaultGreen, "Press a button to start the cheat, make sure to start the game first.");
        ImGui.NewLine();
        CheatList.Render(_cheatNames, ref _cheat);
        
        switch (_cheat)
        {
            case 0:
                ImGui.Text("Button for CS2 execution code has been pressed");
                await Task.Delay(2000);
                Environment.Exit(0);
                break;
            case 1:
                ImGui.Text("Button for Dead Island 2 execution code has been pressed");
                await Task.Delay(2000);
                Environment.Exit(0);
                break;
            case 2:
                ImGui.Text("Button for RoboQuest execution code has been pressed");
                await Cheats.DownloadAndRun("");
                break;
        }
    } 
}