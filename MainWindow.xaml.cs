using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Speech.Recognition;
using System.Diagnostics;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Threading;
using WindowsInput;
using iFly;
using System.Web.Script.Serialization;
using Microsoft.Kinect;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;
using SKYPE4COMLib;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SocialRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // public SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        // SpeechSynthesizer srespeechMain = new SpeechSynthesizer();

        //Kinect Part Start

        #region defineVar
        /// <summary>
        /// Number of samples captured from Kinect audio stream each millisecond.
        /// </summary>
        private const int SamplesPerMillisecond = 16;

        /// <summary>
        /// Number of bytes in each Kinect audio stream sample (32-bit IEEE float).
        /// </summary>
        private const int BytesPerSample = sizeof(float);

        /// <summary>
        /// Number of audio samples represented by each column of pixels in wave bitmap.
        /// </summary>
        private const int SamplesPerColumn = 40;

        /// <summary>
        /// Minimum energy of audio to display (a negative number in dB value, where 0 dB is full scale)
        /// </summary>
        private const int MinEnergy = -90;

        /// <summary>
        /// Width of bitmap that stores audio stream energy data ready for visualization.
        /// </summary>
        private const int EnergyBitmapWidth = 780;

        /// <summary>
        /// Height of bitmap that stores audio stream energy data ready for visualization.
        /// </summary>
        private const int EnergyBitmapHeight = 195;

        /// <summary>
        /// Rectangle representing the entire energy bitmap area. Used when drawing background
        /// for energy visualization.
        /// </summary>
        private readonly Int32Rect fullEnergyRect = new Int32Rect(0, 0, EnergyBitmapWidth, EnergyBitmapHeight);

        /// <summary>
        /// Array of background-color pixels corresponding to an area equal to the size of whole energy bitmap.
        /// </summary>
        private readonly byte[] backgroundPixels = new byte[EnergyBitmapWidth * EnergyBitmapHeight];

        /// <summary>
        /// Will be allocated a buffer to hold a single sub frame of audio data read from audio stream.
        /// </summary>
        private readonly byte[] audioBuffer = null;

        /// <summary>
        /// Buffer used to store audio stream energy data as we read audio.
        /// We store 25% more energy values than we strictly need for visualization to allow for a smoother
        /// stream animation effect, since rendering happens on a different schedule with respect to audio
        /// capture.
        /// </summary>
        private readonly float[] energy = new float[(uint)(EnergyBitmapWidth * 1.25)];

        /// <summary>
        /// Object for locking energy buffer to synchronize threads.
        /// </summary>
        private readonly object energyLock = new object();

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Reader for audio frames
        /// </summary>
        private AudioBeamFrameReader reader = null;

        /// <summary>
        /// Last observed audio beam angle in radians, in the range [-pi/2, +pi/2]
        /// </summary>
        private float beamAngle = 0;

        /// <summary>
        /// Last observed audio beam angle confidence, in the range [0, 1]
        /// </summary>
        public static float beamAngleInDeg = 0;
        public static float beamAngleConfidence = 0;

        /// <summary>
        /// Sum of squares of audio samples being accumulated to compute the next energy value.
        /// </summary>
        private float accumulatedSquareSum;

        /// <summary>
        /// Number of audio samples accumulated so far to compute the next energy value.
        /// </summary>
        private int accumulatedSampleCount;

        /// <summary>
        /// Index of next element available in audio energy buffer.
        /// </summary>
        private int energyIndex;

        /// <summary>
        /// Number of newly calculated audio stream energy values that have not yet been
        /// displayed.
        /// </summary>
        private int newEnergyAvailable;

        //ProjectOxford Part Start
        private ColorFrameReader colorFrameReader = null;
        private WriteableBitmap colorBitmap = null;
        private string statusText = null;
        int PictureMark = 0;
        private DispatcherTimer EmotionTimer = new DispatcherTimer();//The timer used in face tracking
        public static DispatcherTimer ReadNewsTimer = new DispatcherTimer();//The timer used in reading news function
        public static DispatcherTimer GrammarTimer = new DispatcherTimer();//The timer used in reading news function
        public static DispatcherTimer WeatherIconTimer = new DispatcherTimer();//The timer used in weather icon
        //ProjectOxford Part End

        #endregion defineVar

        //Kinect Part End

        public static bool DemoMode = false;

        string ResultName = "";
        string RuleName = "";
        string Country = "";
        string Day = "";
        string Duration = "";
        string question = "";
        string subday = "";
        int subday_num = 0;
        string subhour = "";
        string subhour_12 = "";
        string subminute = "";
        string reminder_task = "";
        string text_test;
        string president = "";
        string ContactName;
        string homecountry = "singapore";

        string alarm;
        DateTime alarmTime;
        Thread alarmThread;
        System.DateTime EventTime;

        BitmapImage b = new BitmapImage();





        string fileLoc = @"C:\test\SMS.txt";

        string device;

        string country_number = "";
        string phone_number = "";
        string country_phone_number = "";
        int i = 1;
        int CurrentGenreIndex;
        string current_MusicOrSongGenre = "";
        string number;
        string SMSContent;
        public static string language = "English";
        public bool EnglishNewsMark = true;
        bool pass = true;
        bool OneTimeMark = false;

        string remind_time = null;
        int substringminute = 60;
        //public SocialRobot.Application.WeatherApp WeatherFunction = new SocialRobot.Application.WeatherApp();
        public Application.CountdownApp CountdownFunction = new Application.CountdownApp();
        public SocialRobot.Application.WikiApp WifiFunction = new SocialRobot.Application.WikiApp();
        public Application.SetReminder SetReminderFunction = new Application.SetReminder();
        public static SocialRobot.Function.Speech_Rcognition_Grammar SRG = new SocialRobot.Function.Speech_Rcognition_Grammar();
        public Application.ReadNewsApp ReadNewsFunction = new Application.ReadNewsApp();
        public Application.DateTimeApp DateTimeFunction = new Application.DateTimeApp();
        public Application.TellJokesApp TellJokesFunction = new Application.TellJokesApp();
        public Application.SkypeApp.SkypeApp Skype = new Application.SkypeApp.SkypeApp();
        Application.musicplayer mp3 = new Application.musicplayer();
        Application.health Health = new Application.health();
        //public Application.SkypeApp.Skype_Interface SkypeInterface = new Application.SkypeApp.Skype_Interface();
        public Application.iRKit.iRKitApp iRKit = new Application.iRKit.iRKitApp();
        public Application.Light.LightControl LightCon = new Application.Light.LightControl();

        public Application.Spotify SpotifyFunction = new Application.Spotify();
        public SocialRobot.Setting Setting_Windows = new Setting();
        public SocialRobot.HealthPage HealthWindows = new HealthPage();
        public SocialRobot.keypad dialpad = new keypad();


        // O_NLP.RootObject is a class that contains the data interpreted from wit.ai
        SocialRobot.Wit.Objects.O_NLP.RootObject oNLP = new SocialRobot.Wit.Objects.O_NLP.RootObject();

        // NLP_Processing is the code that processes the response from wit.ai
        SocialRobot.Wit.Vitals.NLP.NLP_Processing vitNLP = new SocialRobot.Wit.Vitals.NLP.NLP_Processing();

        // Winmm.dll is used for recording speech
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);


        // Variables used for speech recording
        private bool recording = false;
        private string speechfilename = "";
        // Set a timer to make sure recording doesn't exceed 10 seconds
        private System.Timers.Timer speechTimer = new System.Timers.Timer();
        private System.Timers.Timer SMSTimer = new System.Timers.Timer();
        private System.Timers.Timer PhoneCallTimer = new System.Timers.Timer();

        public static bool FacialExpressionMark = true;

        string XunFei_result;

        Function.Motor Motor = new Function.Motor();
        Function.Record Record = new Function.Record();

        public static int HelloCount = 0;

        public bool VideoCallFlag = false;
        //ProjectOxford Part Start

        public ImageSource ImageSource
        {
            get
            {
                return this.colorBitmap;
            }
        }

        internal class EmotionResultDisplay
        {
            public string EmotionString
            {
                get;
                set;
            }
            public float Score
            {
                get;
                set;
            }
        }
        //ProjectOxford Part End
        public MainWindow()
        {
            SetReminderFunction.GoogleCalendar();

            //SpeechRcognitionSystem.SRGS_GrammarModels();
            InitializeComponent();
            // VisionDisplay visiondisplay = new VisionDisplay();

            //Kinect Part Start

            // Only one Kinect Sensor is supported
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // Open the sensor
                this.kinectSensor.Open();

                // Get its audio source
                AudioSource audioSource = this.kinectSensor.AudioSource;

                // Allocate 1024 bytes to hold a single audio sub frame. Duration sub frame 
                // is 16 msec, the sample rate is 16khz, which means 256 samples per sub frame. 
                // With 4 bytes per sample, that gives us 1024 bytes.
                this.audioBuffer = new byte[audioSource.SubFrameLengthInBytes];

                // Open the reader for the audio frames
                this.reader = audioSource.OpenReader();
            }

            //ProjectOxford Part Start
            // get the kinectSensor object
            this.kinectSensor = KinectSensor.GetDefault();

            // open the reader for the color frames
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // wire handler for frame arrival
            this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using Bgra format
            FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

            // create the bitmap to display
            this.colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            // open the sensor
            this.kinectSensor.Open();

            // use the window object as the view model in this simple example
            this.DataContext = this;
            EmotionTimer.Tick += new EventHandler(ProjectOxford);
            EmotionTimer.Interval = new TimeSpan(0, 0, 9);
            EmotionTimer.Start();
            ReadNewsTimer.Tick += new EventHandler(ReadNewsTimer_TimeUp);
            ReadNewsTimer.Interval = new TimeSpan(0, 0, 30);
            GrammarTimer.Tick += new EventHandler(GrammarTimer_TimeUp);
            GrammarTimer.Interval = new TimeSpan(0, 0, 10);
            WeatherIconTimer.Tick += new EventHandler(WeatherIconTimer_TimeUp);
            WeatherIconTimer.Interval = new TimeSpan(0, 0, 10);
            //ProjectOxford Part End

            //Kinect Part End

            try
            {
                Function.FaceLED.Instance.Normal();
            }
            catch
            {

            }
            try
            {
                if (!DemoMode)
                {
                    Function.Vision.Instance.FaceTrackingTimerInitialize();
                }
            }
            catch
            {

            }

            Motor.MotorInitialize();

            SRG.SpeechRecognized();
            // SRG.sre.SpeechDetected
            SRG.sre.SpeechRecognized += sre_SpeechRecognized;
            SRG.srecn.SpeechRecognized += sre_SpeechRecognized;
            SRG.srejp.SpeechRecognized += sre_SpeechRecognized;
            SRG.srecan.SpeechRecognized += sre_SpeechRecognized;
            //Audio Level Detected
            SRG.sre.AudioLevelUpdated += sre_AudioLevelUpdated;         // English
            SRG.srecn.AudioLevelUpdated += sre_AudioLevelUpdated;         // Chinese
            SRG.srejp.AudioLevelUpdated += sre_AudioLevelUpdated;         // Japanese
            SRG.srecan.AudioLevelUpdated += sre_AudioLevelUpdated;         // Cantnonse

            // SRG.srecnspeech.GetInstalledVoices(new System.Globalization.CultureInfo("zh-CN"));
            try
            {
                SRG.srespeech.SelectVoice("IVONA 2 Amy");
                //SRG.srespeech.SelectVoice("Microsoft Zira Desktop");
                SRG.srespeech.SetOutputToDefaultAudioDevice();

                SRG.srecnspeech.SelectVoice("Microsoft Huihui Desktop");
                SRG.srecnspeech.SetOutputToDefaultAudioDevice();

                SRG.srejpspeech.SelectVoice("VW Misaki");
                //SRG.srejpspeech.SelectVoice("Microsoft Haruka Desktop");
                SRG.srejpspeech.SetOutputToDefaultAudioDevice();

                SRG.srecanspeech.SelectVoice("Microsoft Tracy Desktop");
                SRG.srecanspeech.SetOutputToDefaultAudioDevice();
            }
            catch
            {
                SRG.srespeech.Speak("Please install Chinese and Japanese language pack!");
                Environment.Exit(0);
            }


            SRG.srecnspeech.SpeakProgress += SRG.srecnspeech_SpeakProgress;
            SRG.srespeech.SpeakProgress += SRG.srespeech_SpeakProgress;
            SRG.srejpspeech.SpeakProgress += SRG.srejpspeech_SpeakProgress;
            SRG.srecanspeech.SpeakProgress += SRG.srecanspeech_SpeakProgress;

            DateTimeFunction.Speak.srespeech.SpeakProgress += SRG.srespeech_SpeakProgress;
            DateTimeFunction.Speak.srecnspeech.SpeakProgress += SRG.srecnspeech_SpeakProgress;
            DateTimeFunction.Speak.srejpspeech.SpeakProgress += SRG.srejpspeech_SpeakProgress;
            DateTimeFunction.Speak.srecanspeech.SpeakProgress += SRG.srecanspeech_SpeakProgress;

            TellJokesFunction.Speak.srespeech.SpeakProgress += SRG.srespeech_SpeakProgress;

            CountdownFunction.Speak.srespeech.SpeakProgress += SRG.srespeech_SpeakProgress;
            ReadNewsFunction.SRG.srespeech.SpeakProgress += SRG.srespeech_SpeakProgress;

            try
            {
                if (!DemoMode)
                {
                    Function.Vision.Instance.SRG.srespeech.SpeakProgress += SRG.srespeech_SpeakProgress;
                }
            }
            catch
            {

            }

            SRG.srecnspeech.SpeakCompleted += SRG.srecnspeech_SpeakCompleted;
            SRG.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;
            SRG.srejpspeech.SpeakCompleted += SRG.srejpspeech_SpeakCompleted;
            SRG.srecanspeech.SpeakCompleted += SRG.srecanspeech_SpeakCompleted;

            DateTimeFunction.Speak.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;
            DateTimeFunction.Speak.srecnspeech.SpeakCompleted += SRG.srecnspeech_SpeakCompleted;
            DateTimeFunction.Speak.srejpspeech.SpeakCompleted += SRG.srejpspeech_SpeakCompleted;
            DateTimeFunction.Speak.srecanspeech.SpeakCompleted += SRG.srecanspeech_SpeakCompleted;



            TellJokesFunction.Speak.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;
            // WeatherFunction.Speak.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;
            CountdownFunction.Speak.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;
            ReadNewsFunction.SRG.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;

            try
            {
                if (!DemoMode)
                {
                    Function.Vision.Instance.SRG.srespeech.SpeakCompleted += SRG.srespeech_SpeakCompleted;
                }
            }
            catch
            {

            }


            speechTimer = new System.Timers.Timer();
            speechTimer.Elapsed += new ElapsedEventHandler(OnTimedSpeechEvent);
            speechTimer.Interval = 4000; //6 seconds

            SMSTimer = new System.Timers.Timer();
            SMSTimer.Elapsed += new ElapsedEventHandler(OnTimeSMSEvent);
            SMSTimer.Interval = 10000;

            //!!!!!Xunfei = new Wave();
            //!!!!!Xunfei.ErrorEvent += new ErrorEventHandle(Xunfei_ErrorEvent);
            //!!!!!Xunfei.SavedFile = AppDomain.CurrentDomain.BaseDirectory + "aaa.wav";
            //!!!!!Xunfei.RecordQuality = Quality.Height;

            Language_text.Text = language;
            //Record_State.Text = recording.ToString();
            //WitKeyIn.Focus();
            //bv  PhoneCallTimer.Enabled = true;

            try
            {
                loadxml();
                setalarm();
            }

            catch
            {
            }
            //SRG.srespeech.SpeakAsync("Hi, I'm Ruth.   Your personal companion.");!!
        }

        //ProjectOxford Part Start
        public event PropertyChangedEventHandler PropertyChanged;

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.colorBitmap.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                        }

                        this.colorBitmap.Unlock();
                    }
                }
            }
        }
        //ProjectOxford Part End

        private void sre_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            SRG.sre.AudioLevelUpdated -= sre_AudioLevelUpdated;

            Volume.Value = e.AudioLevel * 5;
            Setting_Windows.Audio_Level.Value = e.AudioLevel * 15;
            Setting_Windows.tb_audiolevel.Text = e.AudioLevel.ToString();
            SRG.sre.AudioLevelUpdated += sre_AudioLevelUpdated;
        }

        //private void tbYou_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter && WitKeyIn.Text.Length > 0)
        //    {
        //        WitKeyIn.IsEnabled = false;
        //        StartProcessing(WitKeyIn.Text, 0);
        //        WitKeyIn.Text = "";
        //        WitFeedBack.Text = "Hold on..";
        //        WitKeyIn.Focus();
        //        SRG.LayerGrammarLoadAndUnload(RuleName, ResultName);
        //    }
        //}

        //void srecn_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        // {
        //     SRG.sre.SpeechRecognized -= sre_SpeechRecognized;
        //     tb_confidence.Text = "Confidence: " + e.Result.Confidence;
        //     RuleName = e.Result.Grammar.RuleName;
        //     ResultName = e.Result.Text;

        //     if (e.Result.Confidence > 0.9)
        //     {
        //         tb_result_text.Text = tb_result_text.Text + "\n\n" + e.Result.Text + "\n" + e.Result.Grammar.RuleName + "\n";
        //         tb_result_text.ScrollToEnd();
        //         SRG.LayerGrammarLoadAndUnload(RuleName, ResultName);
        //         ThinkingProcess(RuleName, ResultName);
        //     }
        // }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SRG.sre.SpeechRecognized -= sre_SpeechRecognized;
            Setting_Windows.tb_confidence.Text = "Confidence: " + e.Result.Confidence;
            // tb_confidence.Text = "Confidence: " + e.Result.Confidence;
            RuleName = e.Result.Grammar.RuleName;
            ResultName = e.Result.Text;
            if (RuleName == "SGM_FUNC_AskTodayCustomWeather" || RuleName == "SGM_FUNC_AskTomorrowCustomWeather" || RuleName == "SGM_FUNC_Countdown" || RuleName == "SGM_DIAL_AskCountryPresidentOrPrimeMinister" || RuleName == "SGM_FUNC_COMPLEX_SetReminder" || RuleName == "SGM_FUNC_COMPLEX_天気" || RuleName == "SGM_DIAL_AskRobotNameAns")
            {
                RecognizedAudio audio = e.Result.Audio;
                TimeSpan start = new TimeSpan(0);
                TimeSpan duration = audio.Duration - start;
                // Add code to verify and persist the audio.

                string path = @"C:\Users\Ruth\Desktop\imagine 1 - Demo - BU\Wav\nameAudio.wav";
                using (Stream outputStream = new FileStream(path, FileMode.Create))
                {
                    RecognizedAudio nameAudio = audio.GetRange(start, duration);
                    nameAudio.WriteToWaveStream(outputStream);
                    outputStream.Close();
                    Setting_Windows.WitFeedBack.Text = "Hold on please";
                    //SRG.srespeech.SpeakAsync("Hold on please");

                    StartProcessing(path, 1);
                }
            }
            if (e.Result.Confidence > 0.85)
            {
                tb_result_text.Text = e.Result.Text + "\n";
                Setting_Windows.Setting_Result.Text = Setting_Windows.Setting_Result.Text + "\n\n" + e.Result.Text + "\n" + e.Result.Grammar.RuleName + "\n";
                SRG.LayerGrammarLoadAndUnload(RuleName, ResultName);
                ThinkingProcess(RuleName, ResultName);
            }


            SRG.sre.SpeechRecognized += sre_SpeechRecognized;
        }



        public async void StartProcessing(string text, int type)
        {
            try
            {
                string modtext = SocialRobot.Wit.Vitals.NLP.Pre_NLP_Processing.preprocessText(text);

                string nlp_text = "";

                if (type == 0)
                {
                    nlp_text = await vitNLP.ProcessWrittenText(modtext);
                }
                else
                {
                    nlp_text = await vitNLP.ProcessSpokenText(text);
                }

                // If the audio file doesn't contain anything, or wit.ai doesn't understand it, a code 400 will be returned
                if (nlp_text.Contains("The remote server returned an error: (400) Bad Request"))
                {
                    Setting_Windows.WitFeedBack.Text = "Sorry, didn't get that. Could you please repeat yourself?";
                    // WitKeyIn.IsEnabled = true;
                    Setting_Windows.WitRaw.Text = nlp_text;
                    return;
                }

                Setting_Windows.WitRaw.Text = nlp_text;

                oNLP = SocialRobot.Wit.Vitals.NLP.Post_NLP_Processing.ParseData(nlp_text);

                // This codeblock dynamically casts the intent to the corresponding class
                // Check README.txt in Vitals.Brain
                Assembly objAssembly;
                objAssembly = Assembly.GetExecutingAssembly();

                Type classType = objAssembly.GetType("SocialRobot.Wit.Vitals.Brain." + oNLP.outcomes.intent);

                //object obj = Activator.CreateInstance(classType);

                // MethodInfo mi = classType.GetMethod("makeSentence");

                object[] parameters = new object[1];
                parameters[0] = oNLP;

                //   mi = classType.GetMethod("makeSentence");
                //    string sentence = "";
                //   sentence = (string)mi.Invoke(obj, parameters);


                if (RuleName == "SGM_FUNC_AskTodayCustomWeather" || RuleName == "SGM_FUNC_AskTomorrowCustomWeather" || RuleName == "SGM_FUNC_天気")
                {
                    Day = oNLP.outcomes.entities.day[0].value;
                    Country = oNLP.outcomes.entities.location[0].value;
                }

                if (RuleName == "SGM_FUNC_Countdown")
                {
                    Duration = oNLP.outcomes.entities.duration[0].normalized[0].value;
                }

                if (RuleName == "SGM_DIAL_AskCountryPresidentOrPrimeMinister")
                {
                    question = "who is the " + oNLP.outcomes.entities.role[0].value + " of " + oNLP.outcomes.entities.location[0].value;
                }

                if (RuleName == "SGM_FUNC_COMPLEX_SetReminder" || RuleName == "")
                {
                    subday = oNLP.outcomes.entities.datetime[0].value.Day.ToString();
                    if (subday == DateTime.Now.Date.Day.ToString())
                    {
                        subday_num = DateTime.Now.Date.Day;
                        subday = "today";
                    }
                    else if (subday != DateTime.Now.Date.Day.ToString())
                    {
                        subday = "tomorrow";
                        subday_num = DateTime.Now.Date.Day + 1;
                    }
                    subhour = oNLP.outcomes.entities.datetime[0].value.Hour.ToString();
                    subminute = oNLP.outcomes.entities.datetime[0].value.Minute.ToString();
                    reminder_task = oNLP.outcomes.entities.reminder[0].value;
                    remind_time = oNLP.outcomes.entities.datetime[0].value.ToString();

                }
                if (RuleName == "SGM_DIAL_AskRobotNameAns")
                {
                    ContactName = oNLP.outcomes.entities.contact[0].value;
                    Function.FaceLED.Instance.Happy();
                    Thread.Sleep(800);
                    SRG.srespeech.SpeakAsync("Hello " + ContactName);
                }

                // Show what was deducted from the sentence
                //tbI.Text = sentence;           
                //  WitKeyIn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Setting_Windows.WitFeedBack.Text = "Sorry, didn't get that. Could you please repeat yourself?";
                //   WitKeyIn.IsEnabled = true;
                //   WitFeedBack.Text = "Sorry, no idea what's what. Try again later please!" + Environment.NewLine + Environment.NewLine + "I bumped onto this error: " + ex.Message;
            }

            if (RuleName == "SGM_FUNC_AskTodayCustomWeather")
            {
                // if (Country != "")
                SRG.sre.RecognizeAsyncCancel();
                getweather(Country, Day);
                // else
                //UI.Text = Country;
                //UI.Text = Day;
                // SRG.srespeech.SpeakAsync("Please repeat");

            }
            if (RuleName == "SGM_FUNC_天気")
            {
                //getweather2(Country, Day);
            }
            if (RuleName == "SGM_FUNC_AskTomorrowCustomWeather")
            {
                SRG.sre.RecognizeAsyncCancel();
                getweather(Country, Day);
            }
            if (RuleName == "SGM_FUNC_Countdown")
            {
                CountdownFunction.conversion = CountdownFunction.TimeConversion(Duration);
                SRG.sre.RecognizeAsyncCancel();
                SRG.srespeech.Speak("Do you want to countdown " + CountdownFunction.conversion);
                SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
            }

            if (RuleName == "SGM_DIAL_AskCountryPresidentOrPrimeMinister")
            {
                WifiFunction.AskPresidentOrPrimeMinister_Async(question);
                //president = WifiFunction.getdata();
                //SRG.srespeech.SpeakAsync(president);
            }

            if (RuleName == "SGM_FUNC_COMPLEX_SetReminder" || RuleName == "")
            {
                try
                {
                    if (Convert.ToInt32(subhour) > 12)
                    {
                        int temp_12 = 0;
                        temp_12 = Convert.ToInt32(subhour) - 12;
                        subhour_12 = temp_12.ToString();
                    }
                    else subhour_12 = subhour;
                    if (subminute == "0")
                    {
                        SRG.srespeech.SpeakAsync("Set reminder for  " + reminder_task + " at " + subhour_12 + "o'clock ," + " ," + subday + " ?");
                    }
                    else if (subminute != "0")
                    {
                        SRG.srespeech.SpeakAsync("Set reminder for  " + reminder_task + " at " + subhour_12 + " ," + subminute + " ," + subday + " ?");
                    }
                }
                catch
                {
                    SRG.srespeech.SpeakAsync("Sorry I don't understand, can you repeat again?");
                }
            }
        }


        public void ThinkingProcess(string RuleName, string ResultName)
        {
            string 曜日;
            Random RanNum = new Random();
            int RandomNumber = RanNum.Next(1, 101);
            if (Function.Vision.WakeUp)
            {
                switch (RuleName)
                {

                    case "SGM_DIAL_NiceToMeetYou":
                        //robotEmoton_show("happiness");
                        // flag_speak_completed = false;
                        UI.Text = "Nice to meet you too!";
                        try
                        {
                            Function.FaceLED.Instance.Happy();
                        }
                        catch { }
                        Thread.Sleep(500);
                        SRG.srespeech.SpeakAsync("Nice to meet you too!");
                        break;

                    case "SGM_DIAL_AskIntroduction":
                        // robotEmoton_show("neutral");
                        //  flag_speak_completed = false;
                        if (RandomNumber <= 90)
                        {
                            if (ResultName == "introduce yourself")
                            {
                                Function.FaceLED.Instance.Happy();
                                Thread.Sleep(1000);
                                SRG.srespeech.SpeakAsync("My name is Ruth, I am a social robot designed by students from Nanyung Polytechnic and Kitakyushu National College of Technology. I have the abilities to recognize human emotions and natural language. I can also perform a variety of function such as news reading, weather broadcast, skype call or even music playing.");
                            }
                            else
                            {
                                Function.FaceLED.Instance.Happy();
                                Thread.Sleep(1000);
                                SRG.srespeech.SpeakAsync("I can perform lots of functions. I can turn on the lights by remote controlling. I can recognize human's facial expression and tell when you are happy or when you are sad. I can set reminders for taking medicine, appointments. I can also call for help when the elderly is in danger by making phone calls using Skype.");
                            }
                        }
                        else
                        {
                            Function.FaceLED.Instance.Sad();
                            Thread.Sleep(1000);
                            SRG.srespeech.SpeakAsync("I'm so tired.");
                        }
                        break;


                    case "SGM_DIAL_AskRobotName":
                        //robotEmoton_show("neutral");

                        //   if (flag_registered == false)
                        //   {
                        //       flag_speak_completed = false;
                        //       synthesizer.SpeakAsync("My name is Eva. What about you?");
                        //    }
                        //   else
                        //    {
                        //        flag_speak_completed = false;
                        //        synthesizer.SpeakAsync("I'm Eva, my dear " + user_username + ". Do you forget me?");
                        //        flag_registered = false;
                        //    }
                        Function.FaceLED.Instance.Happy();
                        Thread.Sleep(800);
                        SRG.srespeech.SpeakAsync("My name is Ruth. What about you?");
                        UI.Text = "I'm Ruth";
                        break;

                    //    case "SGM_DIAL_AskFunctions":
                    //    robotEmoton_show("neutral");
                    //    flag_speak_completed = false;
                    //    SRG.srespeech.SpeakAsync("I can perform a variety of functions.");

                    //break;
                    case "SGM_DIAL_AskWhoDesign":
                        // robotEmoton_show("neutral");
                        // flag_speak_completed = false;
                        if (ResultName == "how old are you")
                        {
                            Motor.ArmInitialize();
                            SRG.srespeech.Speak("I was created in 2014. So, does that means I'm " + (System.DateTime.Now.Year - 2014) + " years old?");
                            Motor.LeftArmRest();
                            Motor.RightArmRest();
                            SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                        }
                        else
                        {
                            Motor.ArmInitialize();
                            SRG.srespeech.Speak("I was designed by students from Nanyang Polytechnic and Kitakyushu National College of Technology.");
                            Motor.LeftArmRest();
                            Motor.RightArmRest();
                            SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                        }
                        break;

                    case "SGM_DIAL_AskWhatIsSocialRobot":
                        //robotEmoton_show("neutral");
                        // flag_speak_completed = false;
                        Motor.ArmInitialize();
                        SRG.srespeech.Speak("Sure. According to Wikipedia, a social robot is an autonomous robot that interacts and communicates with humans or other autonomous physical agents by following social behaviors and rules attached to its role.");
                        Motor.LeftArmRest();
                        Motor.RightArmRest();
                        SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                        break;

                    case "SGM_DIAL_AskRobotHowAreYou":
                        //robotEmoton_show("happiness");
                        //flag_speak_completed = false;
                        UI.Text = "I'm fine!";
                        if (RandomNumber < 25)
                        {
                            try
                            {
                                Function.FaceLED.Instance.Happy();
                            }
                            catch { }
                            Thread.Sleep(500);
                            SRG.srespeech.SpeakAsync("I am fine, thank you.");
                        }
                        else if (RandomNumber < 50)
                        {
                            try
                            {
                                Function.FaceLED.Instance.Happy();
                            }
                            catch { }
                            Thread.Sleep(500);
                            SRG.srespeech.SpeakAsync("Excellent. How are you?");
                        }
                        else if (RandomNumber < 75)
                        {
                            SRG.srespeech.SpeakAsync("Not bad.");
                        }
                        else
                        {
                            try
                            {
                                Function.FaceLED.Instance.Happy();
                            }
                            catch { }
                            Thread.Sleep(500);
                            SRG.srespeech.SpeakAsync("Pretty good.");
                        }
                        break;

                    case "SGM_DIAL_Compliment":
                        //robotEmoton_show("happiness");
                        //flag_speak_completed = false;
                        UI.Text = "Thank you!";
                        try
                        {
                            Function.FaceLED.Instance.Smile();
                        }
                        catch { }
                        Motor.ArmInitialize();
                        Thread.Sleep(500);
                        SRG.srespeech.SpeakAsync("Thank you very much.");
                        break;

                    case "SGM_DIAL_Scold":
                        // robotEmoton_show("sadness");
                        //  flag_speak_completed = false;
                        if (ResultName == "fuck you" && RandomNumber <= 30)
                        {
                            UI.Text = "Angry";
                            try
                            {
                                Function.FaceLED.Instance.Angry();
                                Motor.EyesHalfClose();
                            }
                            catch { }
                            Thread.Sleep(500);
                        }
                        else
                        {
                            UI.Text = "I'm so sorry";
                            try
                            {
                                Function.FaceLED.Instance.Sad();
                            }
                            catch { }
                            Thread.Sleep(500);
                            SRG.srespeech.SpeakAsync("I am so sorry.");
                        }
                        break;

                    case "SGM_DIAL_ThankYou":
                        //robotEmoton_show("happiness");
                        //flag_speak_completed = false;
                        UI.Text = "You are welcome";
                        try
                        {
                            Function.FaceLED.Instance.Happy();
                        }
                        catch { }
                        Thread.Sleep(500);
                        if (RandomNumber < 33)
                        {
                            SRG.srespeech.SpeakAsync("You are welcome.");
                        }
                        else if (RandomNumber < 66)
                        {
                            SRG.srespeech.SpeakAsync("My pleasure.");
                        }
                        else
                        {
                            SRG.srespeech.SpeakAsync("No problem.");
                        }
                        // SRG.LayerGrammarLoadAndUnload("SGM_DIAL_ThankYou", "Initialize");
                        break;

                    case "SGM_FUNC_GoingOut":
                        switch (ResultName)
                        {
                            case "how is the weather":
                                getweather(homecountry, "today");
                                break;

                            case "What is the weather":
                                getweather(homecountry, "today");
                                break;

                            case "I am going out now":
                                getweather(homecountry, "goingout");
                                break;

                            case "how is the weather today":
                                getweather(homecountry, "today");
                                break;
                            case "how is the weather tomorrow":
                                getweather(homecountry, "tomorrow");
                                break;
                            case "how is the weather for today":
                                getweather(homecountry, "today");
                                break;
                            case "how is the weather for tomorrow":
                                getweather(homecountry, "tomorrow");
                                break;
                            case "how is the weather now":
                                getweather(homecountry, "now");
                                break;

                        }
                        break;

                    case "SGM_DIAL_SayHello":
                        //robotEmoton_show("happiness");
                        // flag_speak_completed = false;
                        // if (ProcessValid == false)
                        // {
                        //     synthesizer.SpeakAsync(ResultText);
                        // }
                        //  else
                        //  {
                        //       synthesizer.SpeakAsync("Hello there");
                        //   }
                        //   break;
                        if (!DemoMode)
                        {
                            if (Function.Vision.DataLength < 27)//Nobody in the view
                            {
                                //if (!OneTimeMark)
                                //{
                                //    UI.Text = "Hi!";
                                //    SRG.srespeech.SpeakAsync("Sorry, where are you?");
                                //    OneTimeMark = true;
                                //}
                                //else
                                //{
                                //    UI.Text = "Hi!";
                                //    if (RandomNumber <= 50)
                                //    {
                                //        SRG.srespeech.SpeakAsync("Hi.");
                                //    }
                                //    else
                                //    {
                                //        SRG.srespeech.SpeakAsync("Hello.");
                                //    }
                                //}
                                //Motor.NeckRandom();
                            }
                            else if (Function.Vision.UserName != "Unknown")
                            {
                                UI.Text = "Hi!";
                                if (RandomNumber <= 50)
                                {
                                    SRG.srespeech.SpeakAsync("Hi " + Function.Vision.UserName);
                                }
                                else
                                {
                                    SRG.srespeech.SpeakAsync("Hello " + Function.Vision.UserName);
                                }
                                OneTimeMark = false;
                                Motor.RightArmRaise();
                            }
                            else if (Function.Vision.RegisterModeSwitch && Function.Vision.DataLength == 27)
                            {
                                Function.Vision.Instance.RegisterMode();
                                UI.Text = "Hi!";
                                SRG.srespeech.SpeakAsync("Sorry, it seems I never seen you before. Do you want to register in my database?");
                                SRG.sre.LoadGrammar(SRG.SGM_FUNC_RegisterMode_YesNo);
                                GrammarTimer.Start();
                                OneTimeMark = false;
                                Motor.RightArmRaise();

                            }
                            else
                            {
                                if (RandomNumber <= 50)
                                {
                                    SRG.srespeech.SpeakAsync("Hi.");
                                }
                                else
                                {
                                    SRG.srespeech.SpeakAsync("Hello.");
                                }
                                OneTimeMark = false;
                                Motor.RightArmRaise();
                            }
                        }
                        else
                        {
                            if (RandomNumber <= 50)
                            {
                                SRG.srespeech.SpeakAsync("Hi.");
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync("Hello.");
                            }
                            Motor.RightArmRaise();
                        }
                        if (ImagineMark)
                        {
                            SRG.sre.UnloadGrammar(SRG.SGM_DIAL_SayHello);
                        }
                        break;

                    case "SGM_DIAL_GoodBye":
                        //robotEmoton_show("happiness");
                        // flag_speak_completed = false;
                        //UI.Text = "Goodbye";
                        //Function.FaceLED.Instance.Happy();
                        //Thread.Sleep(1000);
                        //SRG.srespeech.SpeakAsync("Goodbye.");
                        break;

                    case "SGM_DIAL_SwitchLanguageToChinese":
                        UI.Text = "Switch to Chinese?";
                        SRG.srespeech.SpeakAsync("Do you want me to speak Chinese?");
                        break;

                    case "SGM_DIAL_SwitchLanguageToChinese_YesNo":
                        if (ResultName == "yes")
                        {
                            UI.Text = "切换至中文";
                            SRG.srecnspeech.SpeakAsync("好的，切换至中文语音识别");
                            SwitchToChinese();
                        }
                        else
                        {
                            UI.Text = "Ok, nevermind";
                            SRG.srespeech.SpeakAsync("Ok, never mind");
                        }
                        break;

                    case "SGM_DIAL_SwitchLanguageToJapanese":
                        UI.Text = "Switch to Japanese?";
                        SRG.srespeech.SpeakAsync("Do you want to switch to Japanese speech recgnition?");
                        break;

                    case "SGM_DIAL_SwitchLanguageToJapanese_YesNo":
                        if (ResultName == "yes")
                        {
                            UI.Text = "日本語に切り替えました。";
                            SRG.srejpspeech.SpeakAsync("はい、日本語に切り替えました。");
                            SwitchToJapanese();
                        }
                        else
                        {
                            UI.Text = "Ok, nevermind";
                            SRG.srespeech.SpeakAsync("Ok, never mind");
                        }
                        break;

                    case "SGM_DIAL_LookAtMe":
                        if (ResultName == "Look at me")
                        {
                            Motor.EyesBallInitialize();
                            if (RandomNumber <= 50)
                            {
                                Function.FaceLED.Instance.Happy();
                            }
                            else
                            {
                                Function.FaceLED.Instance.Surprise();
                            }
                        }
                        else if (ResultName == "ruth, can you smile" || ResultName == "say, cheese")
                        {
                            Function.FaceLED.Instance.Happy();
                        }
                        else if (ResultName == "ruth, do you want me to take a photo" || ResultName == "ruth, let's take a photo" || ResultName == "ruth, let's take a photo together")
                        {
                            Function.FaceLED.Instance.Happy();
                            Function.Motor.Instance.RightArmRaise();
                            Thread.Sleep(800);
                            SRG.srespeech.Speak("Sure, why not.");
                            Function.FaceLED.Instance.Happy();
                        }
                        else if (ResultName == "ruth I'm here")
                        {
                            if (Convert.ToInt32(2048 + beamAngleInDeg * 1024 / 90) < Motor.BtmNeckUpLimit && Convert.ToInt32(2048 + beamAngleInDeg * 1024 / 90) > Motor.BtmNeckLowLimit)
                            {
                                Motor.MotorWrite_funcNeck(Motor.BtmNeckID, 50, Convert.ToInt32(2048 + beamAngleInDeg * 1024 / 90));
                            }
                            if (RandomNumber <= 50)
                            {
                                SRG.srespeech.SpeakAsync("Oh, Hi.");
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync("Oh, Hello.");
                            }
                            Motor.RightArmRaise();
                        }
                        else if (ResultName == "Open your eyes")
                        {
                            Motor.EyesOpen();
                        }
                        else if (ResultName == "ruth are you okay?")
                        {
                            Motor.MidMotorInitialize();
                            SRG.srespeech.SpeakAsync("Oh, I'm OK.");
                        }
                        else if (ResultName == "who is the CEO of Microsoft, ruth?")
                        {
                            SRG.srespeech.SpeakAsync("The CEO of microsoft is Mr Satya Nadella.");
                        }
                        else if (ResultName == "do you know me?" || ResultName == "what's my name?" || ResultName == "who am i, ruth")
                        {
                            if (Function.Vision.UserName != "Unknown")
                            {
                                SRG.srespeech.SpeakAsync("You are " + Function.Vision.UserName);
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync("You are my friend.");
                            }
                        }
                        break;

                    case "SGM_DIAL_Sleep":
                        if (ResultName == "have a rest ruth")
                        {
                            SRG.srespeech.SpeakAsync("OK, see you next time.");
                            Thread.Sleep(1000);
                            this.Close();
                        }
                        else
                        {
                            SRG.srespeech.SpeakAsync("Do you want me to go to sleep?");
                        }
                        break;

                    case "SGM_DIAL_Sleep_YesNo":
                        switch (ResultName)
                        {
                            case "yes":
                                SRG.srespeech.Speak("Ok, see you next time.");
                                this.Close();
                                break;

                            case "no":
                                SRG.srespeech.SpeakAsync("Ok, never mind");
                                break;
                        }

                        break;

                    ////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////

                    case "SGM_FUNC_StartRadioStaion":
                        SRG.srespeech.SpeakAsync("Do you want me to start playing radio?");
                        //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_StartRadioStaion", "");
                        break;

                    case "SGM_FUNC_StopRadioStation":
                        SRG.srespeech.SpeakAsync("Do you want to stop playing radio?");
                        //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_StopRadioStation", "");
                        break;

                    case "SGM_FUNC_StartRadioStationYesNo":
                        if (ResultName == "yes")
                        {
                            UI.Text = "Ok, start playing radio.";
                            SRG.srespeech.SpeakAsync("Ok, start playing radio.");
                            Process.Start("http://streema.com/radios/play/43058");
                            //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_StartRadioStationYesNo", "");
                        }
                        else if (ResultName == "no")
                        {
                            UI.Text = "Ok, nevermind";
                            SRG.srespeech.SpeakAsync("Ok, never mind");
                        }

                        break;

                    case "SGM_FUNC_StopRadioStationYesNo":
                        if (ResultName == "yes")
                        {
                            UI.Text = "Ok, close radio";
                            SRG.srespeech.SpeakAsync("Ok, close radio");
                            Process[] processNames = Process.GetProcessesByName("chrome");
                            foreach (Process item in processNames)
                            {
                                item.Kill();
                            }
                            SRG.LayerGrammarLoadAndUnload("SGM_FUNC_StopRadioStationYesNo", "");
                        }

                        else if (ResultName == "no")
                        {
                            UI.Text = "Ok,nevermind";
                            SRG.srespeech.SpeakAsync("Ok,never mind");
                        }
                        break;

                    //////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////
                    //case "SGM_FUNC_ControlTV":
                    //    UI.Text = "Control TV";
                    //    SRG.srespeech.SpeakAsync("Ok,I can help you to control the TV");
                    //    device = "TV";
                    //    //SRG.LayerGrammarLoadAndUnload("SGM_FUNC_ControlTV", "");
                    //    break;

                    //case "SGM_FUNC_ControlProjector":
                    //    UI.Text = "Control Projector";
                    //    SRG.srespeech.SpeakAsync("Ok, I will help you to control the projector");
                    //    device = "projector";
                    //    //SRG.LayerGrammarLoadAndUnload("SGM_FUNC_ControlProjector", "");
                    //    break;

                    //case "SGM_FUNC_PowerOnOffTV":
                    //    UI.Text = "Power";
                    //    SRG.srespeech.SpeakAsync("Ok");
                    //    iRKit.iRkitcontrol(device, "power");
                    //    break;

                    //case "SGM_FUNC_MenuTV":
                    //    UI.Text = "Menu";
                    //    iRKit.iRkitcontrol(device, "menu");
                    //    break;

                    //case "SGM_FUNC_MuteTV":
                    //    UI.Text = "Mute";
                    //    iRKit.iRkitcontrol(device, "mute");
                    //    break;

                    //case "SGM_FUNC_ChangeInputTV":
                    //    UI.Text = "Change Input";
                    //    iRKit.iRkitcontrol(device, "change input");
                    //    break;

                    //case "SGM_FUNC_upTV":
                    //    UI.Text = "Up";
                    //    iRKit.iRkitcontrol(device, "up");
                    //    break;

                    //case "SGM_FUNC_downTV":
                    //    UI.Text = "Down";
                    //    iRKit.iRkitcontrol(device, "down");
                    //    break;

                    //case "SGM_FUNC_leftTV":
                    //    UI.Text = "Left";
                    //    iRKit.iRkitcontrol(device, "left");
                    //    break;

                    //case "SGM_FUNC_rightTV":
                    //    UI.Text = "Right";
                    //    iRKit.iRkitcontrol(device, "right");
                    //    break;

                    //case "SGM_FUNC_enterTV":
                    //    UI.Text = "Enter";
                    //    iRKit.iRkitcontrol(device, "enter");
                    //    break;

                    //case "SGM_FUNC_ChannelPlusTV":
                    //    UI.Text = "Next Channel";
                    //    iRKit.iRkitcontrol(device, "channel plus");
                    //    break;

                    //case "SGM_FUNC_ChannelMinusTV":
                    //    UI.Text = "Previous Channel";
                    //    iRKit.iRkitcontrol(device, "channel minus");
                    //    break;

                    //case "SGM_FUNC_VolumePlusTV":
                    //    UI.Text = "Volume Plus";
                    //    iRKit.iRkitcontrol(device, "volume plus");
                    //    break;

                    //case "SGM_FUNC_VolumeMinusTV":
                    //    UI.Text = "Volume Minus";
                    //    iRKit.iRkitcontrol(device, "volume minus");
                    //    break;
                    ///////////////////////////////////////////////////////////////////////////////////////


                    case "SGM_FUNC_ControlFan":
                        UI.Text = "Control Fan";
                        SRG.srespeech.SpeakAsync("Ok, I will help you to control the Fan");
                        device = "fan";
                        break;

                    case "SGM_FUNC_PowerOnOffFan":
                        UI.Text = "Power";
                        SRG.srespeech.SpeakAsync("Ok");
                        iRKit.iRkitcontrol(device, "power on");
                        break;

                    case "SGM_FUNC_onLeftRightFan":
                        UI.Text = "Left Right";
                        iRKit.iRkitcontrol(device, "left right");
                        break;

                    case "SGM_FUNC_onUpDown":
                        UI.Text = "Up Down";
                        iRKit.iRkitcontrol(device, "up down");
                        break;

                    case "SGM_FUNC_Speed":
                        UI.Text = "Speed";
                        iRKit.iRkitcontrol(device, "speed");
                        break;

                    case "SGM_FUNC_Timer":
                        UI.Text = "Timer";
                        iRKit.iRkitcontrol(device, "timer");
                        break;

                    case "SGM_FUNC_ControlRadio":
                        UI.Text = "Control Radio";
                        SRG.srespeech.SpeakAsync("Ok, I will help you to control the Radio");
                        device = "radio";
                        break;

                    case "SGM_FUNC_PowerOnOffRadio":
                        UI.Text = "Power";
                        SRG.srespeech.SpeakAsync("Ok, power on");
                        iRKit.iRkitcontrol(device, "power on");
                        break;


                    case "SGM_FUNC_NextRadio":
                        UI.Text = "Next";
                        SRG.srespeech.SpeakAsync("Ok,next channel");
                        iRKit.iRkitcontrol(device, "next channel");
                        break;

                    case "SGM_FUNC_PreviousRadio":
                        UI.Text = "Previous";
                        SRG.srespeech.SpeakAsync("Ok, previous channel");
                        iRKit.iRkitcontrol(device, "previous channel");
                        break;

                    case "SGM_FUNC_VolumeUpRadio":
                        UI.Text = "Volume Up";
                        iRKit.iRkitcontrol(device, "volume pluse");
                        break;

                    case "SGM_FUNC_VolumeDownRadio":
                        UI.Text = "Volume Down";
                        iRKit.iRkitcontrol(device, "volume minus");
                        break;


                    //case "SGM_FUNC_PowerOnLight":
                    //    UI.Text = "Light";
                    //    LightCon.control("on");
                    //    SRG.srespeech.SpeakAsync("Ok, light on");
                    //    break;

                    //case "SGM_FUNC_PowerOffLight":
                    //    UI.Text = "Light";
                    //    LightCon.control("off");
                    //    SRG.srespeech.SpeakAsync("Ok, light off");
                    //    break;


                    ///////////////////////////////////////////////////////////////////////////////////////
                    case "SGM_FUNC_AskSkypeFunction":
                        UI.Text = "Yes, I have skype";
                        SRG.srespeech.SpeakAsync("Yes, I have skype");
                        break;

                    case "SGM_FUNC_SkypePhoneCall":
                        UI.Text = "Ok,Which country you want to call?";
                        SRG.srespeech.SpeakAsync("Ok,Which country you want to call?");
                        //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_SkypePhoneCall","");
                        break;

                    case "SGM_FUNC_SkypeCall_PhoneCall_Country":
                        if (ResultName == "Japan")
                        {
                            UI.Text = "Japan";
                            country_number = "+81";
                        }

                        if (ResultName == "Singapore")
                        {
                            UI.Text = "Singapore";
                            country_number = "+65";
                        }
                        else if (ResultName == "China")
                        {
                            UI.Text = "China";
                            country_number = "+86";
                        }
                        //sre.RecognizeAsyncCancel();


                        SRG.srespeech.SpeakAsync("Ok, give me the number please");
                        //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_SkypeCall_PhoneCall_Country", "");
                        break;

                    //case "SGM_FUNC_SkypeCall_PhoneNumber":
                    //    if (country_number == "+65")
                    //    {
                    //        if (i < 8)
                    //        {
                    //            if (ResultName == "backspace")
                    //            {
                    //                phone_number = phone_number.Substring(0, phone_number.Length - 1);
                    //                UI.Text = phone_number;
                    //                //SkypeInterface.PhoneNumber.Text = phone_number;
                    //                i = i - 1;
                    //            }
                    //            else
                    //            {
                    //                //switch(selection)
                    //                //case"voice"
                    //                number = ResultName;
                    //                phone_number = phone_number + number;
                    //                UI.Text = phone_number;
                    //                i++;
                    //            }
                    //        }
                    //        else if (i == 8)
                    //        {
                    //            SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SkypeCall_PhoneNumber);
                    //            number = ResultName;
                    //            phone_number = phone_number + number;
                    //            UI.Text = phone_number;
                    //            Thread.Sleep(1000);
                    //            //SRG.LayerGrammarLoadAndUnload("SGM_FUNC_SkypeCall_PhoneNumber", "");
                    //            country_phone_number = country_number + phone_number;
                    //            char[] code_2 = phone_number.ToCharArray();
                    //            SRG.srespeech.SpeakAsync("Do you want to call " + code_2[0] + " " + code_2[1] + " " + code_2[2] + " " + code_2[3] + " " + code_2[4] + " " + code_2[5] + " " + code_2[6] + " " + code_2[7] + "?");
                    //            i = 0;
                    //            SRG.sre.LoadGrammar(SRG.SGM_FUNC_SkypeCall_PhoneCall_YesNo);
                    //        }

                    //    }

                    //    else if (country_number == "+81")
                    //    {
                    //        if (i < 11)
                    //        {
                    //            if (ResultName == "backspace")
                    //            {
                    //                phone_number = phone_number.Substring(0, phone_number.Length - 1);
                    //                UI.Text = phone_number;
                    //                //SkypeInterface.PhoneNumber.Text = phone_number;
                    //                i = i - 1;
                    //            }
                    //            else
                    //            {
                    //                number = ResultName;
                    //                phone_number = phone_number + number;
                    //                UI.Text = phone_number;
                    //                i++;
                    //            }
                    //        }

                    //        else if (i == 11)
                    //        {
                    //            SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SkypeCall_PhoneNumber);
                    //            number = ResultName;
                    //            phone_number = phone_number + number;
                    //            UI.Text = phone_number;
                    //            Thread.Sleep(1000);
                    //            //SRG.LayerGrammarLoadAndUnload("SGM_FUNC_SkypeCall_PhoneNumber", "");
                    //            country_phone_number = country_number + phone_number;
                    //            SRG.srespeech.SpeakAsync("Do you want to call this number?");
                    //            i = 0;
                    //            SRG.sre.LoadGrammar(SRG.SGM_FUNC_SkypeCall_PhoneCall_YesNo);
                    //        }
                    //    }
                    //    break;

                    case "SGM_FUNC_SkypeCall_PhoneCall_YesNo":
                        if (ResultName == "yes")
                        {
                            //  Skype.MakeCall(country_phone_number);
                            SRG.srespeech.Speak("Sure, I'm calling now");

                            Skype.MakeCall("97888234");
                            Motor.LeftArmRaise();
                            Thread.Sleep(1000);
                            for (;;)
                            {
                                Motor.RightArmRaise();
                                Motor.LeftArmRest();
                                Thread.Sleep(1000);
                                Motor.LeftArmRaise();
                                Motor.RightArmRest();
                                Thread.Sleep(1000);
                            }
                            //SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SkypeCall_PhoneCall_YesNo);


                        }
                        else if (ResultName == "no")
                        {
                            SRG.srespeech.SpeakAsync("Ok, never mind");
                        }
                        break;


                    case "SGM_FUNC_SkypePhoneCallFinished":
                        if (Function.Vision.UserName != "Unknown")
                        {
                            SRG.srespeech.SpeakAsync("ok");
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.NEXT);
                            Thread.Sleep(100);
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                            Thread.Sleep(100);
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                            Thread.Sleep(10);
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_N);
                        }
                        break;




                    ///////////////////////////////////////////////////////////////////////////

                    case "SGM_FUNC_SendMessage":
                        SRG.srespeech.SpeakAsync("Ok, do you want to send Skype message or SMS?");
                        //SRG.sre.LoadGrammar(SRG.SGM_FUNC_SendMessageOption);
                        //SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SendSMS);
                        break;

                    case "SGM_FUNC_SendMessageOption":
                        if (ResultName == "SMS")
                        {
                            SRG.srespeech.SpeakAsync("Ok, which country you want to send?");

                            //     SRG.sre.LoadGrammar(SRG.SGM_FUNC_SMSCountry);
                            //      SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SendMessageOption);
                        }

                        else if (ResultName == "message")
                        {

                        }
                        break;

                    case "SGM_FUNC_SendSMS":
                        SRG.srespeech.SpeakAsync("Ok, which country you want to send?");

                        //     SRG.sre.LoadGrammar(SRG.SGM_FUNC_SMSCountry);


                        break;


                    case "SGM_FUNC_SMSCountry":
                        if (ResultName == "Japan")
                        {
                            UI.Text = "Japan";
                            country_number = "+81";

                        }

                        if (ResultName == "Singapore")
                        {
                            UI.Text = "Singapore";
                            country_number = "+65";

                        }
                        else if (ResultName == "China")
                        {
                            UI.Text = "China";
                            country_number = "+86";

                        }
                        //     SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SMSCountry);
                        SRG.srespeech.SpeakAsync("Ok, give me the number please");
                        Thread.Sleep(3000);
                        //      SRG.sre.LoadGrammar(SRG.SGM_FUNC_SMSPhoneNumber);
                        break;

                    case "SGM_FUNC_SMSPhoneNumber":
                        if (country_number == "+65")
                        {
                            if (i < 8)
                            {
                                if (ResultName == "backspace")
                                {
                                    phone_number = phone_number.Substring(0, phone_number.Length - 1);
                                    UI.Text = phone_number;
                                    i = i - 1;
                                }
                                else
                                {
                                    number = ResultName;
                                    phone_number = phone_number + number;
                                    UI.Text = phone_number;
                                    i++;
                                }
                            }
                            else if (i == 8)
                            {
                                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SMSPhoneNumber);
                                number = ResultName;
                                phone_number = phone_number + number;
                                UI.Text = phone_number;
                                Thread.Sleep(1000);

                                country_phone_number = country_number + phone_number;
                                SRG.srespeech.SpeakAsync("Do you want to send message to this number?");
                                i = 0;
                                SRG.sre.LoadGrammar(SRG.SGM_FUNC_SendSMSYesOrNo);
                            }

                        }

                        else if (country_number == "+81")
                        {
                            if (i < 11)
                            {
                                if (ResultName == "backspace")
                                {
                                    phone_number = phone_number.Substring(0, phone_number.Length - 1);
                                    UI.Text = phone_number;
                                    //SkypeInterface.PhoneNumber.Text = phone_number;
                                    i = i - 1;
                                }
                                else
                                {
                                    number = ResultName;
                                    phone_number = phone_number + number;
                                    UI.Text = phone_number;
                                    i++;
                                }
                            }

                            else if (i == 11)
                            {
                                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SMSPhoneNumber);
                                number = ResultName;
                                phone_number = phone_number + number;
                                UI.Text = phone_number;
                                Thread.Sleep(1000);
                                country_phone_number = country_number + phone_number;
                                SRG.srespeech.SpeakAsync("Do you want to send message to this number?");
                                i = 0;
                                SRG.sre.LoadGrammar(SRG.SGM_FUNC_SendSMSYesOrNo);
                            }

                        }
                        break;


                    case "SGM_FUNC_SendSMSYesOrNo":
                        if (ResultName == "Yes")
                        {
                            SRG.srespeech.SpeakAsync("ok, start speech to text program");
                            Thread.Sleep(2000);
                            SwitchWindow_TextDocument();
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.F2);
                        }

                        //   SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SendSMSYesOrNo);
                        //    SRG.sre.LoadGrammar(SRG.SGM_FUNC_SpeechToTextMessageFinished);
                        break;

                    case "SGM_FUNC_SpeechToTextMessageFinished":

                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F2);

                        Thread.Sleep(1000);

                        InputSimulator.SimulateKeyDown(VirtualKeyCode.CONTROL);
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);

                        Thread.Sleep(1000);

                        foreach (Process proc2 in Process.GetProcessesByName("WordPad"))
                        {
                            proc2.Kill();
                        }

                        Thread.Sleep(1000);

                        if (File.Exists(fileLoc))
                        {
                            using (TextReader tr = new StreamReader(fileLoc))
                            {
                                //SMS.Text = tr.ReadLine();
                                UI.Text = tr.ReadLine();
                            }
                        }
                        Thread.Sleep(1000);
                        SRG.srespeech.SpeakAsync("OK,sned message");
                        Skype.SendSMS(country_phone_number, UI.Text);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    case "SGM_FUNC_COMPLEX_SetReminderYesNo":
                        if (ResultName == "yes")
                        {
                            //SetReminderFunction.WriteGoogleCalendarReminder(reminder_task, DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(subday_num), Convert.ToInt32(subhour), Convert.ToInt32(subminute));
                            xmlmaker();
                            loadxml();
                            UI.Text = "Ok, reminder set.";
                            SRG.srespeech.SpeakAsync("Ok, reminder set.");
                            setalarm();
                        }
                        else if (ResultName == "no")
                        {
                            UI.Text = "Ok, nevermind";
                            SRG.srespeech.SpeakAsync("Ok, never mind");
                        }
                        break;

                    case "SGM_FUNC_CountdownYesNo":
                        if (ResultName == "yes")
                        {
                            SRG.srespeech.SpeakAsync("Ok.");
                            CountdownFunction.CountDownFunction();
                        }
                        else if (ResultName == "no")
                        {
                            SRG.srespeech.SpeakAsync("ok, never mind");
                        }
                        break;

                    case "SGM_FUNC_ReadNews":
                        SRG.srespeech.SpeakAsync("English news or Chinese news?");
                        break;

                    case "SGM_FUNC_LanguageOption":
                        SRG.srespeech.SpeakAsync("Ok");
                        Thread.Sleep(1000);
                        if (ResultName == "English news")
                        {
                            EnglishNewsMark = true;
                            ReadNewsFunction.flag_StartNewsReading = true;
                            ReadNewsFunction.NewsReadingFunction_Eng();
                        }

                        else if (ResultName == "Chinese news")
                        {
                            EnglishNewsMark = false;
                            ReadNewsFunction.flag_StartNewsReading = true;
                            ReadNewsFunction.NewsReadingFunction_Cn();
                        }
                        break;

                    case "SGM_ContinueReadNews_YesNo":
                        if (ResultName == "yes" || ResultName == "yes please")
                        {
                            if (EnglishNewsMark)
                            {
                                SRG.srespeech.SpeakAsync("Ok,next news");
                                Thread.Sleep(2000);
                                ReadNewsFunction.flag_StartNewsReading = true;
                                ReadNewsFunction.i_news++;
                                ReadNewsFunction.NewsReadingFunction_Eng();
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync("Ok,next news");
                                Thread.Sleep(2000);
                                ReadNewsFunction.flag_StartNewsReading = true;
                                ReadNewsFunction.i_news_CN++;
                                ReadNewsFunction.NewsReadingFunction_Cn();
                            }
                        }
                        else
                        {
                            SRG.srespeech.SpeakAsync("Ok, I will stop reading news.");
                            ReadNewsFunction.flag_StartNewsReading = false;
                            SRG.sre.LoadGrammar(SRG.SGM_FUNC_ReadNews);
                        }
                        break;

                    case "SGM_FUNC_NextNews":
                        ReadNewsTimer.Stop();
                        SRG.srespeech.SpeakAsync("Ok,next news");
                        Thread.Sleep(2000);
                        ReadNewsFunction.flag_StartNewsReading = true;
                        if (EnglishNewsMark)
                        {
                            ReadNewsFunction.i_news++;
                            ReadNewsFunction.NewsReadingFunction_Eng();
                        }
                        else
                        {
                            ReadNewsFunction.i_news_CN++;
                            ReadNewsFunction.NewsReadingFunction_Cn();
                        }
                        break;

                    case "SGM_FUNC_StopReadNews":
                        ReadNewsTimer.Stop();
                        SRG.srespeech.SpeakAsync("Ok, I will stop reading news.");
                        ReadNewsFunction.flag_StartNewsReading = false;
                        break;

                    case "SGM_FUNC_AskWhatTimeNow":
                        UI.Text = DateTime.Now.ToShortTimeString();
                        DateTimeFunction.timeNow("eng");
                        break;

                    case "SGM_FUNC_AskWhatDayIsToday":
                        UI.Text = DateTime.Now.ToString("dddd");
                        DateTimeFunction.dayNow("eng");
                        break;

                    case "SGM_FUNC_AskWhatDateIsToday":
                        UI.Text = DateTime.Now.ToShortDateString();
                        DateTimeFunction.dateNow("eng");
                        break;

                    case "SGM_FUNC_TellJokes":
                        //TellJokesFunction.RandomJokesAsync();
                        break;

                    case "SGM_FUNC_AskCountdown":
                        SRG.srespeech.SpeakAsync("Yes, I can do countdown for you.");
                        break;


                    case "SGM_GAME_PlayAnimalLegCounting":

                        SRG.srespeech.SpeakAsync("Sure, let's play animal leg counting game.      ");
                        AnimalLegCountingGame();
                        break;

                    //case "SGM_GAME_PlayAnimalLegCountingYesNo":
                    //    if (ResultName == "yes")
                    //    {
                    //        SRG.srespeech.SpeakAsync("Opening game now.");
                    //       // Thread.Sleep(1500);
                    //       AnimalLegCountingGame();
                    //    }

                    //    else if (ResultName == "no")
                    //    {
                    //        SRG.srespeech.SpeakAsync("Okay, let's play next time");
                    //    }

                    //    break;

                    case "SGM_GAME_AnimalLegCountingAnswer":
                        leg_answer = Int32.Parse(ResultName);
                        AnimalLegCounting_CheckAnswer(leg_answer);
                        break;

                    //case "SGM_GAME_AnimalLegCountingQuestionRepeat":               
                    //    SRG.srespeech.SpeakAsync("Sure. " + "How many legs do " + animal_number1 + " " + animals_game[n1_animals] + " and " + animal_number2 + " " + animals_game[n2_animals] + " have?");
                    //    break;

                    case "SGM_GAME_AnimalLegCountingAnswerDontKnow":
                        SRG.srespeech.SpeakAsync("It's okay, let's try another question.");
                        AnimalLegCountingGame();

                        break;
                    case "SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo":
                        SRG.srespeech.SpeakAsync("Next question.");
                        AnimalLegCountingGame();
                        break;


                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case "SGM_FUNC_AskCanPlayMusicOrSongNot":
                        SRG.srespeech.SpeakAsync("I have Spotify integrated, therefore i can play music or songs.");
                        break;

                    case "SGM_FUNC_AskWhatMusicOrSongGenreAvailable":
                        SRG.srespeech.SpeakAsync("I have fourteen genre here: alternative, blues, classical, country, emo, dance, latin, folk, indie, jazz, pop, soul, sixties and twothousands.");
                        break;

                    case "SGM_FUNC_StartMusicOrSong":
                        SRG.srespeech.SpeakAsync("Which genre of music or song do you want?");
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_alternative":
                        if (current_MusicOrSongGenre != "alternative")
                        {
                            CurrentGenreIndex = 1;
                            current_MusicOrSongGenre = "alternative";
                            SRG.srespeech.SpeakAsync("Do you want to play alternative genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_blues":
                        if (current_MusicOrSongGenre == "blues")
                        {
                            current_MusicOrSongGenre = "blues";
                            CurrentGenreIndex = 2;
                            SRG.srespeech.SpeakAsync("Do you want to play blues genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_classical":
                        if (current_MusicOrSongGenre == "classical")
                        {
                            current_MusicOrSongGenre = "classical";
                            CurrentGenreIndex = 3;
                            SRG.srespeech.SpeakAsync("Do you want to play classical genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_country":
                        if (current_MusicOrSongGenre == "country")
                        {
                            current_MusicOrSongGenre = "country";
                            CurrentGenreIndex = 4;
                            SRG.srespeech.SpeakAsync("Do you want to play country genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_dance":
                        if (current_MusicOrSongGenre == "country")
                        {
                            current_MusicOrSongGenre = "country";
                            CurrentGenreIndex = 6;
                            SRG.srespeech.SpeakAsync("Do you want to play dance genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_emo":
                        if (current_MusicOrSongGenre == "emo")
                        {
                            current_MusicOrSongGenre = "emo";
                            CurrentGenreIndex = 5;
                            SRG.srespeech.SpeakAsync("Do you want to play emo genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_folk":
                        if (current_MusicOrSongGenre == "folk")
                        {
                            current_MusicOrSongGenre = "folk";
                            CurrentGenreIndex = 8;
                            SRG.srespeech.SpeakAsync("Do you want to play folk genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_indie":
                        if (current_MusicOrSongGenre == "indie")
                        {
                            current_MusicOrSongGenre = "indie";
                            CurrentGenreIndex = 9;
                            SRG.srespeech.SpeakAsync("Do you want to play indie genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_jazz":
                        if (current_MusicOrSongGenre == "jazz")
                        {
                            current_MusicOrSongGenre = "jazz";
                            CurrentGenreIndex = 10;
                            SRG.srespeech.SpeakAsync("Do you want to play jazz genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_latin":
                        if (current_MusicOrSongGenre == "latin")
                        {
                            current_MusicOrSongGenre = "latin";
                            CurrentGenreIndex = 7;
                            SRG.srespeech.SpeakAsync("Do you want to play latin genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_pop":
                        if (current_MusicOrSongGenre == "pop")
                        {
                            current_MusicOrSongGenre = "pop";
                            CurrentGenreIndex = 11;
                            SRG.srespeech.SpeakAsync("Do you want to play pop genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_sixties":
                        if (current_MusicOrSongGenre == "sixties")
                        {
                            current_MusicOrSongGenre = "sixties";
                            CurrentGenreIndex = 13;
                            SRG.srespeech.SpeakAsync("Do you want to play sixties genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_soul":
                        if (current_MusicOrSongGenre == "soul")
                        {
                            current_MusicOrSongGenre = "soul";
                            CurrentGenreIndex = 12;
                            SRG.srespeech.SpeakAsync("Do you want to play soul genre?");
                        }

                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartOrSelectMusicOrSongGenre_twothousands":
                        if (current_MusicOrSongGenre == "soul")
                        {
                            current_MusicOrSongGenre = "soul";
                            CurrentGenreIndex = 14;
                            SRG.srespeech.SpeakAsync("Do you want to play twothousands genre?");
                        }
                        else
                        {
                            SRG.srespeech.SpeakAsync("You are already at " + current_MusicOrSongGenre + " radio.");
                        }
                        break;

                    case "SGM_FUNC_StartMusicOrSongGenreYesNo":
                        if (ResultName == "ture")
                        {
                            SpotifyFunction.SwitchWindow_toSpotify();
                            SRG.srespeech.SpeakAsync("Okay, playing " + current_MusicOrSongGenre + " Spotify radio station.");
                            SpotifyFunction.SpotifyRadio(CurrentGenreIndex);
                            SpotifyFunction.ContinuePlaySpotify();
                        }
                        else if (ResultName == "false")
                        {
                            SRG.srespeech.SpeakAsync("Perhaps later");
                        }
                        break;

                    case "SGM_FUNC_CHANGE_SGM_FUNC_StartMusicOrSongGenreYesNo":
                        for (int ii = 0; ii < 3; ii++)
                        {
                            SpotifyFunction.VolumeDown_Spotify();
                        }
                        SRG.srespeech.SpeakAsync("What genre do you want?");
                        for (int ii = 0; ii < 3; ii++)
                        {
                            SpotifyFunction.VolumeUp_Spotify();
                        }

                        break;

                    case "SGM_FUNC_NEXT_SGM_FUNC_StartMusicOrSongGenreYesNo":
                        SpotifyFunction.NextMusicSpotify();
                        break;

                    case "SGM_FUNC_PAUSE_SGM_FUNC_StartMusicOrSongGenreYesNo":
                        SpotifyFunction.PauseSpotify();
                        break;

                    case "SGM_FUNC_CONT_SGM_FUNC_StartMusicOrSongGenreYesNo":
                        SpotifyFunction.ContinuePlaySpotify();
                        break;


                    case "SGM_FUNC_STOP_SGM_FUNC_StartMusicOrSongGenreYesNo":
                        SpotifyFunction.PauseSpotify();
                        SRG.srespeech.SpeakAsync("Okay, closing Spotify now.");
                        break;

                    case "SGM_FUNC_FindSong":
                        SRG.srespeech.SpeakAsync("Do you want me to search a song?");
                        break;

                    case "SGM_FUNC_FindSongYesNo":

                        if (ResultName == "yes")
                        {


                            SpotifyFunction.SwitchWindow_toSpotify();


                            SRG.srespeech.SpeakAsync("Okay, starting Spotify, what's the song's name?");
                            SpotifyFunction.PauseSpotify();
                            text_test = "";
                        }
                        else if (ResultName == "no")
                        {
                            SRG.srespeech.SpeakAsync("Perhaps later");

                        }
                        break;

                    case "SGM_FUNC_ConfirmSongName":

                        SRG.srespeech.SpeakAsync("Hold on.");
                        SpotifyFunction.SearchSong(text_test);

                        break;
                    case "SGM_FUNC_STOP_SGM_FUNC_FindSongYesNo":

                        SpotifyFunction.PauseSpotify();
                        SRG.srespeech.SpeakAsync("Stopping now.");
                        break;

                    ////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    case "SGM_FUNC_RegisterMode_YesNo":
                        if (ResultName == "yes")
                        {
                            SRG.sre.UnloadAllGrammars();//Unsafe!!
                            SRG.srespeech.SpeakAsync("Please spell your name to me. If finish then just speak finish. If the character is wrong please speak no");
                            SRG.sre.LoadGrammar(SRG.SGM_FUNC_Char);
                        }
                        else
                        {
                            SRG.srespeech.SpeakAsync("Ok, never mind");
                            Function.Vision.Instance.RegisterModeQuit();
                            Function.Vision.RegisterModeSwitch = false;
                        }
                        break;

                    case "SGM_FUNC_Char":
                        if (ResultName == "finish")
                        {
                            SRG.sre.UnloadGrammar(SRG.SGM_FUNC_Char);
                            SRG.srespeech.SpeakAsync("So, I will call you: " + Function.Vision.Instance.NewUserName);
                            Function.Vision.Instance.Command(ResultName);
                            SRG.srespeech.SpeakAsync("Successfully register");
                            SRG.LayerGrammarLoadAndUnload("RegisterModeCompleted", "");
                        }
                        else
                        {
                            Function.Vision.Instance.Command(ResultName);
                            if (ResultName == "No")
                            {
                                if (Function.Vision.Instance.NewUserName == "")
                                {
                                    SRG.sre.UnloadGrammar(SRG.SGM_FUNC_Char);
                                    SRG.LayerGrammarLoadAndUnload("RegisterModeCompleted", "");
                                }

                                else
                                {
                                    SRG.srespeech.SpeakAsync("backspace");
                                }
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync(ResultName);
                            }
                        }
                        break;
                    case "SGM_DIAL_Raise_Right_Hand":
                        Motor.RightArmInitialize();
                        break;
                    case "SGM_DIAL_Raise_Left_Hand":
                        Motor.LeftArmInitialize();
                        break;

                    case "SGM_DIAL_Dance":
                        //     UI.Text = "Sure";
                        SRG.srespeech.SpeakAsync("Sorry, I don't have this function by now.");
                        //       SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                        //     mp3.music();
                        //Motor.ArmRandom();
                        //Motor.NeckShowNo();
                        //        Motor.Dance();
                        break;


                    //case "SGM_DIAL_Music":
                    //    switch (ResultName)
                    //    {
                    //        case "play me some music":
                    //            mp3.music("song");
                    //            break;

                    //        case "stop playing music":
                    //            mp3.music("stop");
                    //            break;
                    //    }
                    //Thread.Sleep(700);                   
                    //break;

                    //Imagine Cup
                    case "SGM_FUNC_ImagineCup":
                        if (ResultName == "let me introduce our team members")
                        {
                            Motor.MotorInitialize();
                            Function.Vision.Instance.FaceTrackingTimer.Start();
                            Function.FaceLED.Instance.Normal();
                            SRG.sre.UnloadGrammar(SRG.SGM_FUNC_ImagineCup);
                        }
                        break;


                    case "SGM_FUNC_Call":
                        SRG.srespeech.SpeakAsync("OK, tell me who you want to call.");
                        if (ResultName == "i want to make phone call")
                        {
                            VideoCallFlag = false;
                        }
                        else
                        {
                            VideoCallFlag = true;
                        }
                        break;

                    case "SGM_FUNC_CallPerson":
                        if (!VideoCallFlag)
                        {
                            string PhoneNumber = null;
                            try
                            {
                                string MyXMLFilePath = @"C:\test\XmlGrammar\English\Skype\ContactsData.xml";
                                XmlDocument MyXmlDoc = new XmlDocument();
                                MyXmlDoc.Load(MyXMLFilePath);
                                XmlNode RootNode = MyXmlDoc.SelectSingleNode("Users");
                                XmlNodeList FirstLevelNodeList = RootNode.ChildNodes;
                                foreach (XmlNode Node in FirstLevelNodeList)
                                {
                                    XmlNode SecondLevelNode1 = Node.FirstChild;
                                    if (SecondLevelNode1.InnerText == ResultName)
                                    {
                                        XmlNode SecondLevelNode2 = Node.ChildNodes[1];
                                        PhoneNumber = SecondLevelNode2.InnerText;
                                    }
                                }
                            }
                            catch
                            {
                                SRG.srespeech.SpeakAsync("Error!");
                                Environment.Exit(0);
                            }
                            Motor.RightArmRaise();
                            Function.FaceLED.Instance.Happy();
                            SRG.srespeech.SpeakAsync("OK, I'm calling " + ResultName + " for you.");
                            Thread.Sleep(2000);
                            if (PhoneNumber != null)
                            {
                                Skype.MakeCall("+" + PhoneNumber);
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync("Sorry I can not make video call for this person.");
                                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SkypePhoneCallFinished);
                            }
                        }
                        else
                        {
                            string ContactsID = null;
                            try
                            {
                                string MyXMLFilePath = @"C:\test\XmlGrammar\English\Skype\VideoContactsData.xml";
                                XmlDocument MyXmlDoc = new XmlDocument();
                                MyXmlDoc.Load(MyXMLFilePath);
                                XmlNode RootNode = MyXmlDoc.SelectSingleNode("Users");
                                XmlNodeList FirstLevelNodeList = RootNode.ChildNodes;
                                foreach (XmlNode Node in FirstLevelNodeList)
                                {
                                    XmlNode SecondLevelNode1 = Node.FirstChild;
                                    if (SecondLevelNode1.InnerText == ResultName)
                                    {
                                        XmlNode SecondLevelNode2 = Node.ChildNodes[1];
                                        ContactsID = SecondLevelNode2.InnerText;
                                    }
                                }
                            }
                            catch
                            {
                                SRG.srespeech.SpeakAsync("Error!");
                                Environment.Exit(0);
                            }
                            Motor.RightArmRaise();
                            Function.FaceLED.Instance.Happy();
                            SRG.srespeech.SpeakAsync("OK, I'm calling " + ResultName + " for you.");
                            Thread.Sleep(2000);
                            if (ContactsID != null)
                            {
                                Skype skype;
                                skype = new SKYPE4COMLib.Skype();
                                string SkypeID = ContactsID;
                                Call call = skype.PlaceCall(SkypeID);
                                do
                                {
                                    System.Threading.Thread.Sleep(1);
                                } while (call.Status != TCallStatus.clsInProgress);
                                call.StartVideoSend();
                            }
                            else
                            {
                                SRG.srespeech.SpeakAsync("Sorry I can not make video call for this person.");
                                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_SkypePhoneCallFinished);
                            }
                        }
                        break;

                    case "SGM_DIAL_Emotion":
                        if (LastEmotion != null && LastEmotion != "Neutral")
                        {
                            switch (LastEmotion)
                            {
                                //case "Happiness":
                                //    Function.FaceLED.Instance.Happy();
                                //    Function.Motor.Instance.EyesBlink();
                                //    Thread.Sleep(500);
                                //    Function.Motor.Instance.EyesBlink();
                                //    Thread.Sleep(500);
                                //    switch (language)
                                //    {
                                //        case "English":
                                //            SRG.srespeech.SpeakAsync("You look so happy today.");
                                //            break;
                                //        case "Chinese":
                                //            SRG.srecnspeech.SpeakAsync("你看起来好像很开心。发生了什么吗?");
                                //            break;
                                //        case "Japanese":
                                //            SRG.srejpspeech.SpeakAsync("あなたは今すごく幸せそうに見えます。何が起こったか?");
                                //            break;
                                //    }
                                //    break;

                                case "Surprise":
                                    Function.FaceLED.Instance.Smile();
                                    Function.Motor.Instance.EyesBlink();
                                    Thread.Sleep(500);
                                    Function.Motor.Instance.EyesBlink();
                                    Thread.Sleep(500);
                                    switch (language)
                                    {
                                        case "English":
                                            SRG.srespeech.SpeakAsync("Surprise!!!");
                                            break;
                                        case "Chinese":
                                            SRG.srecnspeech.SpeakAsync("哈哈?");
                                            break;
                                        case "Japanese":
                                            SRG.srejpspeech.SpeakAsync("びっくりしたか?");
                                            break;
                                    }
                                    break;

                                case "Anger":
                                    Function.FaceLED.Instance.Fear();
                                    Thread.Sleep(1000);
                                    switch (language)
                                    {
                                        case "English":
                                            SRG.srespeech.SpeakAsync("Relax, please relax!");
                                            break;
                                        case "Chinese":
                                            SRG.srecnspeech.SpeakAsync("我觉得你应该放轻松！");
                                            break;
                                        case "Japanese":
                                            SRG.srejpspeech.SpeakAsync("怖い顔ですね。");
                                            break;
                                    }
                                    break;

                                case "Sadness":
                                    Function.FaceLED.Instance.Surprise();
                                    Thread.Sleep(1000);
                                    switch (language)
                                    {
                                        case "English":
                                            SRG.srespeech.SpeakAsync("You look upset now.");//, maybe I can tell a joke to you to make you happy?
                                            break;
                                        case "Chinese":
                                            SRG.srecnspeech.SpeakAsync("你看起来似乎有些不开心呢 呵呵");
                                            break;
                                        case "Japanese":
                                            SRG.srejpspeech.SpeakAsync("何が…起こりましたか？");
                                            break;
                                    }
                                    break;

                                case "Contempt"://鄙视
                                    Function.FaceLED.Instance.Angry();
                                    Function.Motor.Instance.EyesHalfClose();
                                    Thread.Sleep(1000);
                                    switch (language)
                                    {
                                        case "English":
                                            SRG.srespeech.SpeakAsync("Why?");
                                            break;
                                        case "Chinese":
                                            SRG.srecnspeech.SpeakAsync("为什么要露出这样的表情");
                                            break;
                                        case "Japanese":
                                            SRG.srejpspeech.SpeakAsync("何を！");
                                            break;
                                    }
                                    break;

                                case "Disgust":
                                    Function.FaceLED.Instance.Disgust();
                                    Function.Motor.Instance.EyesHalfClose();
                                    Thread.Sleep(1000);
                                    switch (language)
                                    {
                                        case "English":
                                            SRG.srespeech.SpeakAsync("Hey!");
                                            break;
                                        case "Chinese":
                                            SRG.srecnspeech.SpeakAsync("你肚子痛吗？");
                                            break;
                                        case "Japanese":
                                            SRG.srejpspeech.SpeakAsync("気持ち悪い。");
                                            break;
                                    }
                                    break;

                                case "Fear":
                                    Function.FaceLED.Instance.Fear();
                                    Function.Motor.Instance.EyesHalfClose();
                                    Thread.Sleep(1000);
                                    switch (language)
                                    {
                                        case "English":
                                            SRG.srespeech.SpeakAsync("Don't be scared. Let's play a game.");
                                            break;
                                        case "Chinese":
                                            SRG.srecnspeech.SpeakAsync("不要怕 不要慌 我不会突然爆炸。");
                                            break;
                                        case "Japanese":
                                            SRG.srejpspeech.SpeakAsync("大丈夫だから");
                                            break;
                                    }
                                    break;

                                default:
                                    break;

                            }
                        }
                        else
                        {
                            SRG.srespeech.SpeakAsync("You look fine today.");
                        }
                        break;

                    case "SGM_DIAL_Help":
                        //SRG.srespeech.SpeakAsync("OK, I will call for help.");
                        //Skype skype1;
                        //skype1 = new SKYPE4COMLib.Skype();
                        //string SkypeID1 = "elthef";
                        //Call call1 = skype1.PlaceCall(SkypeID1);
                        //do
                        //{
                        //    System.Threading.Thread.Sleep(1);
                        //} while (call1.Status != TCallStatus.clsInProgress);
                        //call1.StartVideoSend();
                        //for(;;)
                        //{
                        //    SRG.srespeech.SpeakAsync("Mr. Tay is in dangerous, please help.");
                        //}
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    case "SGM_DIAL_你好":
                        UI.Text = "你好";
                        SRG.srecnspeech.SelectVoice("Microsoft Huihui Desktop");
                        SRG.srecnspeech.SpeakAsync("你好");
                        break;

                    case "SGM_DIAL_谢谢":
                        UI.Text = "不谢";
                        SRG.srecnspeech.SpeakAsync("不谢");
                        break;

                    case "SGM_DIAL_早上好":
                        UI.Text = "早上好";
                        SRG.srecnspeech.SpeakAsync("早上好");
                        break;

                    case "SGM_DIAL_中午好":
                        UI.Text = "中午好";
                        SRG.srecnspeech.SpeakAsync("中午好");
                        break;

                    case "SGM_DIAL_下午好":
                        UI.Text = "下午好";
                        SRG.srecnspeech.SpeakAsync("下午好");
                        break;

                    case "SGM_DIAL_晚上好":
                        UI.Text = "晚上好";
                        SRG.srecnspeech.SpeakAsync("晚上好");
                        break;

                    case "SGM_DIAL_你叫什么名字":
                        UI.Text = "我的名字叫做露丝";
                        SRG.srecnspeech.SpeakAsync("我的名字叫做露丝");
                        break;


                    case "SGM_DIAL_自我介绍":
                        SRG.srecnspeech.SpeakAsync("好的，我是由新加坡南洋理工学院和日本北九州高专联合设计和生产的。 这个项目的目的是制造一个智能社交机器人去理解人类的语言，从而成为他们的助手");
                        break;

                    case "SGM_DIAL_谁设计了你":
                        SRG.srecnspeech.SpeakAsync("好的，我是由新加坡南洋理工学院和日本北九州高专的学生联合设计的");
                        break;


                    case "SGM_DIAL_功能":
                        SRG.srecnspeech.SpeakAsync("我有一系列的功能");
                        break;

                    case "SGM_DIAL_英文识别":
                        UI.Text = "需要切换至英文语音识别吗？";
                        SRG.srecnspeech.SpeakAsync("需要切换至英文语音识别吗？");
                        break;

                    case "SGM_DIAL_英文识别_是否":
                        if (ResultName == "是的" || ResultName == "是")
                        {
                            UI.Text = "Ok, switch to English speech recognition";
                            SRG.srespeech.SpeakAsync("Ok, switch to English speech recognition");
                            SwitchToEnglish();
                        }
                        else
                        {
                            UI.Text = "好吧";
                            SRG.srecnspeech.SpeakAsync("好吧");
                        }
                        break;

                    case "SGM_DIAL_日文识别":
                        UI.Text = "需要切换至日文语音识别吗？";
                        SRG.srecnspeech.SpeakAsync("需要切换至日文语音识别吗？");
                        break;

                    case "SGM_DIAL_日文识别_是否":
                        if (ResultName == "是的" || ResultName == "是")
                        {
                            UI.Text = "日本語に切り替えました。";
                            SRG.srejpspeech.SpeakAsync("はい、日本語に切り替えました。");
                            SwitchToJapanese();
                        }
                        else
                        {
                            UI.Text = "好吧";
                            SRG.srecnspeech.SpeakAsync("好吧");
                        }
                        break;

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    case "SGM_FUNC_CHN_时间":
                        UI.Text = DateTime.Now.ToShortTimeString();
                        DateTimeFunction.timeNow("cn");
                        break;

                    case "SGM_FUNC_CHN_日期":
                        UI.Text = DateTime.Now.ToShortDateString();
                        DateTimeFunction.dateNow("cn");
                        break;

                    case "SGM_FUNC_CHN_星期":
                        UI.Text = DateTime.Now.ToString("dddd");
                        DateTimeFunction.dayNow("cn");
                        break;

                    ///////////////////////////////////////////////////////////////////////////////////////

                    case "SGM_FUNC_CHN_控制电视":
                        SRG.srecnspeech.SpeakAsync("好的，我可以帮你控制电视机");
                        device = "TV";
                        break;
                    case "SGM_FUNC_CHN_电源":
                        SRG.srecnspeech.SpeakAsync("好的");
                        iRKit.iRkitcontrol(device, "power");
                        break;

                    case "SGM_FUNC_CHN_菜单":
                        iRKit.iRkitcontrol(device, "menu");
                        break;

                    case "SGM_FUNC_CHN_上":
                        iRKit.iRkitcontrol(device, "up");
                        break;

                    case "SGM_FUNC_CHN_下":
                        iRKit.iRkitcontrol(device, "down");
                        break;

                    case "SGM_FUNC_CHN_左":
                        iRKit.iRkitcontrol(device, "left");
                        break;

                    case "SGM_FUNC_CHN_右":
                        iRKit.iRkitcontrol(device, "right");
                        break;

                    case "SGM_FUNC_CHN_声音加":
                        iRKit.iRkitcontrol(device, "volume plus");
                        break;

                    case "SGM_FUNC_CHN_声音减":
                        iRKit.iRkitcontrol(device, "volume minus");
                        break;

                    case "SGM_FUNC_CHN_频道加":
                        iRKit.iRkitcontrol(device, "channel plus");
                        break;

                    case "SGM_FUNC_CHN_频道减":
                        iRKit.iRkitcontrol(device, "channel minus");
                        break;

                    case "SGM_FUNC_CHN_进入":
                        iRKit.iRkitcontrol(device, "enter");
                        break;

                    //case "SGM_FUNC_CHN_退出":

                    //    break;

                    ///////////////////////////////////////////////////////////////////////////////////

                    case "SGM_FUNC_问问题":
                        SRG.srecnspeech.Speak("好的， 请说");
                        recording = true;
                        speechTimer.Enabled = true;
                        Record.Recordstart();
                        //!!!!!Xunfei.Start();
                        break;


                    case "SGM_FUNC_计算":
                        SRG.srecnspeech.Speak("好的， 请说");
                        recording = true;
                        speechTimer.Enabled = true;
                        Record.Recordstart();
                        //!!!!!Xunfei.Start();
                        break;


                    case "SGM_FUNC_打电话":
                        SRG.srecnspeech.Speak("好的， 请告诉我打电话给谁？");
                        recording = true;
                        speechTimer.Enabled = true;
                        Record.Recordstart();
                        //!!!!!Xunfei.Start();
                        break;

                    case "SGM_FUNC_打电话_是否":
                        if (ResultName == "是" || ResultName == "是的")
                        {
                            SRG.srecnspeech.SpeakAsync("好的, 正在拨号");
                            Skype.MakeCall(phone_number);
                            //   SRG.LayerGrammarLoadAndUnload("SGM_FUNC_打电话_结束", "");
                        }
                        else
                        {
                            SRG.srecnspeech.Speak("好的,请重新给我号码");
                            recording = true;
                            speechTimer.Enabled = true;
                            Record.Recordstart();
                            //!!!!!Xunfei.Start();
                        }

                        break;

                    case "SGM_FUNC_打电话_结束":
                        SRG.srecnspeech.SpeakAsync("好的");
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.NEXT);
                        Thread.Sleep(100);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                        Thread.Sleep(100);
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                        Thread.Sleep(10);
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_N);
                        break;

                    case "SGM_FUNC_发简讯":
                        SRG.srecnspeech.Speak("好的， 请告诉我短信的内容以及电话号码");
                        recording = true;
                        SMSTimer.Enabled = true;
                        Record.Recordstart();
                        //!!!!!Xunfei.Start();
                        break;

                    case "SGM_FUNC_发简讯_是否":
                        if (ResultName == "是的" || ResultName == "是")
                        {
                            SRG.srecnspeech.SpeakAsync("好的， 正在发送简讯");
                            Thread.Sleep(1000);
                            Skype.SendSMS(phone_number, SMSContent);
                        }
                        else
                        {
                            SRG.srecnspeech.SpeakAsync("好的， 请重新给我指令");
                        }

                        break;

                    //////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////

                    case "SGM_FUNC_こんにちは":
                        if (ResultName == "こんにちは")
                        {
                            if (!MainWindow.DemoMode)
                            {
                                UI.Text = "こんにちは!";
                                SRG.srejpspeech.SpeakAsync("こんにちは!");
                            }
                            else
                            {
                                UI.Text = "こんにちは!";
                                SRG.srejpspeech.SpeakAsync("こんにちは!");
                            }
                        }
                        else if (ResultName == "おはようございます" || ResultName == "おはよう")
                        {
                            UI.Text = "おはようございます!";
                            SRG.srejpspeech.SpeakAsync("おはようございます!");
                        }
                        else if (ResultName == "こんばんは")
                        {
                            if (DateTime.Now.Hour > 18 || DateTime.Now.Hour < 3)
                            {
                                UI.Text = "こんばんは!";
                                SRG.srejpspeech.SpeakAsync("こんばんは!");
                            }
                            else
                            {
                                UI.Text = "今は夜じゃないよ!";
                                SRG.srejpspeech.SpeakAsync("今は夜じゃないよ!");
                            }
                        }
                        else if (ResultName == "初めまして")
                        {
                            if (Function.Vision.UserName == "Unknown")
                            {
                                UI.Text = "初めまして!どうぞよろしくお願いします。";
                                SRG.srejpspeech.SpeakAsync("初めまして!どうぞよろしくお願いします。");
                            }
                            else
                            {
                                UI.Text = "初めてじゃないでしょう？";
                                SRG.srejpspeech.SpeakAsync("初めてじゃないでしょう？");
                            }
                        }
                        else if (ResultName == "いい天気ですね")
                        {
                            UI.Text = "そうですね。いい天気ですね。";
                            SRG.srejpspeech.SpeakAsync("そうですね。いい天気ですね。");
                        }
                        break;

                    case "SGM_FUNC_中国語に切り替え":
                        UI.Text = "中国語に切り替えますか?";
                        SRG.srejpspeech.SpeakAsync("中国語に切り替えますか?");
                        break;

                    case "SGM_FUNC_中国語に切り替え_確認":
                        if (ResultName == "はい" || ResultName == "そうです")
                        {
                            UI.Text = "切换至中文";
                            SRG.srecnspeech.SpeakAsync("好的，切换至中文语音识别");
                            SwitchToChinese();
                        }
                        else
                        {
                            UI.Text = "じゃあ、そうしましょう。";
                            SRG.srejpspeech.SpeakAsync("じゃあ、そうしましょう。");
                        }
                        break;

                    case "SGM_FUNC_英語に切り替え":
                        UI.Text = "英語に切り替えますか?";
                        SRG.srejpspeech.SpeakAsync("英語に切り替えますか?");
                        break;

                    case "SGM_FUNC_英語に切り替え_確認":
                        if (ResultName == "はい" || ResultName == "そうです")
                        {
                            UI.Text = "Ok, switch to English speech recognition";
                            SRG.srespeech.SpeakAsync("Ok, switch to English speech recognition");
                            SwitchToEnglish();
                        }
                        else
                        {
                            UI.Text = "じゃあ、そうしましょう。";
                            SRG.srejpspeech.SpeakAsync("じゃあ、そうしましょう。");
                        }
                        break;

                    //  case "SGM_FUNC_天気":
                    //      UI.Text = "どこの天気ですか？";
                    //      SRG.srejpspeech.SpeakAsync("どこの天気ですか");


                    //1.Find the speech recognition Japanese website
                    //2.Get the City name by the website
                    //3.Find the website to get weather condition in Japanese language
                    //4.Get the information and send it to program
                    //5.Read the information in Japanese
                    //      break;

                    case "SGM_FUNC_時間":
                        UI.Text = "今の時間は" + DateTime.Now.Hour + "時" + DateTime.Now.Minute + "分です。";
                        if (DateTime.Now.Hour > 12)
                        {
                            int HourInAfternoon = DateTime.Now.Hour - 12;
                            SRG.srejpspeech.SpeakAsync("今の時間は 午後 " + HourInAfternoon + "時" + DateTime.Now.Minute + "分です。");
                        }
                        else
                        {
                            SRG.srejpspeech.SpeakAsync("今の時間は 午前 " + DateTime.Now.Hour + "時" + DateTime.Now.Minute + "分です。");
                        }
                        break;

                    case "SGM_FUNC_曜日":

                        //DateTimeFunction.dayNow("jp");
                        曜日 = DateTime.Now.DayOfWeek.ToString();
                        switch (曜日)
                        {
                            case "Monday":
                                曜日 = "月曜日";
                                break;

                            case "Tuesday":
                                曜日 = "火曜日";
                                break;

                            case "Wednesday":
                                曜日 = "水曜日";
                                break;

                            case "Thursday":
                                曜日 = "木曜日";
                                break;

                            case "Friday":
                                曜日 = "金曜日";
                                break;

                            case "Saturday":
                                曜日 = "土曜日";
                                break;

                            case "Sunday":
                                曜日 = "日曜日";
                                break;

                        }
                        //Speak.srejpspeech.SelectVoice("VW Misaki");
                        SRG.srejpspeech.SpeakAsync(曜日 + "です");
                        UI.Text = 曜日;


                        break;

                    case "SGM_FUNC_名前":
                        UI.Text = "私の名前は,ルース.です！";
                        SRG.srejpspeech.SpeakAsync("私の名前はルースです!");
                        break;

                    case "SGM_FUNC_自己紹介":
                        UI.Text = "自己紹介";
                        SRG.srejpspeech.SpeakAsync("私の名前はルースです。");
                        SRG.srejpspeech.SpeakAsync("ナンヤンポリテクニックと 北九州高専の共同開発によって日々機能の拡張のための研究が続けられています.");
                        SRG.srejpspeech.SpeakAsync("みなさんのお役に立つことが私の夢です");
                        break;

                    case "SGM_FUNC_ラッスンゴレライ":
                        UI.Text = "ちょとまてちょとまてお兄さんっ！";
                        SRG.srejpspeech.SpeakAsync("いやちょとまてちょとまてお兄さん　ラッスンゴレライってなんですのー　説明しろと言われましても意味わからんからできまっせーん");
                        break;

                    case "SGM_FUNC_ラッスンゴレライ_昨日の晩飯":
                        UI.Text = "ちょとまてちょとまてお兄さんっ！";
                        SRG.srejpspeech.SpeakAsync("ちょとまてちょとまてお兄さん　ラッスンゴレライって食べ物なん　晩飯言うてもジャンルは広い、肉さかな野菜どれですのー");
                        break;

                    case "SGM_FUNC_血液型":
                        UI.Text = "私に流れているのは血液ではなく電流です";
                        SRG.srejpspeech.SpeakAsync("私に流れているのは血液ではなく電流です");
                        break;

                    case "SGM_FUNC_なんか言って":
                        UI.Text = "人生は、見たり聞いたり試したり、3つの知恵でまとまっているが　多くの人は見たり聞いたりばかりで一番重要な試したりをほとんどしない";
                        SRG.srejpspeech.SpeakAsync("人生は、見たり聞いたり試したり、みっつの知恵でまとまっているが　多くの人は見たり聞いたりばかりで一番重要な試したりをほとんどしない");
                        break;

                    case "SGM_FUNC_休憩":
                        UI.Text = "休憩なんて　あの世に行けばいくらでもできるでしょ";
                        SRG.srejpspeech.SpeakAsync("休憩なんて　あの世に行けばいくらでもできるでしょ");
                        break;

                    case "SGM_FUNC_地球":
                        UI.Text = "地球は俺の遊園地だ";
                        SRG.srejpspeech.SpeakAsync("地球は俺の遊園地だ");
                        break;

                    case "SGM_FUNC_おやすみ":
                        UI.Text = "終了しますか？";
                        SRG.srejpspeech.SpeakAsync("終了しますか？");
                        break;

                    case "SGM_FUNC_おやすみ_確認":
                        if (ResultName == "はい" || ResultName == "そうです")
                        {
                            UI.Text = "おやすみなさいませ";
                            SRG.srejpspeech.Speak("おやすみなさいませ");
                            this.Close();
                        }
                        else
                        {
                            UI.Text = "かしこまりました。";
                            SRG.srejpspeech.SpeakAsync("かしこまりました。");
                        }
                        break;

                    case "SGM_FUNC_ばか":
                        UI.Text = "ごめんね";
                        Function.FaceLED.Instance.Sad();
                        Thread.Sleep(1000);
                        SRG.srejpspeech.SpeakAsync("ごめんね！");
                        break;
                    case "SGM_FUNC_日":
                        DateTimeFunction.dateNow("jp");
                        UI.Text = "こにちわ";
                        break;

                    case "SGM_FUNC_元気":
                        UI.Text = "元気です";
                        SRG.srejpspeech.SpeakAsync("元気だよう、ありがとう");
                        break;


                    /*
                     //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                        case "SGM_FUNC_Skype起動":
                                        UI.Text = "はい、あります。";
                                        SRG.srespeech.SpeakAsync("はいあります");
                                        break;

                                    case "SGM_FUNC_Skype電話":
                                        UI.Text = "どこの国にかけますか？";
                                        SRG.srespeech.SpeakAsync("どこの国にかけますか？");
                                        //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_Skype電話","");
                                        break;

                                    case "SGM_FUNC_Skype電話_国":
                                        if (ResultName == "日本")
                                        {
                                            UI.Text = "日本";
                                            country_number = "+81";
                                        }

                                        if (ResultName == "シンガポール")
                                        {
                                            UI.Text = "シンガポール";
                                            country_number = "+65";
                                        }
                                        else if (ResultName == "中国")
                                        {
                                            UI.Text = "中国";
                                            country_number = "+86";
                                        }
                                        //sre.RecognizeAsyncCancel();


                                        SRG.srespeech.SpeakAsync("番号を教えてください");
                                        //  SRG.LayerGrammarLoadAndUnload("SGM_FUNC_Skype電話_国", "");
                                        break;

                                    case "SGM_FUNC_Skype電話_電話番号":
                                        if (country_number == "+65")
                                        {
                                            if (i < 8)
                                            {
                                                if (ResultName == "backspace")
                                                {
                                                    phone_number = phone_number.Substring(0, phone_number.Length - 1);
                                                    UI.Text = phone_number;
                                                    //SkypeInterface.PhoneNumber.Text = phone_number;
                                                    i = i - 1;                     
                                                }   
                                                else if(ResultName == "いち")
                                                {
                                                    number = "1";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "に")
                                                {
                                                    number = "2";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "さん")
                                                {
                                                    number = "3";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "よん" || ResultName == "し")
                                                {
                                                    number = "4";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "ご")
                                                {
                                                    number = "5";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "ろく")
                                                {
                                                    number = "6";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "なな" || ResultName == "しち")
                                                {
                                                    number = "7";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "はち")
                                                {
                                                    number = "8";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "きゅう")
                                                {
                                                    number = "9";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "ぜろ" || ResultName == "れい")
                                                {
                                                    number = "0";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                            }
                                            else if (i == 8)
                                            {
                                                SRG.srejp.UnloadGrammar(SRG.SGM_FUNC_Skype電話_電話番号);
                                                if(ResultName == "いち")
                                                {
                                                    number = "1";

                                                }
                                                else if (ResultName == "に")
                                                {
                                                    number = "2";

                                                }
                                                else if (ResultName == "さん")
                                                {
                                                    number = "3";

                                                }
                                                else if (ResultName == "よん" || ResultName == "し")
                                                {
                                                    number = "4";

                                                }
                                                else if (ResultName == "ご")
                                                {
                                                    number = "5";

                                                }
                                                else if (ResultName == "ろく")
                                                {
                                                    number = "6";

                                                }
                                                else if (ResultName == "なな" || ResultName == "しち")
                                                {
                                                    number = "7";

                                                }
                                                else if (ResultName == "はち")
                                                {
                                                    number = "8";

                                                }
                                                else if (ResultName == "きゅう")
                                                {
                                                    number = "9";

                                                }
                                                else if (ResultName == "ぜろ" || ResultName == "れい")
                                                {
                                                    number = "0";

                                                }
                                                phone_number = phone_number + number;
                                                UI.Text = phone_number;
                                                Thread.Sleep(1000);
                                                //SRG.LayerGrammarLoadAndUnload("SGM_FUNC_Skype電話_電話番号", "");
                                                country_phone_number = country_number + phone_number;
                                                char[] code_2 = phone_number.ToCharArray();
                                                SRG.srespeech.SpeakAsync(code_2[0] + " " + code_2[1] + " " + code_2[2] + " " + code_2[3] + " " + code_2[4] + " " + code_2[5] + " " + code_2[6] + " " + code_2[7] +"に電話をかけますか?");
                                                i = 0;
                                                SRG.srejp.LoadGrammar(SRG.SGM_FUNC_Skype電話_電話番号_確認);
                                            }

                                        }

                                        else if (country_number == "+81")
                                        {
                                            if (i < 11)
                                            {
                                                if (ResultName == "backspace")
                                                {
                                                    phone_number = phone_number.Substring(0, phone_number.Length - 1);
                                                    UI.Text = phone_number;
                                                    //SkypeInterface.PhoneNumber.Text = phone_number;
                                                    i = i - 1;
                                                }
                                                else if (ResultName == "いち")
                                                {
                                                    number = "1";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "に")
                                                {
                                                    number = "2";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "さん")
                                                {
                                                    number = "3";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "よん" || ResultName == "し")
                                                {
                                                    number = "4";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "ご")
                                                {
                                                    number = "5";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "ろく")
                                                {
                                                    number = "6";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "なな" || ResultName == "しち")
                                                {
                                                    number = "7";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "はち")
                                                {
                                                    number = "8";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "きゅう")
                                                {
                                                    number = "9";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                                else if (ResultName == "ぜろ" || ResultName == "れい")
                                                {
                                                    number = "0";
                                                    phone_number = phone_number + number;
                                                    UI.Text = phone_number;
                                                    i++;
                                                }
                                            }

                                            else if (i == 11)
                                            {
                                                SRG.srejp.UnloadGrammar(SRG.SGM_FUNC_Skype電話_電話番号);
                                                if (ResultName == "いち")
                                                {
                                                    number = "1";

                                                }
                                                else if (ResultName == "に")
                                                {
                                                    number = "2";

                                                }
                                                else if (ResultName == "さん")
                                                {
                                                    number = "3";

                                                }
                                                else if (ResultName == "よん" || ResultName == "し")
                                                {
                                                    number = "4";

                                                }
                                                else if (ResultName == "ご")
                                                {
                                                    number = "5";

                                                }
                                                else if (ResultName == "ろく")
                                                {
                                                    number = "6";

                                                }
                                                else if (ResultName == "なな" || ResultName == "しち")
                                                {
                                                    number = "7";

                                                }
                                                else if (ResultName == "はち")
                                                {
                                                    number = "8";

                                                }
                                                else if (ResultName == "きゅう")
                                                {
                                                    number = "9";

                                                }
                                                else if (ResultName == "ぜろ" || ResultName == "れい")
                                                {
                                                    number = "0";

                                                }
                                                phone_number = phone_number + number;
                                                UI.Text = phone_number;
                                                Thread.Sleep(1000);
                                                //SRG.LayerGrammarLoadAndUnload("SGM_FUNC_Skype電話_電話番号", "");
                                                country_phone_number = country_number + phone_number;
                                                SRG.srespeech.SpeakAsync("この番号に電話をかけますか？");
                                                i = 0;
                                                SRG.srejp.LoadGrammar(SRG.SGM_FUNC_Skype電話_電話番号_確認);
                                            }
                                        }
                                        break;

                                    case "SGM_FUNC_Skype電話_電話番号_確認":
                                        if (ResultName == "はい" || ResultName == "そうです")
                                        {
                                            Skype.MakeCall(country_phone_number);

                                        }
                                        else if (ResultName == "いいえ" || ResultName == "ちがいます")
                                        {
                                            SRG.srespeech.SpeakAsync("かしこまりました");
                                        }
                                        break;

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    */

                    case "SGM_DIAL_HK_下午好":
                        SRG.srecanspeech.SpeakAsync("下午好");
                        break;

                    case "SGM_DIAL_HK_你叫什么名字":
                        SRG.srecanspeech.SpeakAsync("我是ruth");
                        break;

                    case "SGM_DIAL_HK_你好":
                        SRG.srecanspeech.SpeakAsync("你好！");
                        break;

                    case "SGM_DIAL_HK_早上好":
                        SRG.srecanspeech.SpeakAsync("早上好");
                        break;

                    case "SGM_DIAL_HK_晚上好":
                        SRG.srecanspeech.SpeakAsync("晚上好");
                        break;

                    case "SGM_DIAL_HK_自我介绍":
                        SRG.srecanspeech.SpeakAsync("我是ruth!你系边个？");
                        break;


                    case "SGM_FUNC_HK_时间":
                        UI.Text = DateTime.Now.ToShortTimeString();
                        DateTimeFunction.timeNow("can");
                        break;

                    case "SGM_FUNC_HK_日期":
                        UI.Text = DateTime.Now.ToShortDateString();
                        DateTimeFunction.dateNow("can");
                        break;

                    case "SGM_FUNC_HK_星期":
                        UI.Text = DateTime.Now.ToString("dddd");
                        DateTimeFunction.dayNow("can");
                        break;

                    case "SGM_FUNC_HK_关灯":
                        SRG.srecanspeech.SpeakAsync("正在关闭");
                        LightCon.control("off");
                        break;

                    case "SGM_FUNC_HK_开灯":
                        SRG.srecanspeech.SpeakAsync("正在打开");
                        LightCon.control("on");
                        break;

                    case "SGM_FUNC_HK_开收音机":
                        SRG.srecanspeech.SpeakAsync("正在打开");
                        break;
                    case "SGM_FUNC_HK_关闭收音机":
                        SRG.srecanspeech.SpeakAsync("正在关闭");
                        break;

                    case "SGM_FUNC_HK_打电话":
                        SRG.srecanspeech.SpeakAsync("好的，稍等");
                        Skype.MakeCall("97888234");
                        break;

                    case "SGM_FUNC_HK_关电视":
                        iRKit.iRkitcontrol("TV", "power");
                        SRG.srecanspeech.SpeakAsync("正在关闭");
                        break;

                    case "SGM_FUNC_HK_开电视":
                        iRKit.iRkitcontrol("TV", "power");
                        SRG.srecanspeech.SpeakAsync("正在打开");

                        break;


                    case "SGM_FUNC_HK_开风扇":
                        iRKit.iRkitcontrol("fan", "power on");
                        SRG.srecanspeech.SpeakAsync("正在打开");
                        break;

                    case "SGM_FUNC_HK_关风扇":
                        iRKit.iRkitcontrol("fan", "power on");
                        SRG.srecanspeech.SpeakAsync("正在关闭");
                        break;

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case "SGM_FUNC_HOK_日期":
                        UI.Text = DateTime.Now.ToShortDateString();
                        mp3.music("今天是3月28号");
                        //Thread.Sleep(700);                   
                        break;

                    case "SGM_FUNC_HOK_星期":
                        UI.Text = DateTime.Now.ToString("dddd");
                        DateTimeFunction.dayNow("hok");
                        break;

                    case "SGM_FUNC_HOK_开灯":
                        mp3.music("好的，稍等");
                        LightCon.control("on");
                        break;

                    case "SGM_FUNC_HOK_关灯":
                        mp3.music("好的，稍等");
                        LightCon.control("off");
                        break;

                    case "SGM_FUNC_HOK_开收音机":
                        mp3.music("好的，稍等");
                        break;

                    case "SGM_FUNC_HOK_关收音机":
                        mp3.music("好的，稍等");

                        break;

                    case "SGM_FUNC_HOK_开电视机":
                        mp3.music("好的，稍等");
                        iRKit.iRkitcontrol("TV", "power");
                        break;

                    case "SGM_FUNC_HOK_关电视机":
                        mp3.music("好的，稍等");
                        iRKit.iRkitcontrol("TV", "power");
                        break;

                    case "SGM_FUNC_HOK_开风扇":
                        iRKit.iRkitcontrol("fan", "power on");
                        mp3.music("好的，稍等");
                        break;

                    case "SGM_FUNC_HOK_关风扇":
                        iRKit.iRkitcontrol("fan", "power on");
                        mp3.music("好的，稍等");
                        break;

                        //case "SGM_FUNC_HOK_打电话":
                        //    mp3.music("好的，稍等");
                        //    Skype.MakeCall("97888234");
                        //    break;


                }
            }
        }



        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd);
        String ProcWindow_TextDocument = "SMS-WordPad";
        //String ProcWindow_robot = "vshost32.exe";

        private void SwitchWindow_TextDocument()
        {
            Process[] procs = Process.GetProcessesByName(ProcWindow_TextDocument);
            ProcessStartInfo start_TextDocument = new ProcessStartInfo();
            start_TextDocument.FileName = @"C:\test\SMS.txt";
            if (procs.Length > 0)
            {
                foreach (Process proc in procs)
                {
                    //switch to process by name
                    SwitchToThisWindow(proc.MainWindowHandle);
                }
            }
            else
            {
                Process.Start(start_TextDocument);
                Thread.Sleep(3000);
            }

        }



        public void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Function.Vision.Instance.FaceTrackingTimer.Stop();
                Function.Vision.Instance.FaceTrackingTimer = null;
            }
            catch { }
            Thread.Sleep(100);
            for (int i = 0; i < 2; i++)
            {
                Motor.RobotSleep();
                Thread.Sleep(100);
                Motor.EyesClose();
                Thread.Sleep(100);
                Motor.RightArmRest();
                Thread.Sleep(100);
                Motor.LeftArmRest();
                Thread.Sleep(100);
                try
                {
                    Function.FaceLED.Instance.blank();
                }
                catch { }
            }
            GC.Collect();
            Environment.Exit(0);
        }



        public void SwitchToChinese()
        {
            switch (language)
            {
                case "English":
                    language = "Chinese";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.sre.RecognizeAsyncCancel();
                    SRG.sre.UnloadAllGrammars();
                    SRG.ChineseGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Japanese":
                    language = "Chinese";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.srejp.RecognizeAsyncCancel();
                    SRG.srejp.UnloadAllGrammars();
                    SRG.ChineseGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Cantonese":
                    language = "Chinese";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.srecan.RecognizeAsyncCancel();
                    SRG.srecan.UnloadAllGrammars();
                    SRG.ChineseGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Hokkien":
                    language = "Chinese";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.ChineseGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;


            }
        }

        public void SwitchToJapanese()
        {
            switch (language)
            {
                case "English":
                    language = "Japanese";
                    SRG.type = "srejp";
                    Language_text.Text = language;
                    SRG.sre.RecognizeAsyncCancel();
                    SRG.sre.UnloadAllGrammars();
                    SRG.JapaneseGrammarLoad();
                    SRG.srejp.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Chinese":
                    language = "Japanese";
                    SRG.type = "srejp";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.JapaneseGrammarLoad();
                    SRG.srejp.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Cantonese":
                    language = "Japanese";
                    SRG.type = "srejp";
                    Language_text.Text = language;
                    SRG.srecan.RecognizeAsyncCancel();
                    SRG.srecan.UnloadAllGrammars();
                    SRG.JapaneseGrammarLoad();
                    SRG.srejp.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Hokkien":
                    language = "Japanese";
                    SRG.type = "srejp";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.JapaneseGrammarLoad();
                    SRG.srejp.RecognizeAsync(RecognizeMode.Multiple);
                    break;

            }
        }

        public void SwitchToEnglish()
        {
            switch (language)
            {
                case "Chinese":
                    language = "English";
                    SRG.type = "sre";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.SRGS_GrammarModels();
                    SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Japanese":
                    language = "English";
                    SRG.type = "sre";
                    Language_text.Text = language;
                    SRG.srejp.RecognizeAsyncCancel();
                    SRG.srejp.UnloadAllGrammars();
                    SRG.SRGS_GrammarModels();
                    SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Cantonese":
                    language = "English";
                    SRG.type = "sre";
                    Language_text.Text = language;
                    SRG.srecan.RecognizeAsyncCancel();
                    SRG.srecan.UnloadAllGrammars();
                    SRG.SRGS_GrammarModels();
                    SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Hokkien":
                    language = "English";
                    SRG.type = "sre";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.SRGS_GrammarModels();
                    SRG.sre.RecognizeAsync(RecognizeMode.Multiple);
                    break;
            }
        }

        public void SwitchToCantonese()
        {
            switch (language)
            {
                case "English":
                    language = "Cantonese";
                    SRG.type = "srecan";
                    Language_text.Text = language;
                    SRG.sre.RecognizeAsyncCancel();
                    SRG.sre.UnloadAllGrammars();
                    // SRG.ChineseGrammarLoad();
                    SRG.srecan.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Japanese":
                    language = "Cantonese";
                    SRG.type = "srecan";
                    Language_text.Text = language;
                    SRG.srejp.RecognizeAsyncCancel();
                    SRG.srejp.UnloadAllGrammars();

                    SRG.CantoneseGrammarLoad();
                    SRG.srecan.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Chinese":
                    language = "Cantonese";
                    SRG.type = "srecan";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.CantoneseGrammarLoad();
                    SRG.srecan.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Hokkien":
                    language = "Cantonese";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.CantoneseGrammarLoad();
                    SRG.srecan.RecognizeAsync(RecognizeMode.Multiple);
                    break;

            }
        }

        public void SwitchTohokkien()
        {
            switch (language)
            {
                case "English":
                    language = "Hokkien";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.sre.RecognizeAsyncCancel();
                    SRG.sre.UnloadAllGrammars();
                    SRG.HokkienGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Japanese":
                    language = "Hokkien";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.srejp.RecognizeAsyncCancel();
                    SRG.srejp.UnloadAllGrammars();
                    SRG.HokkienGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Chinese":
                    language = "Hokkien";
                    SRG.type = "srecan";
                    Language_text.Text = language;
                    SRG.srecn.RecognizeAsyncCancel();
                    SRG.srecn.UnloadAllGrammars();
                    SRG.HokkienGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;

                case "Cantonese":
                    language = "Hokkien";
                    SRG.type = "srecn";
                    Language_text.Text = language;
                    SRG.srecan.RecognizeAsyncCancel();
                    SRG.srecan.UnloadAllGrammars();
                    SRG.HokkienGrammarLoad();
                    SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
                    break;
            }

        }



        //private void SwitchWindow_toSocialRobotics()
        //{
        //    Process[] proc_socialrobot = Process.GetProcessesByName(ProcWindow_robot);
        //    foreach (Process proc_sb in proc_socialrobot)
        //    {
        //        SwitchToThisWindow(proc_sb.MainWindowHandle);
        //    }

        //}

        private void Xunfei_ErrorEvent(Exception e, string error)
        {
            SRG.srespeech.SpeakAsync(e.Message);
        }

        //private void ButtonRec_Click(object sender, RoutedEventArgs e)
        //{
        //    if (btnRecord.Content.ToString() == "录音")
        //    {
        //        recording = true;
        //        btnRecord.Content = "停止";
        //        Xunfei.Start();
        //    }
        //    else
        //    {
        //        recording = false;
        //        btnRecord.Content = "转换中";
        //        Xunfei.Stop();
        //        try
        //        {
        //            string c1 = "server_url=dev.voicecloud.cn,appid=556b30ff,timeout=10000";
        //            string c2 = "sub=iat,ssm=1,sch=1,auf=audio/L16;rate=16000,aue=speex,ent=sms16k,ptt=0,rst=json,rse=gb2312,nlp_version=2.0";
        //            iFlyASR asr = new iFlyASR(c1, c2);
        //            XunFei_result = asr.Audio2Txt(AppDomain.CurrentDomain.BaseDirectory + "aaa.wav");
        //            Msg.Text = XunFei_result;
        //        }

        //        catch (Exception)
        //        {
        //            Msg.Text = "无法识别";
        //        }

        //        btnRecord.Content = "录音";
        //        Thread.Sleep(500);
        //        //  JsonMy(result);
        //    }
        //}



        private void OnTimedSpeechEvent(object source, ElapsedEventArgs e)
        {
            recording = false;
            speechTimer.Enabled = false;
            Record.Recordstop();
            //!!!!!Xunfei.Stop();
            SRG.srecnspeech.Speak("正在处理，请稍等");
            //   Thread.Sleep(2000);


            this.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    string c1 = "server_url=dev.voicecloud.cn,appid=556addae,timeout=10000";
                    string c2 = "sub=iat,ssm=1,sch=1,auf=audio/L16;rate=16000,aue=speex,ent=sms16k,ptt=0,rst=json,rse=gb2312,nlp_version=2.0";
                    iFlyASR asr = new iFlyASR(c1, c2);
                    XunFei_result = asr.Audio2Txt(AppDomain.CurrentDomain.BaseDirectory + "aaa.wav");
                    Setting_Windows.XunFeiRaw.Text = XunFei_result;
                    pass = true;
                }

                catch (Exception)
                {
                    Setting_Windows.XunFeiRaw.Text = "无法识别";
                    pass = false;
                    SRG.srecnspeech.Speak("对不起， 我没有听清楚，可以重复一遍吗");
                    recording = true;
                    speechTimer.Enabled = true;
                    Record.Recordstart();
                    //!!!!!Xunfei.Start();
                }
            }));

            Thread.Sleep(500);
            if (pass == true)
            {
                if (RuleName == "SGM_FUNC_问问题" || RuleName == "SGM_FUNC_计算")
                {
                    JsonAns(XunFei_result);
                    SRG.LayerGrammarLoadAndUnload("SGM_FUNC_问问题_完成", "");
                }

                if (RuleName == "SGM_FUNC_打电话" || RuleName == "SGM_FUNC_打电话_是否")
                {
                    JsonCall(XunFei_result);
                    SRG.srecn.UnloadAllGrammars();
                    SRG.srecn.LoadGrammar(SRG.SGM_FUNC_打电话_是否);
                }
            }
        }


        private void OnTimeSMSEvent(object source, ElapsedEventArgs e)
        {
            recording = false;
            SMSTimer.Enabled = false;
            Record.Recordstop();
            //!!!!!Xunfei.Stop();
            SRG.srecnspeech.Speak("正在处理，请稍等");
            this.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    string c1 = "server_url=dev.voicecloud.cn,appid=556addae,timeout=10000";
                    string c2 = "sub=iat,ssm=1,sch=1,auf=audio/L16;rate=16000,aue=speex,ent=sms16k,ptt=0,rst=json,rse=gb2312,nlp_version=2.0";
                    iFlyASR asr = new iFlyASR(c1, c2);
                    XunFei_result = asr.Audio2Txt(AppDomain.CurrentDomain.BaseDirectory + "aaa.wav");
                    Setting_Windows.XunFeiRaw.Text = XunFei_result;
                    pass = true;
                }

                catch (Exception)
                {
                    Setting_Windows.XunFeiRaw.Text = "无法识别";
                    pass = false;
                    SRG.srecnspeech.Speak("对不起， 我没有听清楚，可以重复一遍吗");
                    recording = true;
                    SMSTimer.Enabled = true;
                    Record.Recordstop();
                    //!!!!!Xunfei.Start();
                }
            }));

            Thread.Sleep(500);

            if (pass == true)
            {
                JsonSMS(XunFei_result);
                //   SRG.LayerGrammarLoadAndUnload("SGM_FUNC_发简讯_是否", "");
            }


        }
        public struct ToJson_Ans
        {
            public string re { get; set; }
            public string operation { get; set; }
            public string service { get; set; }
            public string text { get; set; }
            public Answer answer { get; set; }
        }

        public struct Answer
        {
            public string text { get; set; }
            public string type { get; set; }
        }



        public struct ToJson_Call
        {
            public Semantic semantic { get; set; }
            public string operation { get; set; }
            public string service { get; set; }
            public string text { get; set; }
        }

        public struct Semantic
        {
            public Slots slots { get; set; }
        }

        public struct Slots
        {
            public string code { get; set; }
            public string content { get; set; }
        }


        public struct ToJson_SMS
        {
            public Semantic semantic { get; set; }
            public string operation { get; set; }
            public string service { get; set; }
            public string text { get; set; }
        }




        public void JsonAns(string ANS)
        {
            string json = ANS;
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            ToJson_Ans list = js.Deserialize<ToJson_Ans>(json);    //将json数据转化为对象类型并赋值给list
            string re = list.re;
            string operation = list.operation;
            string service = list.service;
            string text = list.text;
            string answer_text = list.answer.text;
            string answer_type = list.answer.type;
            SRG.srecnspeech.Speak(answer_text);

        }


        public void JsonCall(string ANS)
        {
            string json = ANS;
            JavaScriptSerializer js = new JavaScriptSerializer();
            ToJson_Call list = js.Deserialize<ToJson_Call>(json);
            string operation = list.operation;
            string service = list.service;
            string text = list.text;
            string code = list.semantic.slots.code;

            phone_number = code;
            char[] code_1 = code.ToCharArray();
            SRG.srecnspeech.Speak("你想打电话给 " + code_1[0] + " " + code_1[1] + " " + code_1[2] + " " + code_1[3] + " " + code_1[4] + " " + code_1[5] + " " + code_1[6] + " " + code_1[7] + " " + "吗？");
            SRG.srecn.LoadGrammar(SRG.SGM_FUNC_打电话_是否);
            SRG.srecn.RecognizeAsync(RecognizeMode.Multiple);
        }


        public void JsonSMS(string ANS)
        {
            string json = ANS;
            JavaScriptSerializer js = new JavaScriptSerializer();
            ToJson_SMS list = js.Deserialize<ToJson_SMS>(json);
            string operation = list.operation;
            string service = list.service;
            string text = list.text;
            string code = list.semantic.slots.code;
            string content = list.semantic.slots.content;

            if (content == null)
            {
                SRG.srecnspeech.Speak("对不起， 我没有听清楚短信的内容， 请重复一遍");
                recording = true;
                SMSTimer.Enabled = true;
            }
            else if (content != null)
            {
                phone_number = code;
                SMSContent = content;
                char[] code_1 = code.ToCharArray();
                SRG.srecnspeech.SpeakAsync("你想发短息给 " + code_1[0] + " " + code_1[1] + " " + code_1[2] + " " + code_1[3] + " " + code_1[4] + " " + code_1[5] + " " + code_1[6] + " " + code_1[7] + " " + "说 " + content + "" + "吗？");
            }
            SRG.srecn.LoadGrammar(SRG.SGM_FUNC_发简讯_是否);
        }


        //string temperature;
        //string condition;
        //string humidity;
        //string windspeed;
        //string town;
        string apikey = "b3c579db28aa6eec025acbaaf78c2976";
        //List<string> condition_weather = new List<string>() { };
        //List<string> highest_temperature = new List<string>() { };
        //List<string> lowest_temperature = new List<string>() { };

        string weather_condition;

        BitmapImage WeatherBit;

        public void weathericon(string icon)
        {
            WeatherBit = new BitmapImage();

            switch (icon)
            {
                #region thunderstorm
                case "200":
                    weather_condition = "there will be a thunderstorm with light rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "201":
                    weather_condition = "there will be a thunderstorm with rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "202":
                    weather_condition = "there will be a thunderstorm with heavy rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\5.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "210":
                    weather_condition = "there will be a light thunderstorm";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "211":
                    weather_condition = "there will be a thunderstorm";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "212":
                    weather_condition = "there will be a heavy thunderstorm";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "221":
                    weather_condition = "there will be a ragged thunderstorm";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "230":
                    weather_condition = "there will be a thunderstorm with light drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "231":
                    weather_condition = "there will be a thunderstorm with drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "232":
                    weather_condition = "there will be a thunderstorm with heavy drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;
                #endregion thunderstorm
                #region drizzle
                case "300":
                    weather_condition = "there will be a light intensity drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "301":
                    weather_condition = "there will be a drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "302":
                    weather_condition = "there will be a heavy intensity drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "310":
                    weather_condition = "there will be a light intensity drizzle rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "311":
                    weather_condition = "there will be a drizzle rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "312":
                    weather_condition = "there will be a heavy intensity drizzle rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "313":
                    weather_condition = "there will be a shower rain and drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "314":
                    weather_condition = "there will be a heavy shower rain and drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "321":
                    weather_condition = "there will be a shower drizzle";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;
                #endregion drizzle
                #region rain
                case "500":
                    weather_condition = "there will be a light rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "501":
                    weather_condition = "there will be a moderate rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "502":
                    weather_condition = "there will be a heavy intensity rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\5.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "503":
                    weather_condition = "there will be a very heavy rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\5.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "504":
                    weather_condition = "there will be a extreme rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\5.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "511":
                    weather_condition = "there will be a freezing rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\11.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "520":
                    weather_condition = "there will be a light intensity shower rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "521":
                    weather_condition = "there will be a shower rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "522":
                    weather_condition = "there will be a heavy intensity shower rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\5.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "531":
                    weather_condition = "there will be a ragged shower rain";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\5.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;
                #endregion rain
                #region snow
                case "600":
                    weather_condition = "there will be a light snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\11.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "601":
                    weather_condition = "there will be a snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\10.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "602":
                    weather_condition = "there will be a heavy snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\10.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "611":
                    weather_condition = "there will be a sleet";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\10.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "612":
                    weather_condition = "there will be a shower sleet";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\10.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "615":
                    weather_condition = "there will be a light rain and snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\11.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "616":
                    weather_condition = "there will be a rain and snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\11.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "620":
                    weather_condition = "there will be a light shower snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "621":
                    weather_condition = "there will be a shower snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\10.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "622":
                    weather_condition = "there will be a heavy shower snow";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\10.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;
                #endregion snow
                #region atmosphere
                case "701":
                    weather_condition = "there will be a mist";
                    break;

                case "711":
                    weather_condition = "it will be smoky";
                    break;

                case "721":
                    weather_condition = "it will be hazy haze";
                    break;

                case "731":
                    weather_condition = "there will be sand, dust whirls";
                    break;

                case "741":
                    weather_condition = "it will be foggy";
                    break;

                case "751":
                    weather_condition = "It will be sandy ";
                    break;

                case "761":
                    weather_condition = "it will be dusty";
                    break;

                case "762":
                    weather_condition = "there will be volcanic ash";
                    break;

                case "771":
                    weather_condition = "there will be squalls";
                    break;

                case "781":
                    weather_condition = "there will be a tornado";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\16.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;
                #endregion atmosphere
                #region clouds
                case "800":
                    weather_condition = "clear sky";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Sunny.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "801":
                    weather_condition = "few clouds";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Partly Cloudy.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "802":
                    weather_condition = "scattered clouds";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Partly Cloudy.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "803":
                    weather_condition = "broken clouds";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Fair.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "804":
                    weather_condition = "overcast clouds";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\Partly Cloudy.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;
                #endregion clouds
                #region extreme
                case "900":
                    weather_condition = "there will be a tornado";
                    WeatherBit.BeginInit();
                    WeatherBit.UriSource = new Uri(@"C:\test\weather-icons-set\16.png");
                    WeatherBit.EndInit();
                    WeatherIcon.Source = WeatherBit;
                    break;

                case "901":
                    weather_condition = "there will be a tropical storm";
                    break;

                case "902":
                    weather_condition = "there will be a hurricane";
                    break;

                case "903":
                    weather_condition = "the weather will be extremely cold";
                    break;

                case "904":
                    weather_condition = "the weather will be extremely hot";
                    break;

                case "905":
                    weather_condition = "the weather will be extremely windy";
                    break;

                case "906":
                    weather_condition = "there will be hail";
                    break;
                #endregion extreme
                #region additional
                case "951":
                    break;

                case "952":
                    break;

                case "953":
                    break;

                case "954":
                    break;

                case "955":
                    break;

                case "956":
                    break;

                case "957":
                    break;

                case "958":
                    break;

                case "959":
                    break;

                case "960":
                    break;

                case "961":
                    break;

                case "962":
                    break;
                    #endregion additional

            }
            WeatherIconTimer.Start();
        }
        public void getweather(string country, string day)
        {
            List<string> from_weather = new List<string>() { };
            List<string> to_weather = new List<string>() { };
            List<string> temperature = new List<string>() { };
            List<string> rain = new List<string>() { };
            List<string> symbolID = new List<string>() { };
            string rain1;
            //string query = string.Format("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" + country + "%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            //XmlDocument wData = new XmlDocument();
            //wData.Load(query);

            //XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
            //manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
            //XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");


            //temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;

            //condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;

            //humidity = channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value;

            //windspeed = channel.SelectSingleNode("yweather:wind", manager).Attributes["speed"].Value;

            //town = channel.SelectSingleNode("yweather:location", manager).Attributes["city"].Value;


            //condition_weather = new List<string>() { };
            //highest_temperature = new List<string>() { };
            //lowest_temperature = new List<string>() { };


            //var fiveDays = channel.SelectSingleNode("item").SelectNodes("yweather:forecast", manager);
            //foreach (XmlNode node in fiveDays)
            //{
            //    var text = node.Attributes["text"].Value;
            //    var high = node.Attributes["high"].Value;
            //    var low = node.Attributes["low"].Value;

            //    condition_weather.Add(text);
            //    highest_temperature.Add(high);
            //    lowest_temperature.Add(low);
            //}
            #region weather
            string query2 = String.Format("http://api.openweathermap.org/data/2.5/forecast?q=" + country + "&mode=xml&appid=" + apikey);
            XmlDocument wData = new XmlDocument();
            try
            {
                wData.Load(query2);
            }
            catch
            {
                country = "";
            }


            XmlNode channel = wData.SelectSingleNode("weatherdata");
            XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);



            try
            {
                var forecast = channel.SelectSingleNode("forecast");
                foreach (XmlNode node in forecast)
                {
                    // get the hours
                    //var from = node.Attributes["from"].Value;
                    ////var to = node.Attributes["to"].Value;
                    //from_weather.Add(from);
                    ////to_weather.Add(to);
                    //manager.AddNamespace("from", from);

                    //wind attributes
                    //var windsp = node.SelectSingleNode("windSpeed").Attributes["mps"].Value;
                    //windspeed.Add(windsp);
                    //var windcon = node.SelectSingleNode("windSpeed").Attributes["name"].Value;
                    //windcondition.Add(windcon);
                    ////humdity
                    //var humid = node.SelectSingleNode("humidity").Attributes["value"].Value;
                    //humidity.Add(humid);
                    ////cloud condition
                    //var cloudcon = node.SelectSingleNode("clouds").Attributes["value"].Value;
                    //cloud.Add(cloudcon);
                    //temperature
                    var temp = node.SelectSingleNode("temperature").Attributes["value"].Value;
                    temperature.Add(temp);

                    //check if there is rain
                    try
                    {
                        var pep = node.SelectSingleNode("precipitation").Attributes["type"].Value;
                        rain.Add(pep);
                    }

                    catch
                    {
                        rain1 = "No rain";
                        rain.Add(rain1);
                    }
                    //symbol number
                    var ID = node.SelectSingleNode("symbol").Attributes["number"].Value;
                    symbolID.Add(ID);
                }
                try
                {
                    if (day == "today" || day == "Today")
                    {

                        double temp = 0;
                        temp = Convert.ToDouble(temperature[1]);

                        if (rain[1] == "No rain")
                        {
                            weathericon(symbolID[1]);
                            SRG.srespeech.SpeakAsync("the weather condition in " + country + " for " + day + " is " + (int)temp + " degrees celsius with " + weather_condition); country = "";
                        }
                        else
                        {
                            weathericon(symbolID[1]);
                            SRG.srespeech.SpeakAsync("the weather condition in " + country + " for " + day + " is " + (int)temp + " degrees celsius and " + weather_condition); country = "";
                        }
                        WeatherText.Text = (int)temp + " °C";


                    }

                    else if (day == "tomorrow" || day == "Tomorrow")
                    {
                        double temp = 0;
                        temp = Convert.ToDouble(temperature[8]);
                        if (rain[8] == "No rain")
                        {
                            weathericon(symbolID[8]);
                            SRG.srespeech.SpeakAsync("the weather condition in " + country + " for " + day + " is " + (int)temp + " degrees celsius. with " + weather_condition); country = "";
                        }
                        else
                        {
                            weathericon(symbolID[8]);
                            SRG.srespeech.SpeakAsync("the weather condition in " + country + " for " + day + " is " + (int)temp + " degrees celsius and " + weather_condition); country = "";
                        }
                        WeatherText.Text = (int)temp + " °C";
                    }
                    else if (day == "goingout")
                    {
                        try
                        {// double temp = 0;
                         //    temp = Convert.ToDouble(temperature[0]);
                            if (rain[1] == "No rain")
                            {

                                weathericon(symbolID[0]);
                                SRG.srespeech.SpeakAsync("Bon voyage.");
                            }
                            else
                            {
                                weathericon(symbolID[0]);
                                SRG.srespeech.SpeakAsync("Bon voyage, By the way please remember to take an umbrella."); country = "";
                            }
                        }
                        catch

                        {
                            SRG.srespeech.Speak("Bon vovage!");
                        }

                    }
                    else if (day == "now")
                    {
                        double temp = 0;
                        temp = Convert.ToDouble(temperature[0]);

                        if (rain[1] == "No rain")
                        {
                            weathericon(symbolID[1]);
                            SRG.srespeech.SpeakAsync("the weather condition in " + country + " for " + day + " is " + (int)temp + " degrees celsius with " + weather_condition); country = "";
                        }
                        else
                        {
                            weathericon(symbolID[1]);
                            SRG.srespeech.SpeakAsync("the weather condition in " + country + " for " + day + " is " + (int)temp + " degrees celsius and " + weather_condition + " now."); country = "";
                        }
                        WeatherText.Text = (int)temp + " °C";
                    }
                }
                catch
                {
                    SRG.srespeech.SpeakAsync("It seems I have trouble obtaining the weather forecast for " + country + ". please try another city"); country = "";
                }
            }
            catch
            {
                if (day == "now")
                {
                    SRG.srespeech.SpeakAsync("Bon voyage!");
                }
                else
                {
                    SRG.srespeech.SpeakAsync("sorry I did not hear clearly, please try again!"); country = "";

                }
            }

            #endregion weather
            #region old weather
            //    if (day == "today")
            //    {
            //        double temp_temperature_2 = 0;
            //        temp_temperature_2 = Convert.ToDouble(temperature);
            //        temp_temperature_2 = ((temp_temperature_2 - 32) / 1.8);
            //        //  SRG.srespeech.SelectVoice("IVONA Amy");
            //        SRG.srespeech.SpeakAsync("The weather condition in " + country + ", is " + condition + ", at " + (int)temp_temperature_2 + " degrees celsius. The wind speed is " + windspeed + " miles per hour, and humidity is " + humidity + ".");
            //        switch (condition)
            //        {
            //            case "Thunder in the Vicinity":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Thunderstorms.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;


            //            case "Mostly Cloudy":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Mostly Cloudy.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Partly Cloudy":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Partly Cloudy.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Fair":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Fair.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Sunny":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Sunny.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Showers in the Vicinity":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Showers":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "PM Thunderstorms":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Thunderstorms":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Thunderstorms.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;
            //                break;

            //            case "Light Rain with Thunder":
            //                b.BeginInit();
            //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Thunderstorms.png");
            //                b.EndInit();
            //                WeatherIcon.Source = b;


            //                break;
            //        }
            //        WeatherText.Text = (int)temp_temperature_2 + " °C";


            //    }
            //    else if (day == "Tomorrow")
            //    {
            //        double temp_tfhigh = 0;
            //        double temp_tflow = 0;


            //        temp_tfhigh = Convert.ToDouble(highest_temperature[1]);
            //        temp_tflow = Convert.ToDouble(lowest_temperature[1]);

            //        temp_tfhigh = ((temp_tfhigh - 32) / 1.8);
            //        temp_tflow = ((temp_tflow - 32) / 1.8);
            //        //  SRG.srespeech.SelectVoice("IVONA Amy");
            //        SRG.srespeech.SpeakAsync("Tomorrow's weather condition in " + country + " is " + condition_weather[1] + ", with the temperature range between " + (int)temp_tfhigh + ", to " + (int)temp_tflow + " degree celsius.");
            //    }
            #endregion
        }

        //public void getweather2(string country, string day)
        //{
        //    string query = string.Format("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" + country + "%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
        //    XmlDocument wData = new XmlDocument();
        //    wData.Load(query);

        //    XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
        //    manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
        //    XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");


        //    temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;

        //    condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;

        //    humidity = channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value;

        //    windspeed = channel.SelectSingleNode("yweather:wind", manager).Attributes["speed"].Value;

        //    town = channel.SelectSingleNode("yweather:location", manager).Attributes["city"].Value;

        //    condition_weather = new List<string>() { };
        //    highest_temperature = new List<string>() { };
        //    lowest_temperature = new List<string>() { };


        //    var fiveDays = channel.SelectSingleNode("item").SelectNodes("yweather:forecast", manager);
        //    foreach (XmlNode node in fiveDays)
        //    {
        //        var text = node.Attributes["text"].Value;
        //        var high = node.Attributes["high"].Value;
        //        var low = node.Attributes["low"].Value;

        //        condition_weather.Add(text);
        //        highest_temperature.Add(high);
        //        lowest_temperature.Add(low);
        //    }

        //    if (day == "today")
        //    {
        //        double temp_temperature_2 = 0;
        //        temp_temperature_2 = Convert.ToDouble(temperature);
        //        temp_temperature_2 = ((temp_temperature_2 - 32) / 1.8);
        //        //  SRG.srespeech.SelectVoice("IVONA Amy");
        //        SRG.srespeech.SpeakAsync("The weather condition in " + country + ", is " + condition + ", at " + (int)temp_temperature_2 + " degrees celsius. The wind speed is " + windspeed + " miles per hour, and humidity is " + humidity + ".");
        //        switch (condition)
        //        {
        //            case "Thunder in the Vicinity":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Thunderstorms.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;


        //            case "Mostly Cloudy":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Mostly Cloudy.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Partly Cloudy":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Partly Cloudy.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Fair":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Fair.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Sunny":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Sunny.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Showers in the Vicinity":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Showers":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Showers.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "PM Thunderstorms":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\PM Thunderstorms.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Thunderstorms":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Thunderstorms.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;
        //                break;

        //            case "Light Rain with Thunder":
        //                b.BeginInit();
        //                b.UriSource = new Uri(@"C:\test\weather-icons-set\Thunderstorms.png");
        //                b.EndInit();
        //                WeatherIcon.Source = b;


        //                break;
        //        }
        //        WeatherText.Text = (int)temp_temperature_2 + " °C";


        //    }
        //    else if (day == "Tomorrow")
        //    {
        //        double temp_tfhigh = 0;
        //        double temp_tflow = 0;


        //        temp_tfhigh = Convert.ToDouble(highest_temperature[1]);
        //        temp_tflow = Convert.ToDouble(lowest_temperature[1]);

        //        temp_tfhigh = ((temp_tfhigh - 32) / 1.8);
        //        temp_tflow = ((temp_tflow - 32) / 1.8);
        //        //  SRG.srespeech.SelectVoice("IVONA Amy");
        //        SRG.srespeech.SpeakAsync("Tomorrow's weather condition in " + country + " is " + condition_weather[1] + ", with the temperature range between " + (int)temp_tfhigh + ", to " + (int)temp_tflow + " degree celsius.");
        //    }
        //}





        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting_Windows.Show();
        }

        private void Esc_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SwitchLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (language == "English")
            {
                SwitchToChinese();
            }
            else if (language == "Chinese")
            {
                SwitchToJapanese();
            }
            else if (language == "Japanese")
            {
                SwitchToCantonese();
            }

            else if (language == "Cantonese")
            {
                SwitchTohokkien();
            }

            else if (language == "Hokkien")
            {
                SwitchToEnglish();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            HealthWindows.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dialpad.Show();
        }


        int[] array2;
        int animal_number1;
        int animal_number2;
        int n1_animals;
        int n2_animals;
        string[] animals_game = { "cat", "horse", "frog", "chicken", "snake", "dog" };


        int leg_1 = 0;
        int leg_2 = 0;
        int leg_3 = 0;
        int total_leg_game = 0;
        int leg_answer = 0;

        public void AnimalLegCountingGame()
        {
            array2 = new int[] { 0, 1, 2, 3, 4 };
            var random = new Random();
            var total = (int)array2.
                OrderBy(digit => random.Next()).
                Select((digit, index) => digit * Math.Pow(10, index)).Sum();
            var shuffledArray = array2.OrderBy(n => random.Next()).
                ToArray();
            n1_animals = shuffledArray[1];
            n2_animals = shuffledArray[2];
            if (n1_animals == 0)
            {
                leg_1 = 4;
            }
            else if (n1_animals == 1)
            {
                leg_1 = 4;

            }
            else if (n1_animals == 2)
            {
                leg_1 = 4;

            }
            else if (n1_animals == 3)
            {
                leg_1 = 2;
            }
            else if (n1_animals == 4)
            {
                leg_1 = 0;
            }
            else if (n1_animals == 5)
            {
                leg_1 = 4;
            }
            if (n2_animals == 0)
            {
                leg_2 = 4;
            }
            else if (n2_animals == 1)
            {
                leg_2 = 4;

            }
            else if (n2_animals == 2)
            {
                leg_2 = 4;

            }
            else if (n2_animals == 3)
            {
                leg_2 = 2;
            }
            else if (n2_animals == 4)
            {
                leg_2 = 0;
            }
            else if (n2_animals == 5)
            {
                leg_2 = 4;
            }
            animal_number1 = random.Next(1, 9);
            animal_number2 = random.Next(1, 9);
            if (animal_number1 != 1)
            {
                animals_game[n1_animals] = animals_game[n1_animals] + "s";
            }
            if (animal_number2 != 1)
            {
                animals_game[n2_animals] = animals_game[n2_animals] + "s";
            }
            // flag_speak_completed = false;
            // SRG.srespeech.SelectVoice("Microsoft Zira Desktop");
            SRG.srespeech.SpeakAsync("How many legs do " + animal_number1 + " " + animals_game[n1_animals] + " and " + animal_number2 + " " + animals_game[n2_animals] + " have?");
            total_leg_game = animal_number1 * leg_1 + animal_number2 * leg_2;
            //    flag_speak_completed = false;

        }

        static Thread MotorShowYes;

        public static void ShowYes()
        {
            Function.Vision.Instance.FaceTrackingTimer.Stop();
            Function.Motor.Instance.NeckShowYes();
            Function.Vision.Instance.FaceTrackingTimer.Start();
            MotorShowYes.Abort();
            MotorShowYes = null;
        }

        static Thread MotorShowNo;

        private static void ShowNo()
        {
            Function.Vision.Instance.FaceTrackingTimer.Stop();
            Function.Motor.Instance.NeckShowNo();
            Function.Vision.Instance.FaceTrackingTimer.Start();
            MotorShowNo.Abort();
            MotorShowNo = null;
        }

        public void AnimalLegCounting_CheckAnswer(int leg_answer)
        {
            if (leg_answer == total_leg_game)
            {
                //   flag_speak_completed = false;
                //Speech.srespeech.SelectVoice("Microsoft Zira Desktop");
                SRG.srespeech.SpeakAsync("You are right, Let's try more");
                MotorShowYes = new Thread(ShowYes);
                MotorShowYes.Start();
                AnimalLegCountingGame();
            }
            else
            {
                //flag_speak_completed = false;
                //  Speech.srespeech.SelectVoice("Microsoft Zira Desktop");
                SRG.srespeech.SpeakAsync("Wrong answer. It's" + total_leg_game);
                MotorShowNo = new Thread(ShowNo);
                MotorShowNo.Start();
                Thread.Sleep(3000);
                SRG.srespeech.SpeakAsync("Don't be sad, let's do again");
                AnimalLegCountingGame();
            }
        }

        public static bool ImagineMark = false;

        private void ImagineCup_Click(object sender, RoutedEventArgs e)
        {
            Function.Vision.Instance.FaceTrackingTimer.Stop();
            SRG.sre.UnloadAllGrammars();
            ImagineMark = true;
            HelloCount = 0;
            Thread.Sleep(100);
            Motor.RobotSleep();
            Thread.Sleep(100);
            Motor.EyesClose();
            Thread.Sleep(100);
            Motor.RightArmRest();
            Thread.Sleep(100);
            Motor.LeftArmRest();
            Thread.Sleep(100);
            Function.FaceLED.Instance.blank();
            SRG.sre.LoadGrammar(SRG.SGM_FUNC_ImagineCup);
        }

        //Kinect Part Start

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.reader != null)
            {
                // Subscribe to new audio frame arrived events
                this.reader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.reader != null)
            {
                // AudioBeamFrameReader is IDisposable
                this.reader.Dispose();
                this.reader = null;
            }

            if (this.colorFrameReader != null)
            {
                // ColorFrameReder is IDisposable
                this.colorFrameReader.Dispose();
                this.colorFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        private void Reader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            AudioBeamFrameReference frameReference = e.FrameReference;
            AudioBeamFrameList frameList = frameReference.AcquireBeamFrames();

            if (frameList != null)
            {
                // AudioBeamFrameList is IDisposable
                using (frameList)
                {
                    // Only one audio beam is supported. Get the sub frame list for this beam
                    IReadOnlyList<AudioBeamSubFrame> subFrameList = frameList[0].SubFrames;

                    // Loop over all sub frames, extract audio buffer and beam information
                    foreach (AudioBeamSubFrame subFrame in subFrameList)
                    {
                        // Check if beam angle and/or confidence have changed
                        bool updateBeam = false;

                        if (subFrame.BeamAngle != this.beamAngle)
                        {
                            this.beamAngle = subFrame.BeamAngle;
                            updateBeam = true;
                        }

                        if (subFrame.BeamAngleConfidence != beamAngleConfidence)
                        {
                            beamAngleConfidence = subFrame.BeamAngleConfidence;
                            updateBeam = true;
                        }

                        if (updateBeam)
                        {
                            // Refresh display of audio beam
                            this.AudioBeamChanged();
                        }

                        // Process audio buffer
                        subFrame.CopyFrameDataToArray(this.audioBuffer);

                        for (int i = 0; i < this.audioBuffer.Length; i += BytesPerSample)
                        {
                            // Extract the 32-bit IEEE float sample from the byte array
                            float audioSample = BitConverter.ToSingle(this.audioBuffer, i);

                            this.accumulatedSquareSum += audioSample * audioSample;
                            ++this.accumulatedSampleCount;

                            if (this.accumulatedSampleCount < SamplesPerColumn)
                            {
                                continue;
                            }

                            float meanSquare = this.accumulatedSquareSum / SamplesPerColumn;

                            if (meanSquare > 1.0f)
                            {
                                // A loud audio source right next to the sensor may result in mean square values
                                // greater than 1.0. Cap it at 1.0f for display purposes.
                                meanSquare = 1.0f;
                            }

                            // Calculate energy in dB, in the range [MinEnergy, 0], where MinEnergy < 0
                            float energy = MinEnergy;

                            if (meanSquare > 0)
                            {
                                energy = (float)(10.0 * Math.Log10(meanSquare));
                            }

                            lock (this.energyLock)
                            {
                                // Normalize values to the range [0, 1] for display
                                this.energy[this.energyIndex] = (MinEnergy - energy) / MinEnergy;
                                this.energyIndex = (this.energyIndex + 1) % this.energy.Length;
                                ++this.newEnergyAvailable;
                            }

                            this.accumulatedSquareSum = 0;
                            this.accumulatedSampleCount = 0;
                        }
                    }
                }
            }
        }

        private void AudioBeamChanged()
        {
            // Convert from radians to degrees for display purposes
            beamAngleInDeg = this.beamAngle * 180.0f / (float)Math.PI;

            // Display new numerical values

            //beamAngleInDeg;
            //beamAngleConfidence;
        }

        //Read News Timer Part Start
        public void ReadNewsTimer_TimeUp(object sender, EventArgs e)
        {
            ReadNewsTimer.Stop();
            SRG.srespeech.SpeakAsync("Do you want me to continue?");
            SRG.sre.LoadGrammar(SRG.SGM_ContinueReadNews_YesNo);
            GrammarTimer.Start();
        }
        //Read News Timer Part End

        //Grammar Timer Part Start
        public void GrammarTimer_TimeUp(object sender, EventArgs e)
        {
            GrammarTimer.Stop();
            if (SRG.SGM_ContinueReadNews_YesNo.Loaded)
            {
                SRG.sre.UnloadGrammar(SRG.SGM_ContinueReadNews_YesNo);
                SRG.srespeech.SpeakAsync("I will stop reading news.");
                ReadNewsFunction.flag_StartNewsReading = false;
                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_NextNews);
                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_StopReadNews);
                SRG.sre.LoadGrammar(SRG.SGM_FUNC_ReadNews);
            }
            if (SRG.SGM_DIAL_SwitchLanguageToChinese_YesNo.Loaded)
            {
                SRG.sre.UnloadGrammar(SRG.SGM_DIAL_SwitchLanguageToChinese_YesNo);
                SRG.sre.LoadGrammar(SRG.SGM_DIAL_SwitchLanguageToChinese);
                UI.Text = "Ok, nevermind";
                SRG.srespeech.SpeakAsync("Ok, never mind");
            }
            if (SRG.SGM_DIAL_SwitchLanguageToJapanese_YesNo.Loaded)
            {
                SRG.sre.UnloadGrammar(SRG.SGM_DIAL_SwitchLanguageToJapanese_YesNo);
                SRG.sre.LoadGrammar(SRG.SGM_DIAL_SwitchLanguageToJapanese);
                UI.Text = "Ok, nevermind";
                SRG.srespeech.SpeakAsync("Ok, never mind");
            }
            if (SRG.SGM_DIAL_Sleep_YesNo.Loaded)
            {
                SRG.sre.UnloadGrammar(SRG.SGM_DIAL_Sleep_YesNo);
                SRG.sre.LoadGrammar(SRG.SGM_DIAL_Sleep);
                SRG.srespeech.SpeakAsync("Ok, never mind");
            }
            if (SRG.SGM_FUNC_RegisterMode_YesNo.Loaded)
            {
                SRG.sre.UnloadGrammar(SRG.SGM_FUNC_RegisterMode_YesNo);
                SRG.srespeech.SpeakAsync("Ok, never mind");
                Function.Vision.Instance.RegisterModeQuit();
                Function.Vision.RegisterModeSwitch = false;
            }
        }
        //Grammar Timer Part End

        public void WeatherIconTimer_TimeUp(object sender, EventArgs e)
        {
            WeatherIconTimer.Stop();
            WeatherIcon.Source = null;
            WeatherBit = null;
            WeatherText.Text = null;
        }

        //ProjectOxford Part Start
        private async Task<Emotion[]> UploadAndDetecEmotion(string imageFilePath)
        {
            string subscriptionKey = "662c614f4eba4df2906931f6c11b2746";

            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(subscriptionKey);
            try
            {
                Emotion[] emotionResult;
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    emotionResult = await emotionServiceClient.RecognizeAsync(imageFileStream);
                    return emotionResult;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async void ProjectOxford(object sender, EventArgs e)
        {
            if (colorBitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(this.colorBitmap));

                string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = @"C:\test\Photo";

                string path = Path.Combine(myPhotos, "KinectScreenshot-Color" + PictureMark + ".jpg");

                PictureMark++;

                if (PictureMark >= 5)
                {
                    File.Delete(@"C:\test\Photo\KinectScreenshot-Color" + (PictureMark - 5) + ".jpg");
                }

                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
                catch (IOException)
                {

                }
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                string imageFilePath = path;
                Uri fileUri = new Uri(imageFilePath);

                BitmapImage bitmapSource = new BitmapImage();
                bitmapSource.BeginInit();
                bitmapSource.CacheOption = BitmapCacheOption.None;
                bitmapSource.UriSource = fileUri;
                bitmapSource.EndInit();
                Emotion[] emotionResult = await UploadAndDetecEmotion(path);



                if (emotionResult != null)
                {
                    EmotionResultDisplay[] resultDisplay = new EmotionResultDisplay[8];
                    for (int i = 0; i < emotionResult.Length; i++)
                    {
                        Emotion emotion = emotionResult[i];
                        resultDisplay[0] = new EmotionResultDisplay { EmotionString = "Anger", Score = emotion.Scores.Anger };
                        resultDisplay[1] = new EmotionResultDisplay { EmotionString = "Contempt", Score = emotion.Scores.Contempt };
                        resultDisplay[2] = new EmotionResultDisplay { EmotionString = "Disgust", Score = emotion.Scores.Disgust };
                        resultDisplay[3] = new EmotionResultDisplay { EmotionString = "Fear", Score = emotion.Scores.Fear };
                        resultDisplay[4] = new EmotionResultDisplay { EmotionString = "Happiness", Score = emotion.Scores.Happiness };
                        resultDisplay[5] = new EmotionResultDisplay { EmotionString = "Neutral", Score = emotion.Scores.Neutral };
                        resultDisplay[6] = new EmotionResultDisplay { EmotionString = "Sadness", Score = emotion.Scores.Sadness };
                        resultDisplay[7] = new EmotionResultDisplay { EmotionString = "Surprise", Score = emotion.Scores.Surprise };

                        Array.Sort(resultDisplay, delegate (EmotionResultDisplay result1, EmotionResultDisplay result2)
                        {
                            return ((result1.Score == result2.Score) ? 0 : ((result1.Score < result2.Score) ? 1 : -1));
                        });

                        StackPanel itemPanel = new StackPanel();
                        itemPanel.Orientation = Orientation.Horizontal;

                        String[] emotionStrings = new String[3];
                        for (int j = 0; j < 3; j++)
                        {
                            StackPanel emotionPanel = new StackPanel();
                            emotionPanel.Orientation = Orientation.Vertical;

                            itemPanel.Children.Add(emotionPanel);
                            emotionStrings[j] = resultDisplay[j].EmotionString + ":" + resultDisplay[j].Score.ToString("0.000000"); ;
                        }
                        string FinalEmotion = null;
                        if (resultDisplay[0].EmotionString == "Neutral" && resultDisplay[0].Score < 0.6 && resultDisplay[1].EmotionString != "Happiness")//if too sensitive decrease the number otherwise increase
                        {
                            FinalEmotion = resultDisplay[1].EmotionString;
                        }
                        else
                        {
                            FinalEmotion = resultDisplay[0].EmotionString;
                        }
                        FacialExpResponse(FinalEmotion);
                        //_resultListBox.ItemsSource = null;
                        //_resultListBox.ItemsSource = itemSource;
                    }
                }
            }
        }

        string LastEmotion = null;
        private void FacialExpResponse(string ExpMark)
        {
            if (ExpMark != LastEmotion && Function.Vision.WakeUp)
            {
                switch (ExpMark)
                {
                    //case "Happiness":
                    //    Function.FaceLED.Instance.Happy();
                    //    Function.Motor.Instance.EyesBlink();
                    //    Thread.Sleep(500);
                    //    Function.Motor.Instance.EyesBlink();
                    //    Thread.Sleep(500);
                    //    switch (language)
                    //    {
                    //        case "English":
                    //            SRG.srespeech.SpeakAsync("You look so happy today.");
                    //            break;
                    //        case "Chinese":
                    //            SRG.srecnspeech.SpeakAsync("你看起来好像很开心。发生了什么吗?");
                    //            break;
                    //        case "Japanese":
                    //            SRG.srejpspeech.SpeakAsync("あなたは今すごく幸せそうに見えます。何が起こったか?");
                    //            break;
                    //    }
                    //    break;

                    case "Surprise":
                        Function.FaceLED.Instance.Smile();
                        Function.Motor.Instance.EyesBlink();
                        Thread.Sleep(500);
                        Function.Motor.Instance.EyesBlink();
                        Thread.Sleep(500);
                        switch (language)
                        {
                            case "English":
                                //SRG.srespeech.SpeakAsync("Surprise!!!");
                                break;
                            case "Chinese":
                                SRG.srecnspeech.SpeakAsync("哈哈?");
                                break;
                            case "Japanese":
                                SRG.srejpspeech.SpeakAsync("びっくりしたか?");
                                break;
                        }
                        break;

                    case "Anger":
                        Function.FaceLED.Instance.Fear();
                        Thread.Sleep(1000);
                        switch (language)
                        {
                            case "English":
                                SRG.srespeech.SpeakAsync("Relax, please relax!");
                                break;
                            case "Chinese":
                                SRG.srecnspeech.SpeakAsync("我觉得你应该放轻松！");
                                break;
                            case "Japanese":
                                SRG.srejpspeech.SpeakAsync("怖い顔ですね。");
                                break;
                        }
                        break;

                    case "Sadness":
                        Function.FaceLED.Instance.Surprise();
                        Thread.Sleep(1000);
                        switch (language)
                        {
                            case "English":
                                SRG.srespeech.SpeakAsync("You look upset now.");//, maybe I can tell a joke to you to make you happy?
                                break;
                            case "Chinese":
                                SRG.srecnspeech.SpeakAsync("你看起来似乎有些不开心呢 呵呵");
                                break;
                            case "Japanese":
                                SRG.srejpspeech.SpeakAsync("何が…起こりましたか？");
                                break;
                        }
                        break;

                    case "Contempt"://鄙视
                        Function.FaceLED.Instance.Angry();
                        Function.Motor.Instance.EyesHalfClose();
                        Thread.Sleep(1000);
                        switch (language)
                        {
                            case "English":
                                SRG.srespeech.SpeakAsync("Why?");
                                break;
                            case "Chinese":
                                SRG.srecnspeech.SpeakAsync("为什么要露出这样的表情");
                                break;
                            case "Japanese":
                                SRG.srejpspeech.SpeakAsync("何を！");
                                break;
                        }
                        break;

                    case "Disgust":
                        Function.FaceLED.Instance.Disgust();
                        Function.Motor.Instance.EyesHalfClose();
                        Thread.Sleep(1000);
                        switch (language)
                        {
                            case "English":
                                SRG.srespeech.SpeakAsync("Hey!");
                                break;
                            case "Chinese":
                                SRG.srecnspeech.SpeakAsync("你肚子痛吗？");
                                break;
                            case "Japanese":
                                SRG.srejpspeech.SpeakAsync("気持ち悪い。");
                                break;
                        }
                        break;

                    case "Fear":
                        Function.FaceLED.Instance.Fear();
                        Function.Motor.Instance.EyesHalfClose();
                        Thread.Sleep(1000);
                        switch (language)
                        {
                            case "English":
                                SRG.srespeech.SpeakAsync("Don't be scared. Let's play a game.");
                                break;
                            case "Chinese":
                                SRG.srecnspeech.SpeakAsync("不要怕 不要慌 我不会突然爆炸。");
                                break;
                            case "Japanese":
                                SRG.srejpspeech.SpeakAsync("大丈夫だから");
                                break;
                        }
                        break;

                    default:
                        break;

                }
            }
            if (ExpMark != "Neutral")
            {
                LastEmotion = ExpMark;
            }
        }
        //ProjectOxford Part End

        public string KinectDetectedWords = null;
        static readonly string VisionApiKey = "b500fc94c69e47d99e08ac15a82838d2";
        public async void KinectTakePhoto()
        {
            if (colorBitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(this.colorBitmap));

                string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = @"C:\test\Photoes\";

                string path = Path.Combine(myPhotos, "KinectScreenshot-Color" + PictureMark + ".jpg");

                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
                catch (IOException)
                {

                }
                Thread.Sleep(100);
                Bitmap bitmap1;
                bitmap1 = (Bitmap)Bitmap.FromFile(@"C:\test\Photoes\KinectScreenshot-Color" + PictureMark + ".jpg");
                bitmap1.RotateFlip(RotateFlipType.Rotate180FlipY);
                bitmap1.Save(@"C:\test\Photoes\KinectScreenshot-Color" + PictureMark + ".jpg");

                PictureMark++;

                if (PictureMark >= 5)
                {
                    File.Delete(@"C:\test\Photoes\KinectScreenshot-Color" + (PictureMark - 5) + ".jpg");
                }

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", VisionApiKey);

                // Request parameters
                queryString["language"] = "unk";
                queryString["detectOrientation "] = "true";
                var uri = "https://api.projectoxford.ai/vision/v1.0/ocr?" + queryString;



                string absolutePath = Path.GetFullPath(path);
                HttpResponseMessage response;

                #region Image

                BitmapImage myBitmapImage = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(absolutePath);

                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                //set image source


                var imageBytes = File.ReadAllBytes(absolutePath);

                #endregion Image

                using (var content = new ByteArrayContent(imageBytes))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);

                    var a = response.IsSuccessStatusCode;
                    if (a == true)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        string[] SeperateStringByLines = Regex.Split(result, "lines\":");

                        try
                        {
                            string data = SeperateStringByLines[1];
                            string[] seperatedLines = Regex.Split(data, "}]}");
                            int length = seperatedLines.Length;
                            for (int s = 0; s < length - 2; s++)
                            {
                                string temp = seperatedLines[s];//temporary variable

                                string[] seperateByWords = Regex.Split(temp, "\"text\"");
                                int lengthOfSeperateByWords = seperateByWords.Length;


                                for (int j = 1; j <= lengthOfSeperateByWords - 1; j++)
                                {
                                    int StartOfpersonId = seperateByWords[j].IndexOf(":\"");
                                    int EndOfpersonId = seperateByWords[j].IndexOf("\"", StartOfpersonId + 2 + 1);
                                    var x = seperateByWords[j].Substring(StartOfpersonId + 2, EndOfpersonId - StartOfpersonId - 2);
                                    KinectDetectedWords = KinectDetectedWords + " " + x;
                                }
                                KinectDetectedWords = KinectDetectedWords + "\n";
                            }
                            SRG.srespeech.SpeakAsync(KinectDetectedWords);
                            //SRG.srejpspeech.SpeakAsync(KinectDetectedWords);
                        }
                        catch
                        {
                            SRG.srespeech.SpeakAsync("Sorry, I cannot see clearly.");
                        }
                    }
                    else MessageBox.Show("OCR Error.\n" + response.ReasonPhrase);
                }

            }
        }
        //Kinect Part End

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (Function.Vision.RegisterModeSwitch)
            {
                Function.Vision.RegisterModeSwitch = false;
                SRG.srespeech.SpeakAsync("Exiting the register mode.");
            }
            else
            {
                Function.Vision.RegisterModeSwitch = true;
                SRG.srespeech.SpeakAsync("Entering the register mode.");
            }
        }
        private void Free_Click(object sender, RoutedEventArgs e)
        {
            Motor.FreeMode();
        }



        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //Motor.ArmInitialize();
        //Motor.TurnLeft();
        //SRG.srespeech.Speak("The School of Engineering offers an exciting range of quality engineering");

        //Motor.TurnRight();
        //SRG.srespeech.Speak("science and technology courses that is designed for your success");

        //Motor.TurnLeft();
        //SRG.srespeech.Speak("Learn and grow with our innovation-based curriculum and extensive industry partnerships");


        //Motor.TurnRight();
        //SRG.srespeech.Speak("Go beyond the classroom to gain a well-rounded and highly relevant education!");

        //Motor.TurnLeft();
        //SRG.srespeech.Speak("Experience a vibrant student life and learning experiences full of endless possibilities");

        //Motor.TurnRight();
        //SRG.srespeech.Speak("as we prepare you for both exciting careers and higher studies at reputable local and overseas universities");

        //Motor.LeftArmRest();
        //Motor.RightArmRest();
        //}


        private void loadxml()
        {

            try
            {

                string MyXMLDataPath = @"C:\test\ReminderData.xml";
                XmlDocument MyXmlData = new XmlDocument();
                MyXmlData.Load(MyXMLDataPath);
                XmlNode RootNode = MyXmlData.SelectSingleNode("Reminders");//The first node（SelectSingleNode）：the root node of this xml file
                XmlNodeList FirstLevelNodeList = RootNode.ChildNodes;
                foreach (XmlNode Node in FirstLevelNodeList)
                {
                    try
                    {
                        XmlNode SecondLevelNode1 = Node.ChildNodes[0];
                        XmlNode SecondLevelNode2 = Node.ChildNodes[1];//Get the child node of the root node
                        XmlNode SecondLevelNode3 = Node.ChildNodes[2];
                        XmlNode SecondLevelNode4 = Node.ChildNodes[3];
                        if (Int32.Parse(SecondLevelNode2.InnerText) >= DateTime.Now.Hour)
                        {
                            EventTime = Convert.ToDateTime(SecondLevelNode4.InnerText.ToString());
                            reminder_task = SecondLevelNode1.InnerText.ToString();
                            substringminute = Convert.ToInt32(SecondLevelNode3.InnerText.ToString());
                        }
                    }
                    catch
                    {

                    }

                }
                MyXmlData.Save(MyXMLDataPath);

            }
            catch
            {
                Environment.Exit(0);
            }
        }

        private void xmlmaker()
        {
            try
            {
                if (reminder_task != "")
                {
                    string MyXMLDataPath = @"C:\test\ReminderData.xml";
                    XmlDocument MyXmlData = new XmlDocument();
                    MyXmlData.Load(MyXMLDataPath);
                    XmlNode RootNode = MyXmlData.SelectSingleNode("Reminders");
                    XmlElement newElement1 = MyXmlData.CreateElement("Reminder");
                    RootNode.AppendChild(newElement1);

                    XmlElement newElementChild1 = MyXmlData.CreateElement("Event");
                    newElementChild1.InnerText = reminder_task;
                    newElement1.AppendChild(newElementChild1);

                    XmlElement newElementChild2 = MyXmlData.CreateElement("SubHour");
                    newElementChild2.InnerText = subhour;
                    newElement1.AppendChild(newElementChild2);

                    XmlElement newElementChild3 = MyXmlData.CreateElement("SubMin");
                    newElementChild3.InnerText = subminute;
                    newElement1.AppendChild(newElementChild3);

                    XmlElement newElementChild4 = MyXmlData.CreateElement("Time");
                    newElementChild4.InnerText = remind_time;
                    newElement1.AppendChild(newElementChild4);

                    MyXmlData.Save(MyXMLDataPath);
                }
                else
                {
                    SRG.srespeech.SpeakAsync("Sorry I don't understand, can you repeat again?");
                }


            }
            catch
            {

            }

        }

        private void setalarm()
        {
            this.alarmTime = EventTime;
            if (alarmThread != null)
            {
                if (alarmThread.IsAlive)
                {
                    alarmThread.Abort();
                }
            }

            alarmThread = new Thread(new ThreadStart(alarmLoop));
            alarmThread.Start();
            //AlarmStatusLabel.Text = "Alarm set for " + this.alarmTime.ToString("HH:mm");

        }


        private void alarmLoop()
        {
            while (DateTime.Now < alarmTime)
            {
                ;
            }

            Dispatcher.BeginInvoke(new Action(delegate
            {

                if (DateTime.Now.Minute == substringminute)
                {
                    SRG.srespeech.SpeakAsync("excuse me sir. It's time for you to " + reminder_task);
                }
                else if (DateTime.Now.Minute > substringminute)
                {
                    if (reminder_task != "")
                    {
                        Motor.MotorInitialize();
                        SRG.srespeech.SpeakAsync("excuse me sir. You have missed your appointment to " + reminder_task);
                    }
                }
                string MyXMLDataPath = @"C:\test\ReminderData.xml";
                XmlDocument MyXmlData = new XmlDocument();
                MyXmlData.Load(MyXMLDataPath);
                XmlNode RootNode = MyXmlData.SelectSingleNode("Reminders");
                RootNode.RemoveAll();

                MyXmlData.Save(MyXMLDataPath);


            }));

        }

        private void WakeUp_Click(object sender, RoutedEventArgs e)
        {
            if (!Function.Vision.WakeUp)
            {
                Function.Vision.WakeUp = true;
                WakeUp.Content = "Go Silence";
            }
            else
            {
                Function.Vision.WakeUp = false;
                WakeUp.Content = "Wake Up";
            }
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            KinectDetectedWords = null;
            KinectTakePhoto();
        }
    }
}