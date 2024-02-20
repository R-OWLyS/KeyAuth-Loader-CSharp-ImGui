using ImGuiNET;
using KeyAuth.Rendering.Theme;

namespace KeyAuth.Utility;

public static class CheatList
{
    public static void Render(string[] cheatNames,ref int tab)
    {

        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 2.0f);
        
        ImGui.SeparatorText("List of available cheats");
        //ImGui.TextColored(Colors.defaultMagenta, "List of available cheats.");
        ImGui.NewLine();
        for (var i = 0; i < cheatNames.Length; ++i)
        {
            if (ImGui.Button(cheatNames[i]))
                tab = i;
        }
        ImGui.PopStyleVar(2);
    }
}