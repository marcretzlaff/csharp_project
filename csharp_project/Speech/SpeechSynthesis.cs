using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Timers;
using csharp_project.Data;

namespace csharp_project.Speech
{
    public class SpeechSynthesis
    {
        //choices
        private readonly string[] _choices = new string[] { "Hello", "Cancel", "Hold", "Cancel", "Add", "Delete" };

        #region StateMachines
        enum internalState
        {
            MainCommand,
            Adding,
            DeletingID,
            DeletingName
        }
        private internalState _state { get; set; } = internalState.MainCommand;

        enum addingState
        {
            First,
            Name,
            Expires,
            ExpireDate,
            Size,
            Pieces,
            RoundUp,
            Store
        }
        private addingState _addingstate { get; set; } = addingState.First;

        enum deletingState
        {
            First,
            Name,
            RoundUp,
            Store
        }
        private deletingState _deletingState { get; set; } = deletingState.First;

        #endregion StateMachines

        #region DummyItems
        enum typeState
        {
            FoodSmall,
            FoodBig,
            DrinkSmall,
            DrinkBig
        }
        private typeState _typDummy { get; set; } = typeState.FoodSmall;
        private int _idDummy { get; set; }
        private int _mulDummy { get; set; }
        private string _nameDummy { get; set; }
        private bool _expireDummy { get; set; }
        private int _expiryTimeDummy { get; set; }
        private int _sizeDummy { get; set; }
        #endregion DummyItems

        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
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
            //_recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(_choices))));
            _recognizer.LoadGrammar(new DictationGrammar());
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
                timeout = 0;
                _synth.SpeakAsync("Back to Sleep");
                try
                {
                    _recognizer.RecognizeAsyncCancel();
                    _uirecognizer.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch { };
            }
        }

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
                catch { }
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

            //state machine
            switch(_state)
            {
                case internalState.MainCommand:
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

                        case "Add":
                            _synth.SpeakAsync("Proceed with item typ");
                            _state = internalState.Adding;
                            break;

                        case "Delete":
                            _synth.SpeakAsync("Proceed with item typ");
                            _state = internalState.DeletingID;
                            break;

                        default:
                            _synth.SpeakAsync("Sorry, didn't catch that.");
                            break;
                    }
                    break;

                case internalState.Adding:
                    addingItemSM(speech);
                    break;

