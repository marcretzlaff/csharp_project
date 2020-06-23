using MyLog;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Unity;

namespace csharp_project.Speech
{
    public static class SpeechSerialization
    {
        /// <summary>
        /// Saves available commands to file
        /// </summary>
        public static void StoreGrammar(string _filepath_commands, ObservableCollection<string> choices, UnityContainer container)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<string>));

            try
            {
                using (TextWriter writer = new StreamWriter(_filepath_commands))
                {
                    serializer.Serialize(writer, choices);
                }
            }
            catch (Exception e)
            {
                container.Resolve<Log>().WriteException(e, "Trying to save XML SpeechCommands.");
            }
        }

        public static ObservableCollection<string> LoadGrammar(string _filepath_commands, UnityContainer container)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<string>));

            try
            {
                using (TextReader reader = new StreamReader(_filepath_commands))
                {
                    return serializer.Deserialize(reader) as ObservableCollection<string>;
                }
            }
            catch (Exception e)
            {
                container.Resolve<Log>().WriteException(e, "Trying to load XML SpeechCommands.");

                return new ObservableCollection<string>();
            }
        }
    }
}