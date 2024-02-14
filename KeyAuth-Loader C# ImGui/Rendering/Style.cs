using System.Numerics;
using ImGuiNET;

namespace KeyAuth.Renderer
{
    public static class Style
    {
        public static void SetStyle()
        {
            ImGuiStylePtr style = ImGui.GetStyle();

            style.WindowPadding = new Vector2(11, 11);
            style.WindowRounding = 5.0f;
            style.FramePadding = new Vector2(4, 4);
            style.FrameRounding = 3.0f;
            style.ItemSpacing = new Vector2(9, 5);
            style.ItemInnerSpacing = new Vector2(5, 4);
            style.IndentSpacing = 21.0f;
            style.ScrollbarSize = 13.0f;
            style.ScrollbarRounding = 7.0f;
            style.GrabMinSize = 4.0f;
            style.GrabRounding = 3.0f;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 2.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 4f);
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowTitleAlign, 6f);

            ImGui.PushStyleColor(ImGuiCol.MenuBarBg, Colors.defaultBlack);
            ImGui.PushStyleColor(ImGuiCol.WindowBg, Colors.defaultBlack);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, Colors.defaultPurpleDark);
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.Button, Colors.defaultPurpleDark);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.Header, Colors.defaultPurpleDark);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.TextSelectedBg, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.TitleBg, Colors.defaultBlack);
            ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, Colors.defaultBlack);
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, Colors.defaultBlack);
            ImGui.PushStyleColor(ImGuiCol.Tab, Colors.defaultPurpleDark);
            ImGui.PushStyleColor(ImGuiCol.TabHovered, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.TabActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocused, Colors.defaultPurpleDark);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(1f, 1f, 1f, 1f));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowTitleAlign, new Vector2(0.5f, 0.5f));
            ImGui.PushStyleColor(ImGuiCol.SliderGrab, new Vector4(1f, 1f, 1f, 1f));
            ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new Vector4(1f, 1f, 1f, 1f));
            ImGui.PushStyleColor(ImGuiCol.ScrollbarBg, Colors.defaultPurpleDark);
            ImGui.PushStyleColor(ImGuiCol.ScrollbarGrab, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabActive, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabHovered, Colors.defaultPurple); 
            ImGui.PushStyleColor(ImGuiCol.Border, Colors.defaultPurple);
            ImGui.PushStyleColor(ImGuiCol.Separator, Colors.defaultPurple);

        }
    }

    public static class Colors
    {
        public static Vector4 defaultBlack = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        public static Vector4 defaultPurple = new Vector4(0.19339603f, 0.03854112f, 0.3184713f, 1f);
        public static Vector4 defaultPurpleDark = new Vector4(0.12941177f, 0.02745098f, 0.21176471f, 1);
        public static Vector4 defaultGreen = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
        public static Vector4 defaultRed = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
        public static Vector4 defaultOrange = new Vector4(1.0f, 0.647f, 0.0f, 1.0f);
        public static Vector4 defaultBlue = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
        public static Vector4 defaultYellow = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
        public static Vector4 defaultPink = new Vector4(1.0f, 0.753f, 0.796f, 1.0f);
        public static Vector4 defaultCyan = new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
        public static Vector4 defaultMagenta = new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
        public static Vector4 defaultBrown = new Vector4(0.647f, 0.165f, 0.165f, 1.0f);
    }

}