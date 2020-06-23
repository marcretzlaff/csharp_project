using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using csharp_project.Data;
using System.Collections.ObjectModel;
using MyLog;
using System.Timers;
using Unity;
using csharp_project.DataAccess;
using csharp_project.Speech.States;

namespace csharp_project.Speech
{
    public partial class SpeechSynthesis
    {
        #region Private Fields

        private readonly string _filepath_commands = System.Windows.Forms.Application.CommonAppDataPath + "\\SpeechCommands.xml";

        private bool _loaded = false;

        private readonly List<string> _numbers = new List<string>();

        private readonly SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();

        private readonly SpeechSynthesizer _synth = new SpeechSynthesizer();

        private int _timeout = 0;

        private Timer _timer;

        private readonly SpeechRecognitionEngine _uirecognizer = new SpeechRecognitionEngine();

        private readonly Grammar dict = new DictationGrammar();

        private Grammar usergrammar;

        #endregion Private Fields

        #region Public Constructors

        public SpeechSynthesis(UnityContainer container) { Container = container; }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<string> Choices { get; set; } = new ObservableCollection<string>() { "Speech Recognition deactived. Activate and restart." };

        //public Container due InjectionProperty UnityContainer
        public UnityContainer Container { get; set; }

        public void StoreGrammar()
        {
            SpeechSerialization.StoreGrammar(_filepath_commands, Choices, Container);
        }

        #endregion Public Properties
        
        #region StateMachines

        private AddingState _addingstate = AddingState.First;
        private DeletingState _deletingState = DeletingState.First;
        private InternalState _state = InternalState.MainCommand;
        private TypeState _typDummy = TypeState.FoodSmall;

        #endregion StateMachines

        #region DummyItems

        private bool _expireDummy;
        private int _expiryTimeDummy;
        private int _idDummy;
        private int _mulDummy;
        private string _nameDummy;
        private int _sizeDummy;        

        #endregion DummyItems

        #region Public Methods

        /// <summary>
        /// Deactivates Speech recognition engines
        /// </summary>
        public void DeactivateSpeech()
        {
            _uirecognizer.RecognizeAsyncCancel();
            _recognizer.RecognizeAsyncCancel();

            _timer.Stop();
            _timeout = 0;
        }

        /// <summary>
        /// Loads Grammar to SpeechRecognitionEngine
        /// </summary>
        public void LoadCustomGrammar()
        {
            usergrammar = new Grammar(new GrammarBuilder(new Choices(Choices.ToArray())));
            _recognizer.LoadGrammar(usergrammar);
        }

        /// <summary>
        /// Default intizialisation from SpeechRecognitionEngines and SpeechSynthesizer
        /// If Command file doesn't exist, default command will be used and file created
        /// </summary>
        public void LoadDefault()
        {
            if (_loaded)
            {
                _uirecognizer.RecognizeAsync(RecognizeMode.Single);
            }
            else
            {
                _timer = new Timer(1000);
                _timer.Elapsed += timeOverEvent;

                _synth.Rate = -2;
                _recognizer.SetInputToDefaultAudioDevice();

                loadVoiceGrammar();

                for (int i = 1; i < 100; i++)
                {
                    _numbers.Add(i.ToString());
                }

                usergrammar = new Grammar(new GrammarBuilder(new Choices(Choices.ToArray())))
                {
                    Priority = 127
                };

                var numbergrammar = new Grammar(new GrammarBuilder(new Choices(_numbers.ToArray())))
                {
                    Priority = 126
                };

                _recognizer.LoadGrammar(usergrammar);
                _recognizer.LoadGrammar(numbergrammar);

                _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(default_SpeechRecognized);
                _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(recognizer_SpeechDetected);

                _uirecognizer.SetInputToDefaultAudioDevice();

                _uirecognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices("Sarah"))));
                _uirecognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(uirecognizer_SpeechRecognized);

