using ClickableTransparentOverlay;
using ImGuiNET;
using KeyAuth.Credentials;
using KeyAuth.Utility;
using System.Numerics;
using KeyAuth.Rendering.Theme;

namespace KeyAuth.Rendering;

public class Renderer(api keyAuth, CredentialService credentialService) : Overlay
{
    public static string SystemMessage = "";
    
    private bool _isUpdateAvailable;
    private bool _showCheatListTab;
    private bool _isLoaderShown = true;
    private bool _showFps = false;
    private int _cheat;
    private int _tab;

    private readonly DateTime _connectionTime = DateTime.Now;
    private readonly UpdatesUtils _updatesUtils = new(keyAuth);
    private readonly AuthUtils _authUtils = new(credentialService,keyAuth);
    
    private readonly ThemeSelector _themeSelector = new ThemeSelector();
    private readonly CheckProcess _checkProcess = new CheckProcess();
    private readonly LanguageSelector _ls = new();

    private readonly string[] _tabNames = new[] { "Credentials Login", "License Login", "Register User", "Settings" };
    private readonly string[] _cheatNames = new[] { "CS2", "Dead Island 2", "RoboQuest" };
    
    private readonly string _fontPath = $"Fonts\\LEMONMILKLight.otf";
    private readonly string _fontPath2 = $"Fonts\\arial-unicode-ms.ttf";


    
    protected override unsafe Task PostInitialized()
    { 
        keyAuth.init();
        _checkProcess.CheckCurrentProcess();
        _themeSelector.SetSelectedTheme();
        _isUpdateAvailable = _updatesUtils.AutoUpdate();
        
        ReplaceFont(config =>
        {
            ImGui.GetIO().Fonts.AddFontFromFileTTF(_fontPath, 14.8f, config, ImGui.GetIO().Fonts.GetGlyphRangesDefault());
            config->MergeMode = 1;
            config->OversampleH = 1;
            config->OversampleV = 1;
            config->PixelSnapH = 1;
            ImGui.GetIO().Fonts.AddFontFromFileTTF(_fontPath2, 14.5f, config, ImGui.GetIO().Fonts.GetGlyphRangesCyrillic());
            ImGui.GetIO().Fonts.AddFontFromFileTTF(_fontPath2, 14.8f, config, ImGui.GetIO().Fonts.GetGlyphRangesChineseSimplifiedCommon());
            ImGui.GetIO().Fonts.AddFontFromFileTTF(_fontPath2, 14.8f, config, ImGui.GetIO().Fonts.GetGlyphRangesJapanese());
        });
        
        if (!_isUpdateAvailable)
        {
            _authUtils.CheckAndAutoLogin();
        }
        
        return Task.CompletedTask;
    }
    
