using System.Numerics;
using ImGuiNET;

namespace KeyAuth.Utility
{
    public static class VerticalTabBar
    {
        public static int Render(string[] tabNames)
        {
            var _tab = 0;

            ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 0.0f);

            for (var i = 0; i < tabNames.Length; ++i)
                if (ImGui.Button(tabNames[i], new Vector2(-1, 30)))
                    _tab = i;

            ImGui.PopStyleVar(2);
            return _tab;
        }
    }
}
