using ImGuiNET;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using KeyAuth.Rendering;

namespace KeyAuth.Utility
{
    public class UpdatesUtils (api keyAuth)
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
        public bool AutoUpdate()
        {
            if (keyAuth.response.message == "invalidver")
            {
                keyAuth.response.message = "Invalid Client Version Detected";

                if (!string.IsNullOrEmpty(keyAuth.app_data.downloadLink))
                {
                    return true;
                }
                MessageBox(IntPtr.Zero, " The version of this program does not match the one online, and the online download link is not set. Please contact the developer for assistance.", "Status", 0);
                Environment.Exit(0);
            }
            return false;
        }
        

        public void PerformUpdate()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = keyAuth.app_data.downloadLink,
                    UseShellExecute = true
                });
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                ImGui.Text($"An error occurred while trying to open the URL: {ex.Message}");
            }
        }

        [RequiresAssemblyFiles()]
        public async void PerformAutoUpdate()
        {
            try
            {
                Process.GetCurrentProcess();
                string? exePath = Process.GetCurrentProcess().MainModule?.FileName;
                string rand = Guid.NewGuid().ToString();
                exePath = Path.Combine(Path.GetDirectoryName(exePath) ?? string.Empty, $"{rand}.exe");

                using HttpClient client = new HttpClient();
                byte[] data = await client.GetByteArrayAsync(keyAuth.app_data.downloadLink);
                await File.WriteAllBytesAsync(exePath, data);

                Process.Start(exePath);
                Process.Start(new ProcessStartInfo
                {
                    Arguments = $"/C choice /C Y /N /D Y /T 3 & Del \"{Process.GetCurrentProcess().MainModule?.FileName}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        
        public async Task RestartApplication()
        {
            try
            {
                Renderer.SystemMessage = "Completed: Loader will be restarted, please wait few seconds";
                Process currentProcess = Process.GetCurrentProcess();
                string? exePath = currentProcess.MainModule?.FileName;

                if (exePath == null) return;
                await Task.Delay(2000);
                Process.Start(new ProcessStartInfo
                {
                    Arguments = $"/C choice /C Y /N /D Y /T 2 & start \"\" \"{exePath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