    protected override void Render()
    {

        ImGui.SetNextWindowSize(new Vector2(750, 420), ImGuiCond.Once);
        ImGui.SetNextWindowPos(new Vector2(0, 0), ImGuiCond.Once);
        
        ImGui.Begin("KeyAuth - Loader C#", ref _isLoaderShown, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
        {
            RenderStatusTab();
            
            ImGui.BeginChild("#1", new Vector2(230, -29), ImGuiChildFlags.Border);
            {
                if (!_isUpdateAvailable && !_authUtils.isLoginSuccessful &&!_showCheatListTab)
                {   
                    var localizedTabNames = _ls.GetArray(_tabNames);
                    VerticalTabBar.Render(localizedTabNames, ref _tab);  
                }
                
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 110);
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
            
            _ls.RenderLanguageSelector();
            
            if (_isLoaderShown == false)
            {
                this.Close();
            }

        }
        ImGui.End();
    }

    private void RenderSystemMessage()
    {
        ImGui.Spacing();
        ImGui.TextColored(SystemMessage.Contains("Complete") ? Colors.defaultGreen : Colors.defaultRed, $"{_ls.GetString(SystemMessage)}");
    }
    
    private void RenderInfoTab()
    {
        if (_showFps)
        {
            ImGui.Text($"FPS:");
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, $"{(int)ImGui.GetIO().Framerate}");
        }
        ImGui.Spacing();
        ImGui.Text(_ls.GetString("Built at:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{_connectionTime}");
        ImGui.Text(_ls.GetString("Client Version:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen, $"{keyAuth.version}");
        ImGui.Spacing();
    }

    private void RenderStatusTab()
    {
        if (!keyAuth.response.success)
        {
            ImGui.Text(_ls.GetString($"Status:"));
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultRed, _ls.GetString(keyAuth.response.message));
        }
        else
        {
            ImGui.Text(_ls.GetString($"Status:"));
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultGreen, _ls.GetString(keyAuth.response.message));
        }
    }

    private void RenderSettingsTab()
    {
        ImGui.Checkbox(_ls.GetString("Toggle FPS"), ref _showFps);
        
        if (ImGui.Button(_ls.GetString("Restart Loader")))
        {
            _updatesUtils.RestartApplication();
        }
        
        if (ImGui.Button(_ls.GetString("Delete Saved Creds")))
        {
            credentialService.ClearCredentials();
        }
        
        if (ImGui.Button(_ls.GetString("Check Session")))
        {
            _authUtils.CheckSession();
            ImGui.SameLine();
        }
        
        _themeSelector.Selector();
        
        RenderSystemMessage();
    }

    private void RenderCredentialsTab()
    {
        ImGui.Spacing();
        ImGui.Text(_ls.GetString("Username"));
        ImGui.InputText("##Username", ref credentialService.username, 100);
        ImGui.Text(_ls.GetString("Password"));
        ImGui.InputText("##Password", ref credentialService.password, 100, ImGuiInputTextFlags.Password);
        ImGui.Spacing();
        if (ImGui.Button(_ls.GetString("Login")))
        {
            _authUtils.PerformLogin();
        }
    }

    private void RenderLicenseTab()
    {
        ImGui.Spacing();
        ImGui.Text(_ls.GetString("License"));
        ImGui.InputText("##LicenseKey", ref credentialService.key, 100, ImGuiInputTextFlags.Password);
        ImGui.Spacing();
        if (ImGui.Button(_ls.GetString("License Login")))
        {
            _authUtils.PerformLicenseLogin();
        }
    }           
        
    private void RenderRegisterTab()
    {
        ImGui.Spacing();
        ImGui.Text(_ls.GetString("Username"));
        ImGui.InputText("##Username", ref credentialService.username, 100);
        ImGui.Text(_ls.GetString("Password"));
        ImGui.InputText("##Password", ref credentialService.password, 100, ImGuiInputTextFlags.Password);            
        ImGui.Text(_ls.GetString("License"));
        ImGui.InputText("##License", ref credentialService.key, 100, ImGuiInputTextFlags.Password);            
        ImGui.Text(_ls.GetString("Email"));
        ImGui.InputText("##Email", ref credentialService.email, 100);
        ImGui.Spacing();
        if (!ImGui.Button(_ls.GetString("Register Account"))) return;
        _authUtils.PerformRegisterUser();
    }        
        
    private void RenderUpdateTab()
    {
        ImGui.Spacing();
        ImGui.TextColored(Colors.defaultGreen, _ls.GetString("New Update Available!"));
        ImGui.Spacing();
        ImGui.Text(_ls.GetString("The current client version is obsolete.\nPlease download the new version to be able to connect again."));
        ImGui.Spacing();
        if (ImGui.Button(_ls.GetString("Download Manually")))
        {
            _updatesUtils.PerformUpdate();
        }

        ImGui.SameLine();

        if (ImGui.Button(_ls.GetString("Auto-Update")))
        {
            _updatesUtils.PerformAutoUpdate();
        }
    }


    private void RenderSuccessTab()
    {
        ImGui.Spacing();
        ImGui.TextColored(Colors.defaultGreen,_ls.GetString("Client Authenticated Successfully"));
        ImGui.Spacing();
        ImGui.Text(_ls.GetString("Created at:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen,$"{TimeClock.UnixTimeToDateTime(long.Parse(keyAuth.user_data.createdate))}");        
        ImGui.Text(_ls.GetString("Last login at:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultGreen,$"{TimeClock.UnixTimeToDateTime(long.Parse(keyAuth.user_data.lastlogin))}");
        
        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 25);
        ImGui.Text(_ls.GetString("Username:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultOrange, $"{keyAuth.user_data.username}");
        ImGui.Text(_ls.GetString("IP Address:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultOrange, $"{keyAuth.user_data.ip}");
        ImGui.Text(_ls.GetString("Hardware ID:"));
        ImGui.SameLine();
        ImGui.TextColored(Colors.defaultOrange, $"{keyAuth.user_data.hwid}");

        ImGui.NewLine();
        ImGui.Text(_ls.GetString("Your subscription(s):"));
        ImGui.Spacing();
        ImGui.SameLine();
        ImGui.Spacing();
        
        foreach (var t in keyAuth.user_data.subscriptions)
        {
            ImGui.Text(_ls.GetString($"Subscription name:"));
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultCyan, $"{t.subscription}");
            ImGui.SameLine();
            ImGui.Text(_ls.GetString("Expires at:"));
            ImGui.SameLine();
            ImGui.TextColored(Colors.defaultRed, $"{TimeClock.UnixTimeToDateTime(long.Parse(t.expiry))}");
        }
        ImGui.Spacing();
        if (ImGui.Button(_ls.GetString("Choose Cheat")))
        {
            _authUtils.isLoginSuccessful = false;
            _showCheatListTab = true;
            _tab = 4;
            _cheat = 4;
        }
        ImGui.SameLine();
        if (!ImGui.Button(_ls.GetString("Logout"))) return;
        _authUtils.isLoginSuccessful = false;
        _authUtils.Logout();
        _tab = 1;
    }

    private async void RenderCheatListTab()
    {
        ImGui.TextColored(Colors.defaultGreen, _ls.GetString("Press a button to start the cheat\n make sure to start the game first."));
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