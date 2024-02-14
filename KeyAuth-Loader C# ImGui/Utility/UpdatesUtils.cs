using ImGuiNET;
using KeyAuth.Renderer;
using System.Diagnostics;
using System.Net;

namespace KeyAuth.Utility
{
    public class UpdatesUtils
    {
        public void autoUpdate(ref bool isUpdateAvailable)
        {
            if (Program.KeyAuthApp.response.message == "invalidver")
            {
                Program.KeyAuthApp.response.message = "Invalid Client Version Detected";

                if (!string.IsNullOrEmpty(Program.KeyAuthApp.app_data.downloadLink))
                {
                    isUpdateAvailable = true;
                }
                else
                {
                    ImGui.Text("Status: The version of this program does not match the one online, and the online download link is not set. Please contact the developer for assistance.");
                    Environment.Exit(0);
                }
            }
        }

        public void PerformUpdate()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Program.KeyAuthApp.app_data.downloadLink,
                    UseShellExecute = true
                });
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ImGui.Text($"An error occurred while trying to open the URL: {ex.Message}");
            }
        }

        public void PerformAutoUpdate()
        {
            WebClient webClient = new WebClient();
            string destFile = Application.ExecutablePath;

            string rand = Randomize.TextString();

            destFile = Path.Combine(Path.GetDirectoryName(destFile), $"{rand}.exe");
            webClient.DownloadFile(Program.KeyAuthApp.app_data.downloadLink, destFile);

            Process.Start(destFile);
            Process.Start(new ProcessStartInfo()
            {
                Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            });
            Environment.Exit(0);
        }
    }
}