                _loaded = true;
            }
        }

        /// <summary>
        /// Loads regular dictionary to recognizer
        /// </summary>
        public void LoadRegularDict()
        {
            _recognizer.LoadGrammar(dict);
        }

        /// <summary>
        /// Unloads Grammar from SpeechRecognitionEngine to change
        /// </summary>
        public void UnloadCustomGrammar()
        {
            _recognizer.UnloadGrammar(usergrammar);
        }

        /// <summary>
        /// Unloads regular dictionary to recognizer
        /// </summary>
        public void UnloadRegularDict()
        {
            _recognizer.UnloadGrammar(dict);
        }

        /// <summary>
        /// Loads words to recognize
        /// </summary>
        private void loadVoiceGrammar()
        {
            //Load saved strings from file
            if (File.Exists(_filepath_commands))
            {
                Choices = SpeechSerialization.LoadGrammar(_filepath_commands, Container);
            }
            else
            {
                string[] standard = new string[] { "Hello", "Cancel", "Hold", "Insert", "Delete", "Food", "Drink", "Yes" };
                Choices = new ObservableCollection<string>(standard.ToList());

                StoreGrammar();
            }
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Command SpeechRecognitionEngine SpeechRecognized Event Handler
        /// Main state machine for standard commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            //state machine
            switch (_state)
            {
                case InternalState.MainCommand:
                    switch (speech)
                    {
                        case "Hello":
                            _synth.SpeakAsync("Hello, i'm a lifecycle manager.");
                            break;

                        case "Sleep":
                            _synth.SpeakAsync("Sleep Mode");
                            _recognizer.RecognizeAsyncCancel();
                            _uirecognizer.RecognizeAsync(RecognizeMode.Multiple);
                            break;

                        case "Hold":
                            _synth.SpeakAsync("Waiting");
                            _timer.Stop();
                            break;

                        case "Insert":
                            _synth.SpeakAsync("Proceed with item typ");
                            _state = InternalState.Adding;
                            break;

                        case "Delete":
                            _synth.SpeakAsync("Proceed with item typ");
                            _state = InternalState.DeletingID;
                            break;

                        default:
                            _synth.SpeakAsync("Sorry, didn't catch that.");
                            break;
                    }
                    break;

                case InternalState.Adding:
                    insertItemSM(speech);
                    break;

                case InternalState.DeletingID:
                    deletingItemIDSM(speech);
                    break;
            }
        }

        /// <summary>
        /// Called if Command SpeechRecognitionEngine SpeechDetected State Delete
        /// </summary>
        /// <param name="s"></param>
        private void deletingItemIDSM(string s)
        {
            //state machine
            switch (_deletingState)
            {
                case DeletingState.First:
                    if (s == "Cancel")
                    {
                        _deletingState = DeletingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    switch (s)
                    {
                        case "Food":
                            _typDummy = TypeState.FoodSmall;
                            _synth.SpeakAsync("Please provide the ID of the item.");
                            _deletingState = DeletingState.Name;
                            break;

                        case "Drink":
                            _typDummy = TypeState.DrinkSmall;
                            _synth.SpeakAsync("Please provide the ID of the item.");
                            _deletingState = DeletingState.Name;
                            break;

                        default:
                            _synth.SpeakAsync("Please provide the item typ again.");
                            break;
                    }
                    break;

                case DeletingState.Name:
                    if (s == "Cancel")
                    {
                        _deletingState = DeletingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                    }
                    else
                    {
                        if (!short.TryParse(s, out var result))
                        {
                            _synth.SpeakAsync("Try again or say cancel.");
                        }
                        else
                        {
                            _idDummy = result;
                            _synth.SpeakAsync($"Do you want to delete the item with the ID {result} of the typ {_typDummy}?");
                            _deletingState = DeletingState.RoundUp;
                        }
                    }

                    break;

                case DeletingState.RoundUp:
                    if (s == "Yes")
                    {
                        _deletingState = DeletingState.Store;
                    }
                    else
                    {
                        _synth.SpeakAsync("Cancelled.");
                        _deletingState = DeletingState.First;
                        _state = InternalState.MainCommand;
                    }
                    break;

                case DeletingState.Store:
                    bool success;
                    var dbhelper = Container.Resolve<IDatabase>();

                    if (_typDummy == TypeState.FoodBig || _typDummy == TypeState.FoodSmall)
                    {
                        success = dbhelper.Delete<Food>(_idDummy);
                    }
                    else
                    {
                        success = dbhelper.Delete<Drinks>(_idDummy);
                    }

                    if (success)
                    {
                        _synth.SpeakAsync("Item deleted.");
                    }
                    else
                    {
                        _synth.SpeakAsync("Unsucessful.");
                    }

                    _deletingState = DeletingState.First;
                    _state = InternalState.MainCommand;
                    break;
            }
        }

        /// <summary>
        /// Called if Command SpeechRecognitionEngine SpeechDetected State Insert
        /// </summary>
        /// <param name="s"></param>
        private void insertItemSM(string s)
        {
            //state machine
            switch (_addingstate)
            {
                case AddingState.First:
                    if (s == "Cancel")
                    {
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    switch (s)
                    {
                        case "Food":
                            _typDummy = TypeState.FoodSmall;
                            _synth.SpeakAsync("Please provide the name of the item.");
                            _addingstate = AddingState.Name;
                            break;

                        case "Drink":
                            _typDummy = TypeState.DrinkSmall;
                            _synth.SpeakAsync("Please provide the name of the item.");
                            _addingstate = AddingState.Name;
                            break;

                        default:
                            _synth.SpeakAsync("Please provide the item typ again.");
                            break;
                    }
                    break;

                case AddingState.Name:
                    if (s == "Cancel")
                    {
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                    }
                    else if (s == "Yes")
                    {
                        _synth.SpeakAsync("Does it expire? Say yes if so.");
                        _addingstate = AddingState.Expires;
                    }
                    else
                    {
                        _nameDummy = s;
                        _synth.SpeakAsync($"Is {_nameDummy} correct? Otherwise repeat the name!");
                    }
                    break;

                case AddingState.Expires:
                    if (s == "Cancel")
                    {
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    if (s == "Yes")
                    {
                        _synth.SpeakAsync("How many days does it last?");
                        _expireDummy = true;
                        if (_typDummy == TypeState.FoodSmall)
                            _typDummy = TypeState.FoodBig;
                        else
                            _typDummy = TypeState.DrinkBig;
                        _addingstate = AddingState.ExpireDate;
                    }
                    else
                    {
                        _expireDummy = false;
                        _synth.SpeakAsync("What's the size?");
                        _addingstate = AddingState.Size;
                    }
                    break;

                case AddingState.ExpireDate:
                    if (s == "Cancel")
                    {
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }
                    if (!short.TryParse(s, out var result))
                    {
                        _synth.SpeakAsync("Try again or say cancel.");
                    }
                    else
                    {
                        _expiryTimeDummy = result;
                        _synth.SpeakAsync("What's the size?");
                        _addingstate = AddingState.Size;
                    }
                    break;

                case AddingState.Size:
                    if (s == "Cancel")
                    {
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    if (!short.TryParse(s, out var size))
                    {
                        _synth.SpeakAsync("Try again or say cancel.");
                    }
                    else
                    {
                        _sizeDummy = size;
                        _synth.SpeakAsync("How many pieces?");
                        _addingstate = AddingState.Pieces;
                    }
                    break;

                case AddingState.Pieces:
                    if (s == "Cancel")
                    {
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    if (!short.TryParse(s, out var mul))
                    {
                        _synth.SpeakAsync("Try again or say cancel.");
                    }
                    else
                    {
                        _mulDummy = mul;
                        _synth.SpeakAsync($"Do you want to add the {_mulDummy} item with the name {_nameDummy} of the typ {_typDummy} with size {_sizeDummy}");

                        if (_expireDummy)
                            _synth.SpeakAsync($"and expires at {DateTime.Now.AddDays(_expiryTimeDummy):yyyy-MM-dd}?");

                        _addingstate = AddingState.RoundUp;
                    }
                    break;

                case AddingState.RoundUp:
                    if (s == "Yes")
                    {
                        _addingstate = AddingState.Store;
                    }
                    else
                    {
                        _synth.SpeakAsync("Cancelled.");
                        _addingstate = AddingState.First;
                        _state = InternalState.MainCommand;
                    }
                    break;

                case AddingState.Store:
                    bool success = false;
                    var dbhelper = Container.Resolve<IDatabase>();
                    Food food;
                    Drinks drink;

                    switch (_typDummy)
                    {
                        case TypeState.FoodSmall:
                            food = new Food(_nameDummy, _sizeDummy);

                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Food>(food);
                            break;

                        case TypeState.FoodBig:
                            food = new Food(_nameDummy, DateTime.Now, DateTime.Now.AddDays(_expiryTimeDummy), _sizeDummy);

                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Food>(food);
                            break;

                        case TypeState.DrinkSmall:
                            drink = new Drinks(_nameDummy, _sizeDummy);

                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Drinks>(drink);
                            break;

                        case TypeState.DrinkBig:
                            drink = new Drinks(_nameDummy, DateTime.Now, DateTime.Now.AddDays(_expiryTimeDummy), _sizeDummy);

                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Drinks>(drink);
                            break;
                    }

                    if (success)
                    {
                        _synth.SpeakAsync("Item added.");
                    }
                    else
                    {
                        _synth.SpeakAsync("Unsucessful.");
                    }

                    _addingstate = AddingState.First;
                    _state = InternalState.MainCommand;
                    break;
            }
        }

        /// <summary>
        /// Command SpeechRecognitionEngine SpeechDetected Event Handler
        /// Timer reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            _timeout = 0;

            if (_timer.Enabled == false)
                _timer.Start();
        }

        /// <summary>
        /// Timer Elapsed Event Handler
        /// After 10s silence the command SpeechRecognitionEngine shuts down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeOverEvent(object sender, ElapsedEventArgs e)
        {
            ++_timeout;

            if (_timeout > 10)
            {
                _timer.Stop();
                _timeout = 0;
                _synth.SpeakAsync("Back to Sleep");
                _deletingState = DeletingState.First;
                _addingstate = AddingState.First;
                _state = InternalState.MainCommand;

                try
                {
                    _recognizer.RecognizeAsyncCancel();
                    _uirecognizer.RecognizeAsync(RecognizeMode.Single);
                }
                catch (Exception ex)
                {
                    Container.Resolve<Log>().WriteException(ex, "Exception at start of new recognize");
                }
            }
        }

        /// <summary>
        /// Startup SpeechRecognitionEngine Speechrecognized Event Handler
        /// With Keyword "Sarah" command SpeechRecognitionEngine starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uirecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            if(speech == "Sarah")
            {
                _uirecognizer.RecognizeAsyncCancel();
                _synth.SpeakAsync("Here for you!");

                try
                {
                    _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception ex)
                {
                    Container.Resolve<Log>().WriteException(ex, "Exception at start of new recognize");
                }

                _timer.Start();
            }
        }

        #endregion Private Methods
    }
}