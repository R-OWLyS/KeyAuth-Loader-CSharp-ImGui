﻿using ImGuiNET;
using System.Diagnostics;

namespace KeyAuth.Utility
{
    public class UpdatesUtils (api keyAuth)
    {
        public bool AutoUpdate()
        {
            if (keyAuth.response.message == "invalidver")
            {
                keyAuth.response.message = "Invalid Client Version Detected";

                if (!string.IsNullOrEmpty(keyAuth.app_data.downloadLink))
                {
                    return true;
                }
                else
                {
                    ImGui.Text("Status: The version of this program does not match the one online, and the online download link is not set. Please contact the developer for assistance.");
                    Environment.Exit(0);
                }
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


        public async void PerformAutoUpdate()
        {
            try
            {
                string destFile = Application.ExecutablePath;
                string rand = Guid.NewGuid().ToString();
                destFile = Path.Combine(Path.GetDirectoryName(destFile) ?? string.Empty, $"{rand}.exe");

                using HttpClient client = new HttpClient();
                byte[] data = await client.GetByteArrayAsync(keyAuth.app_data.downloadLink);
                await File.WriteAllBytesAsync(destFile, data);

                Process.Start(destFile);
                Process.Start(new ProcessStartInfo
                {
                    Arguments = $"/C choice /C Y /N /D Y /T 3 & Del \"{Application.ExecutablePath}\"",
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
