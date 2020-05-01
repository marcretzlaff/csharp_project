using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Timers;

namespace csharp_project.Speech
{
    public class SpeechSynthesis
    {
        //choices
        private readonly string[] _choices = new string[] { "Expire", "Hello", "Cancel", "Hold" };

        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer _synth = new SpeechSynthesizer();
        SpeechRecognitionEngine _uirecognizer = new SpeechRecognitionEngine();
        int timeout = 0;
        Timer _timer;

        //Singelton with Lazy
        private SpeechSynthesis() { }
        private static readonly Lazy<SpeechSynthesis> lazy = new Lazy<SpeechSynthesis>
            (() => new SpeechSynthesis());
        public static SpeechSynthesis Instance => lazy.Value;


        public void LoadDefault()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += timeOverEvent;

            _synth.Rate = -2;
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(_choices))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);


            _uirecognizer.SetInputToDefaultAudioDevice();
            _uirecognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices("Sarah"))));
            _uirecognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(uirecognizer_SpeechRecognized);
            _uirecognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void timeOverEvent(object sender, ElapsedEventArgs e)
        {
            ++timeout;
            if (timeout > 10)
            {
                _timer.Stop();
                _synth.SpeakAsync("Sleep");
                _recognizer.RecognizeAsyncCancel();
                _uirecognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void uirecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            if(speech == "Sarah")
            {
                _uirecognizer.RecognizeAsyncCancel();
                _synth.SpeakAsync("Here for you!");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                _timer.Start();
            }
        }

        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            timeout = 0;
            if(_timer.Enabled == false)
                _timer.Start();
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            switch(speech)
            {
                case "Hello": _synth.SpeakAsync("Hello, i'm a lifecycle manager.");
                    break;

                case "Sleep": _synth.SpeakAsync("Sleep Mode");
                    _recognizer.RecognizeAsyncCancel();
                    _uirecognizer.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Hold": _synth.SpeakAsync("Waiting");
                    _timer.Stop();
                    break;
            }
        }
    }
}
