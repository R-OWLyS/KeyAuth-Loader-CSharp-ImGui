using System.Numerics;
using ImGuiNET;

namespace KeyAuth.Utility
{
    public static class VerticalTabBar
    {
        public static void Render(string[] tabNames,ref int tab)
        {

            ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 2.0f);

            for (var i = 0; i < tabNames.Length; ++i)
                if (ImGui.Button(tabNames[i], new Vector2(-1, 30)))
                    tab = i;

            ImGui.PopStyleVar(2);
        }
    }
}
