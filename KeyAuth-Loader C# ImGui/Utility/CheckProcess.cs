using System.Diagnostics;

namespace KeyAuth.Utility
{
    public class CheckProcess
    {
        private static readonly Mutex mutex = new Mutex(true, "DADDYVYNXC");

        public void CheckCurrentProcess()
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                Environment.Exit(0);
            }

            try
            {
                string? currentProcessFileName = Process.GetCurrentProcess().MainModule?.FileName;

                if (currentProcessFileName == null)
                    return;

                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(currentProcessFileName));

                if (processes.Length > 1)
                {
                    Environment.Exit(0);
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}