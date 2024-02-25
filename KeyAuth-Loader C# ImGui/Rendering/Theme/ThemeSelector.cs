using ImGuiNET;
using KeyAuth.Utility;

namespace KeyAuth.Rendering.Theme;

public class ThemeSelector()
{
    private readonly LanguageSelector _ls = new();
    
    private readonly string[] _themeOptions = new[]
    {
        "Default", "Comfy", "Darcula", "Dark Ruda", "Deep Dark", "Discord", "Future Dark", "Gold", "Material Flat",
        "Moonlight", "Nebula"
    };
    private ImGuiStyleType _selectedStyle = ImGuiStyleType.Default;
    
    private enum ImGuiStyleType
    {
        Default,
        ComfyTheme,
        DarculaTheme,
        DarkRudaTheme,
        DeepDarkTheme,
        DiscordTheme,
        FutureDarkTheme,
        GoldTheme,
        MaterialFlatTheme,
        Moonlight,
        NebulaTheme
    }
    
    public void Selector()
    {
        ImGui.Text(_ls.GetString("Theme:"));
        ImGui.SameLine();
        var selectedStyleIndex = (int)_selectedStyle;
        if (!ImGui.Combo("##Theme", ref selectedStyleIndex, _themeOptions, _themeOptions.Length)) return;
        _selectedStyle = (ImGuiStyleType)selectedStyleIndex;
        SetSelectedTheme();
    }
    
    public void SetSelectedTheme()
    {
        switch (_selectedStyle)
        {
            case ImGuiStyleType.ComfyTheme:
                ComfyTheme.SetStyle();
                break;            
            
            case ImGuiStyleType.DarculaTheme:
                DarculaTheme.SetStyle();
                break;

            case ImGuiStyleType.DarkRudaTheme:
                DarkRudaTheme.SetStyle();
                break;
            
            case ImGuiStyleType.DeepDarkTheme:
                DeepDarkTheme.SetStyle();
                break;
            
            case ImGuiStyleType.DiscordTheme:
                DiscordTheme.SetStyle();
                break;
            
            case ImGuiStyleType.FutureDarkTheme:
                FutureDarkTheme.SetStyle();
                break;
            
            case ImGuiStyleType.GoldTheme:
                GoldTheme.SetStyle();
                break;
            
            case ImGuiStyleType.MaterialFlatTheme:
                MaterialFlatTheme.SetStyle();
                break;
                
            case ImGuiStyleType.Moonlight:
                MoonlightTheme.SetStyle();
                break;
            
            case ImGuiStyleType.NebulaTheme:
                NebulaTheme.SetStyle();
                break;
            
            case ImGuiStyleType.Default:
            default:
                DeepDarkTheme.SetStyle(); 
                break;
        }
    }
}