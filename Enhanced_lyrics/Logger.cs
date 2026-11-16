using System;
using System.IO;

namespace MusicBeePlugin
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        public static string FilePath;

        public static void Log(string message)
        {
            if (FilePath == null) return;

            try
            {
                lock (_lock)
                {
                    File.AppendAllText(
                        FilePath,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        + " " + message
                        + Environment.NewLine
                    );
                }
            }
            catch
            {

            }
        }
    }
}