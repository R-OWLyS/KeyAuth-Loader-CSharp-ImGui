using System.Diagnostics;

namespace KeyAuth.Utility
{
    public static class Cheats
    {
        public static async Task DownloadAndRun(string url)
        {
            try
            {
                string tempFolder = Path.GetTempPath();
                string rand = Guid.NewGuid().ToString();
                string destFile = Path.Combine(tempFolder, $"{rand}.exe");

                using HttpClient client = new HttpClient();
                byte[] data = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(destFile, data);

                Process.Start(destFile);
                Process.Start(new ProcessStartInfo
                {
                    FileName = destFile,
                    Arguments = "customparameterforstartingtheprogram",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
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




/* The following code can be implemented in your cheat code to verify whether the program has been initiated with a custom parameter for starting.
 you could use this in order to make sure that cheat can be only started from the loader.
 
public static void Main(string[] args)
{
    try
    {
        if (args.Length > 0 && args[0] == "customparameterforstartingtheprogram")
        {
            put here the logic that start your cheat
        }
        else
        { 
            Console.WriteLine("Unable to start program cause of wrong param.");
            and here you can also implement an easy logic to delete the file if is not started with the custom parameter
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error as occured: {ex.Message}");
    }
}*/