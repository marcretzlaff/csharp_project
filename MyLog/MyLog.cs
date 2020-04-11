using System;
using System.IO;
using System.Text;

namespace MyLog
{
    public static class Log
    {
        static Log()
        {
            CreateLogFile();
        }

        public static void CreateLogFile()
        {
            Directory.CreateDirectory("./Logs");
            if (!File.Exists("./Logs/log.txt"))
            {
                File.Create("./Logs/log.txt");
            }
        }

        public static void WriteException(Exception exception)
        {
            var logFile = File.Open("./Logs/log.txt", FileMode.Open);

            StringBuilder message = new StringBuilder();

            message.AppendLine("<---------------" + DateTime.Now.ToString("G") + "--------------->");
            message.AppendLine("Message: " + exception.Message);
            message.AppendLine("Stacktrace: " + exception.StackTrace);
            message.AppendLine("");

            File.AppendText(message.ToString());
        }

        public static void WriteLog(string text)
        {
            var logFile = File.Open("./Logs/log.txt", FileMode.Open);

            StringBuilder message = new StringBuilder();

            message.AppendLine("<---------------" + DateTime.Now.ToString("G") + "--------------->");
            message.AppendLine(text);
            message.AppendLine("");

            File.AppendText(message.ToString());
        }
    }
}
