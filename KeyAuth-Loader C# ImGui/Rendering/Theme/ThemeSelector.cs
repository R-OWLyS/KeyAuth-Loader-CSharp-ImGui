using ImGuiNET;

namespace KeyAuth.Rendering.Theme;

public class ThemeSelector
{
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
        var selectedStyleIndex = (int)_selectedStyle;
        if (!ImGui.Combo("Loader Theme", ref selectedStyleIndex, "Default\0Comfy\0Darcula\0Dark Ruda\0Deep Dark\0Discord\0Future Dark\0Gold\0Material Flat\0Moonlight\0Nebula\0")) return;
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
                DefaultTheme.SetStyle(); 
                break;
        }
    }
}