                case internalState.DeletingID:
                    deletingItemIDSM(speech);
                    break;
            }
        }

        private void addingItemSM(string s)
        {
            //state machine
            switch (_addingstate)
            {
                case addingState.First:
                    if (s == "Cancel")
                    {
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    switch (s)
                    {
                        case "Food":
                            _typDummy = typeState.FoodSmall;
                            _synth.SpeakAsync("Please provide the ID of the item.");
                            _deletingState = deletingState.Name;
                            break;

                        case "Drink":
                            _typDummy = typeState.DrinkSmall;
                            _synth.SpeakAsync("Please provide the ID of the item.");
                            _deletingState = deletingState.Name;
                            break;

                        default:
                            _synth.SpeakAsync("Please provide the item typ again.");
                            break;
                    }
                    break;

                case addingState.Name:
                    if (s == "Cancel")
                    {
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                    }
                    else
                    {
                        _nameDummy = s;
                        _synth.SpeakAsync("Does it expire?");
                        _addingstate = addingState.Expires;
                    }
                    break;

                case addingState.Expires:
                    if (s == "Cancel")
                    {
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    if(s == "Yes")
                    {
                        _synth.SpeakAsync("How many days does it last?");
                        _expireDummy = true;
                        _addingstate = addingState.ExpireDate;
                    }
                    else
                    {
                        _expireDummy = false;
                        _synth.SpeakAsync("What's the size?");
                        _addingstate = addingState.Size;
                    }
                    break;

                case addingState.ExpireDate:
                    if (s == "Cancel")
                    {
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }
                    break;

                case addingState.Size:
                    if (s == "Cancel")
                    {
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    if (Int16.TryParse(s, out var result))
                    {
                        _synth.SpeakAsync("Try again or say cancel.");
                    }
                    else
                    {
                        _sizeDummy = result;
                        _synth.SpeakAsync("How many pieces?");
                        _addingstate = addingState.Pieces;
                    }
                    break;

                case addingState.Pieces:
                    if (s == "Cancel")
                    {
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }


                    if (Int16.TryParse(s, out var mul))
                    {
                        _synth.SpeakAsync("Try again or say cancel.");
                    }
                    else
                    {
                        _mulDummy = mul;
                        _synth.SpeakAsync($"Do you want to add the {_mulDummy} item with the name {_nameDummy} of the typ {_typDummy} with size {_sizeDummy}");
                        if (_expireDummy)
                            _synth.SpeakAsync($"and expires at {DateTime.Now.AddDays(_expiryTimeDummy).ToString("yyyy-MM-dd")}?");
                        _addingstate = addingState.RoundUp;
                    }
                    break;

                case addingState.RoundUp:
                    if (s == "Yes")
                    {
                        _addingstate = addingState.Store;
                    }
                    else
                    {
                        _synth.SpeakAsync("Cancelled.");
                        _addingstate = addingState.First;
                        _state = internalState.MainCommand;
                    }
                    break;

                case addingState.Store:
                    bool success = false;
                    var dbhelper = DataAccess.DataManager.getInstance();
                    Food food;
                    Drinks drink;

                    switch (_typDummy)
                    {
                        case typeState.FoodSmall:
                            food = new Food(_nameDummy, _sizeDummy);
                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Food>(food);
                            break;

                        case typeState.FoodBig:
                            food = new Food(_nameDummy, DateTime.Now, DateTime.Now.AddDays(_expiryTimeDummy), _sizeDummy);
                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Food>(food);
                            break;

                        case typeState.DrinkSmall:
                            drink = new Drinks(_nameDummy, _sizeDummy);
                            while (_mulDummy-- != 0)
                                success = dbhelper.Insert<Drinks>(drink);
                            break;

                        case typeState.DrinkBig:
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
                    _deletingState = deletingState.First;
                    _state = internalState.MainCommand;
                    break;
            }
        }

        private void deletingItemIDSM( string s)
        {
            //state machine
            switch (_deletingState)
            {
                case deletingState.First:
                    if(s == "Cancel")
                    {
                        _deletingState = deletingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                        break;
                    }

                    switch(s)
                    {
                        case "Food":
                            _typDummy = typeState.FoodSmall;
                            _synth.SpeakAsync("Please provide the ID of the item.");
                            _deletingState = deletingState.Name;
                            break;

                        case "Drink":
                            _typDummy = typeState.DrinkSmall;
                            _synth.SpeakAsync("Please provide the ID of the item.");
                            _deletingState = deletingState.Name;
                            break;

                        default:
                            _synth.SpeakAsync("Please provide the item typ again.");
                            break;
                    }
                    break;

                case deletingState.Name:
                    if (s == "Cancel")
                    {
                        _deletingState = deletingState.First;
                        _state = internalState.MainCommand;
                        _synth.SpeakAsync("Sucessfully cancelled");
                    }
                    else
                    {
                        if (Int16.TryParse(s, out var result))
                        {
                            _synth.SpeakAsync("Try again or say cancel.");
                        }
                        else
                        {
                            _idDummy = result;
                            _synth.SpeakAsync($"Do you want to delete the item with the ID {result} of the typ {_typDummy}?");
                            _deletingState = deletingState.RoundUp;
                        }
                    }

                    break;

                case deletingState.RoundUp:
                    if( s == "Yes")
                    {
                        _deletingState = deletingState.Store;
                    }
                    else
                    {
                        _synth.SpeakAsync("Cancelled.");
                        _deletingState = deletingState.First;
                        _state = internalState.MainCommand;
                    }
                    break;

                case deletingState.Store:
                    bool success = false;
                    var dbhelper = DataAccess.DataManager.getInstance();
                    if (_typDummy == typeState.FoodBig || _typDummy == typeState.FoodSmall)
                    {
                        success = dbhelper.Delete<Food>(_idDummy);
                    }
                    else
                    {
                        success = dbhelper.Delete<Drinks>(_idDummy);
                    }

                    if(success)
                    {
                        _synth.SpeakAsync("Item deleted.");
                    }
                    else
                    {
                        _synth.SpeakAsync("Unsucessful.");
                    }
                    _deletingState = deletingState.First;
                    _state = internalState.MainCommand;
                    break;
            }
        }
    }
}
