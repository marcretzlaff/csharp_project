using System;
using System.IO;
using System.Text;

namespace MyLog
{
    public static class Log
    {
        private static readonly string path = @"./Logs/log.txt";
        private static readonly string directorypath = @"./Logs";
        static Log()
        {
            CreateLogFile();
        }

        public static void CreateLogFile()
        {
            Directory.CreateDirectory(directorypath);
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public static void Delete()
        {
            File.Delete(path);
        }

        public static void WriteException(Exception exception)
        {
            using (StreamWriter fs = new StreamWriter(path, true))
            {
                StringBuilder message = new StringBuilder();

                message.AppendLine("<---------------" + DateTime.Now.ToString("G") + "--------------->");
                message.AppendLine("Message: " + exception.Message);
                message.AppendLine("Stacktrace: " + exception.StackTrace);
                message.AppendLine("");

                fs.Write(message.ToString());
            }
        }

        public static void WriteLog(string text)
        {
            using (StreamWriter fs = new StreamWriter(path,true))
            {
                StringBuilder message = new StringBuilder();

                message.AppendLine("<---------------" + DateTime.Now.ToString("G") + "--------------->");
                message.AppendLine(text);
                message.AppendLine("");

                fs.Write(message.ToString());
            }
        }

        public static void OpenLog()
        {
            System.Diagnostics.Process.Start("notepad.exe", path);
        }
    }
}
