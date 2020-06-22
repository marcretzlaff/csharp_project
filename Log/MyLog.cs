using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MyLog
{
    public class Log
    {
        private readonly string path = Application.CommonAppDataPath + "/Logs/log.txt";
        private readonly string directorypath = Application.CommonAppDataPath + "/Logs";
        public Log()
        {
            CreateLogFile();
        }

        /// <summary>
        /// Creates directory and log file.
        /// </summary>
        public void CreateLogFile()
        {
            Directory.CreateDirectory(directorypath);
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        /// <summary>
        /// Deletes log file.
        /// </summary>
        public void Delete()
        {
            try
            {
                //overwrites written content
                File.CreateText(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception happened!");
            }
        }

        /// <summary>
        /// not for internal exception use
        /// </summary>
        /// <param name="exception"></param>
        public void WriteException(Exception exception, string s)
        {
            using (StreamWriter fs = new StreamWriter(path, true))
            {
                StringBuilder message = new StringBuilder();

                message.AppendLine("<---------------" + DateTime.Now.ToString("G") + "--------------->");
                message.AppendLine($"While: {s}");
                message.AppendLine("Message: " + exception.Message);
                message.AppendLine("Stacktrace: " + exception.StackTrace);
                message.AppendLine("");

                fs.Write(message.ToString());
            }
        }

        /// <summary>
        /// Writes param string to log file.
        /// </summary>
        /// <param name="text"></param>
        public void WriteLog(string text)
        {
            try
            {
                using (StreamWriter fs = new StreamWriter(path, true))
                {
                    StringBuilder message = new StringBuilder();

                    message.AppendLine("<---------------" + DateTime.Now.ToString("G") + "--------------->");
                    message.AppendLine(text);
                    message.AppendLine("");

                    fs.Write(message.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception happened!");
            }
        }

        /// <summary>
        /// Opens log file in editor.
        /// </summary>
        public void OpenLog()
        {
            System.Diagnostics.Process.Start("notepad.exe", path);
        }
    }
}
