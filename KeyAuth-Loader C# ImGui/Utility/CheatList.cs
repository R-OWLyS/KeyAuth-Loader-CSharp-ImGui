using ImGuiNET;

namespace KeyAuth.Utility;

public static class CheatList
{
    public static void Render(string[] cheatNames,ref int tab)
    {

        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 2.0f);
        
        for (var i = 0; i < cheatNames.Length; ++i)
        {
            
            ImGui.Text("Press the button to start " + cheatNames[i] + " cheat");
            ImGui.SameLine();
            
            if (ImGui.Button(cheatNames[i]))
                tab = i;
        }
        ImGui.PopStyleVar(2);
    }
}