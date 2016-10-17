using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.Synthesis;
using System.Windows;
using System.Threading;
using WindowsInput;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Globalization;
using WindowsMicrophoneMuteLibrary;

namespace SocialRobot.Function
{
    public class Speech_Rcognition_Grammar
    {
        //WindowsMicrophoneMuteLibrary.WindowsMicMute micMute = new WindowsMicrophoneMuteLibrary.WindowsMicMute();
        //micMute.MuteMic();
        //micMute.UnMuteMic();

        public SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        public SpeechRecognitionEngine srecn = new SpeechRecognitionEngine();
        public SpeechSynthesizer srespeech = new SpeechSynthesizer();
        public SpeechSynthesizer srecnspeech = new SpeechSynthesizer();
        public SpeechRecognitionEngine srejp = new SpeechRecognitionEngine();
        public SpeechSynthesizer srejpspeech = new SpeechSynthesizer();
        public SpeechRecognitionEngine srecan = new SpeechRecognitionEngine();
        public SpeechSynthesizer srecanspeech = new SpeechSynthesizer();

        public Grammar SGM_FUNC_ReadNews;
        public Grammar SGM_DIAL_AskRobotNameAns;

        public Grammar SGM_FUNC_LanguageOption;
        public Grammar SGM_FUNC_NextNews;
        public Grammar SGM_FUNC_StopReadNews;
        public Grammar SGM_ContinueReadNews_YesNo;

        public Grammar SGM_FUNC_AskWhatTimeNow;
        public Grammar SGM_FUNC_AskWhatDayIsToday;
        public Grammar SGM_FUNC_AskWhatDateIsToday;

        public Grammar SGM_FUNC_TellJokes;

        public Grammar SGM_FUNC_AskCountdown;
        public Grammar SGM_FUNC_Countdown;
        public Grammar SGM_FUNC_CountdownYesNo;

        public Grammar SGM_FUNC_AskTodayCustomWeather;
        public Grammar SGM_FUNC_AskTomorrowCustomWeather;

        //Grammar SGM_DIAL_AskPresidentOrPrimeMinister;
        public Grammar SGM_DIAL_AskCountryPresidentOrPrimeMinister;

        public Grammar SGM_FUNC_COMPLEX_SetReminder;
        public Grammar SGM_FUNC_COMPLEX_SetReminderYesNo;

        public Grammar SGM_FUNC_AskSkypeFunction;
        public Grammar SGM_FUNC_SkypeCall;
        public Grammar SGM_FUNC_SkypePhoneCall;
        public Grammar SGM_FUNC_SkypeCall_Choice;
        public Grammar SGM_FUNC_SkypeCall_PhoneCall_Country;
        public Grammar SGM_FUNC_SkypeCall_PhoneNumber;
        public Grammar SGM_FUNC_SkypeCall_PhoneCall_YesNo;

        //public Grammar SGM_FUNC_SendMessage;
        public Grammar SGM_FUNC_SendMessageOption;
        public Grammar SGM_FUNC_SendSMS;
        public Grammar SGM_FUNC_SMSPhoneNumber;
        public Grammar SGM_FUNC_SMSCountry;
        public Grammar SGM_FUNC_SendSMSYesOrNo;
        public Grammar SGM_FUNC_SpeechToTextMessageFinished;

        public Grammar SGM_FUNC_ControlTV;
        public Grammar SGM_FUNC_ControlProjector;
        public Grammar SGM_FUNC_PowerOnOffTV;
        public Grammar SGM_FUNC_ChangeInputTV;
        public Grammar SGM_FUNC_MuteTV;
        public Grammar SGM_FUNC_upTV;
        public Grammar SGM_FUNC_downTV;
        public Grammar SGM_FUNC_leftTV;
        public Grammar SGM_FUNC_rightTV;
        public Grammar SGM_FUNC_enterTV;
        public Grammar SGM_FUNC_VolumePlusTV;
        public Grammar SGM_FUNC_VolumeMinusTV;
        public Grammar SGM_FUNC_ChannelPlusTV;
        public Grammar SGM_FUNC_ChannelMinusTV;
        public Grammar SGM_FUNC_MenuTV;

        public Grammar SGM_FUNC_ControlRadio;
        public Grammar SGM_FUNC_NextRadio;
        public Grammar SGM_FUNC_PreviousRadio;
        public Grammar SGM_FUNC_VolumeDownRadio;
        public Grammar SGM_FUNC_VolumeUpRadio;


        public Grammar SGM_FUNC_ControlFan;
        public Grammar SGM_FUNC_PowerOnOffFan;
        public Grammar SGM_FUNC_Speed;
        public Grammar SGM_FUNC_Timer;
        public Grammar SGM_FUNC_onUpDown;
        public Grammar SGM_FUNC_onLeftRightFan;


        public Grammar SGM_FUNC_AskForRadioFunction;
        public Grammar SGM_FUNC_PowerOnOffRadio;
        public Grammar SGM_FUNC_StartRadioStaion;
        public Grammar SGM_FUNC_StartRadioStationYesNo;
        public Grammar SGM_FUNC_StopRadioStation;
        public Grammar SGM_FUNC_StopRadioStationYesNo;
        public Grammar SGM_FUNC_GoingOut;

        public Grammar SGM_FUNC_PowerOnLight;
        public Grammar SGM_FUNC_PowerOffLight;

        public Grammar SGM_DIAL_SayHello;
        public Grammar SGM_DIAL_AskIntroduction;
        public Grammar SGM_DIAL_AskRobotHowAreYou;
        public Grammar SGM_DIAL_NiceToMeetYou;
        public Grammar SGM_DIAL_AskWhatIsSocialRobot;
        public Grammar SGM_DIAL_AskWhoDesign;
        public Grammar SGM_DIAL_GoodBye;
        public Grammar SGM_DIAL_Greeting;
        public Grammar SGM_DIAL_ThankYou;
        public Grammar SGM_DIAL_Scold;
        public Grammar SGM_DIAL_Compliment;
        public Grammar SGM_DIAL_AskRobotName;
        public Grammar SGM_DIAL_SwitchLanguageToChinese;
        public Grammar SGM_DIAL_SwitchLanguageToChinese_YesNo;
        public Grammar SGM_DIAL_SwitchLanguageToJapanese;
        public Grammar SGM_DIAL_SwitchLanguageToJapanese_YesNo;
        public Grammar SGM_DIAL_LookAtMe;
        public Grammar SGM_DIAL_Sleep;
        public Grammar SGM_DIAL_Sleep_YesNo;

        public Grammar SGM_FUNC_Char;
        public Grammar SGM_FUNC_RegisterMode_YesNo;

        public Grammar SGM_DIAL_Music;

        public Grammar SGM_FUNC_SkypePhoneCallFinished;

        public Grammar SGM_DIAL_Raise_Right_Hand;
        public Grammar SGM_DIAL_Raise_Left_Hand;

        Grammar SGM_GAME_PlayAnimalLegCounting;
        Grammar SGM_GAME_PlayAnimalLegCountingYesNo;
        Grammar SGM_GAME_AnimalLegCountingAnswer;
        Grammar SGM_GAME_AnimalLegCountingQuestionRepeat;
        Grammar SGM_GAME_AnimalLegCountingAnswerDontKnow;
        Grammar SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo;
        Grammar SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo;

        Grammar SGM_FUNC_AskCanPlayMusicOrSongNot;
        Grammar SGM_FUNC_AskWhatMusicOrSongGenreAvailable;
        Grammar SGM_FUNC_StartMusicOrSong;
        Grammar SGM_FUNC_CHANGE_SGM_FUNC_StartMusicOrSongGenreYesNo;
        // Grammar SGM_FUNC_StartMusicOrSong;
        Grammar SGM_FUNC_ConfirmSongName;
        Grammar SGM_FUNC_CONT_SGM_FUNC_StartMusicOrSongGenreYesNo;
        Grammar SGM_FUNC_FindSong;
        Grammar SGM_FUNC_FindSongYesNo;
        Grammar SGM_FUNC_NEXT_SGM_FUNC_StartMusicOrSongGenreYesNo;
        Grammar SGM_FUNC_PAUSE_SGM_FUNC_StartMusicOrSongGenreYesNo;
        Grammar SGM_FUNC_StartMusicOrSongGenreYesNo;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_alternative;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_blues;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_classical;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_country;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_dance;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_emo;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_folk;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_indie;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_jazz;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_latin;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_pop;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_sixties;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_soul;
        Grammar SGM_FUNC_StartOrSelectMusicOrSongGenre_twothousands;
        Grammar SGM_FUNC_STOP_SGM_FUNC_StartMusicOrSongGenreYesNo;
        Grammar SGM_DIAL_Dance;

        //Imagine Cup
        public Grammar SGM_FUNC_ImagineCup;
        public Grammar SGM_FUNC_Call;
        public Grammar SGM_FUNC_CallPerson;
        public Grammar SGM_DIAL_Emotion;
        public Grammar SGM_DIAL_Help;

        Grammar SGM_FUNC_CHN_时间;
        Grammar SGM_FUNC_CHN_日期;
        Grammar SGM_FUNC_CHN_星期;


        Grammar SGM_DIAL_谢谢;
        Grammar SGM_DIAL_你好;
        Grammar SGM_DIAL_你叫什么名字;
        Grammar SGM_DIAL_早上好;
        Grammar SGM_DIAL_中午好;
        Grammar SGM_DIAL_下午好;
        Grammar SGM_DIAL_晚上好;
        Grammar SGM_DIAL_自我介绍;
        Grammar SGM_DIAL_谁设计了你;
        Grammar SGM_DIAL_功能;
        Grammar SGM_DIAL_英文识别;
        Grammar SGM_DIAL_英文识别_是否;
        Grammar SGM_DIAL_日文识别;
        Grammar SGM_DIAL_日文识别_是否;

        public Grammar SGM_FUNC_CHN_控制电视;
        public Grammar SGM_FUNC_CHN_电源;
        public Grammar SGM_FUNC_CHN_菜单;
        public Grammar SGM_FUNC_CHN_上;
        public Grammar SGM_FUNC_CHN_下;
        public Grammar SGM_FUNC_CHN_左;
        public Grammar SGM_FUNC_CHN_右;
        public Grammar SGM_FUNC_CHN_声音加;
        public Grammar SGM_FUNC_CHN_声音减;
        public Grammar SGM_FUNC_CHN_频道加;
        public Grammar SGM_FUNC_CHN_频道减;
        public Grammar SGM_FUNC_CHN_进入;
        public Grammar SGM_FUNC_CHN_退出;

        Grammar SGM_FUNC_发简讯;
        public Grammar SGM_FUNC_发简讯_是否;
        Grammar SGM_FUNC_打电话;
        public Grammar SGM_FUNC_打电话_是否;
        Grammar SGM_FUNC_打电话_结束;

        Grammar SGM_FUNC_计算;
        Grammar SGM_FUNC_问问题;

        Grammar SGM_FUNC_こんにちは;
        Grammar SGM_FUNC_中国語に切り替え;
        Grammar SGM_FUNC_中国語に切り替え_確認;
        Grammar SGM_FUNC_英語に切り替え;
        Grammar SGM_FUNC_英語に切り替え_確認;
        Grammar SGM_FUNC_天気;
        Grammar SGM_FUNC_時間;
        Grammar SGM_FUNC_名前;
        Grammar SGM_FUNC_おやすみ;
        Grammar SGM_FUNC_おやすみ_確認;
        Grammar SGM_FUNC_ラッスンゴレライ;
        Grammar SGM_FUNC_ラッスンゴレライ_昨日の晩飯;
        Grammar SGM_FUNC_血液型;
        Grammar SGM_FUNC_自己紹介;
        Grammar SGM_FUNC_なんか言って;
        Grammar SGM_FUNC_休憩;
        Grammar SGM_FUNC_地球;
        Grammar SGM_FUNC_曜日;
        Grammar SGM_FUNC_ばか;
        Grammar SGM_FUNC_日;
        Grammar SGM_FUNC_元気;
        /*      Grammar SGM_FUNC_Skype起動;
                Grammar SGM_FUNC_SKype電話;
                Grammar SGM_FUNC_Skype電話_国;
                public Grammar SGM_FUNC_Skype電話_電話番号;
                public Grammar SGM_FUNC_Skype電話_電話番号_確認;
                Grammar SGM_FUNC_Skype電話終了;
        */

        Grammar SGM_DIAL_HK_下午好;
        Grammar SGM_DIAL_HK_你叫什么名字;
        Grammar SGM_DIAL_HK_你好;
        Grammar SGM_DIAL_HK_早上好;
        Grammar SGM_DIAL_HK_晚上好;
        Grammar SGM_DIAL_HK_自我介绍;

        Grammar SGM_FUNC_HK_日期;
        Grammar SGM_FUNC_HK_时间;
        Grammar SGM_FUNC_HK_星期;

        Grammar SGM_FUNC_HK_关灯;
        Grammar SGM_FUNC_HK_开灯;
        Grammar SGM_FUNC_HK_开收音机;
        Grammar SGM_FUNC_HK_关闭收音机;
        Grammar SGM_FUNC_HK_打电话;
        Grammar SGM_FUNC_HK_开电视;
        Grammar SGM_FUNC_HK_关电视;
        Grammar SGM_FUNC_HK_关风扇;
        Grammar SGM_FUNC_HK_开风扇;

        Grammar SGM_FUNC_HOK_日期;
        Grammar SGM_FUNC_HOK_星期;
        Grammar SGM_FUNC_HOK_开灯;
        Grammar SGM_FUNC_HOK_关灯;
        Grammar SGM_FUNC_HOK_开收音机;
        Grammar SGM_FUNC_HOK_关收音机;
        Grammar SGM_FUNC_HOK_开电视机;
        Grammar SGM_FUNC_HOK_关电视机;
        Grammar SGM_FUNC_HOK_开风扇;
        Grammar SGM_FUNC_HOK_关风扇;
        Grammar SGM_FUNC_HOK_打电话;

        DictationGrammar dg;
        List<Grammar> Layerone;
        List<Grammar> SubLayer;
        List<Grammar> DIAL;
        List<Grammar> IRCommand;
        List<Grammar> IRCommandFan;
        List<Grammar> IRCommandRadio;
        List<Grammar> 控制;
        List<Grammar> 中文语法;

        bool IRCommand_flag = false;
        bool Game_flag = false;
        public string type = "sre";

        public void SpeechRecognized()
        {
            //   dg = new DictationGrammar("grammar:dictation#pronunciation");
            //   dg.Name = "Random";
            //  sre.LoadGrammar(dg);  
            try
            {
                sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
                srecn = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("zh-CN"));
                srejp = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("ja-JP"));
                srecan = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("zh-HK"));
            }
            catch
            {
                srespeech.Speak("Please install Chinese, Japanese and Cantonese's language pack!");
                Environment.Exit(0);
            }
            SRGS_GrammarModels();
            try
            {
                srecn.SetInputToDefaultAudioDevice();
                sre.SetInputToDefaultAudioDevice();
                srejp.SetInputToDefaultAudioDevice();
                srecan.SetInputToDefaultAudioDevice();
            }
            catch
            {
                srespeech.SpeakAsync("No audio input");
            }
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void SRGS_GrammarModels()
        {
            try
            {
                SGM_DIAL_SayHello = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_SayHello.xml");
                sre.LoadGrammar(SGM_DIAL_SayHello);

                SGM_DIAL_Music = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Music.xml");
                //sre.LoadGrammar(SGM_DIAL_Music);

                SGM_DIAL_AskIntroduction = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_AskIntroduction.xml");
                sre.LoadGrammar(SGM_DIAL_AskIntroduction);

                SGM_DIAL_AskRobotHowAreYou = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_AskRobotHowAreYou.xml");
                sre.LoadGrammar(SGM_DIAL_AskRobotHowAreYou);

                SGM_DIAL_NiceToMeetYou = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_NiceToMeetYou.xml");
                sre.LoadGrammar(SGM_DIAL_NiceToMeetYou);

                SGM_DIAL_AskWhatIsSocialRobot = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_AskWhatIsSocialRobot.xml");
                sre.LoadGrammar(SGM_DIAL_AskWhatIsSocialRobot);

                SGM_DIAL_AskWhoDesign = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_AskWhoDesign.xml");
                sre.LoadGrammar(SGM_DIAL_AskWhoDesign);

                SGM_DIAL_GoodBye = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_GoodBye.xml");
                sre.LoadGrammar(SGM_DIAL_GoodBye);

                SGM_DIAL_Greeting = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Greeting.xml");
                sre.LoadGrammar(SGM_DIAL_Greeting);

                SGM_DIAL_ThankYou = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_ThankYou.xml");
                sre.LoadGrammar(SGM_DIAL_ThankYou);

                SGM_DIAL_Scold = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Scold.xml");
                sre.LoadGrammar(SGM_DIAL_Scold);

                SGM_DIAL_Compliment = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Compliment.xml");
                sre.LoadGrammar(SGM_DIAL_Compliment);

                SGM_DIAL_AskRobotName = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_AskRobotName.xml");
                sre.LoadGrammar(SGM_DIAL_AskRobotName);

                SGM_DIAL_SwitchLanguageToChinese = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_SwitchLanguageToChinese.xml");
                sre.LoadGrammar(SGM_DIAL_SwitchLanguageToChinese);

                SGM_DIAL_SwitchLanguageToChinese_YesNo = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_SwitchLanguageToChinese_YesNo.xml");

                SGM_DIAL_LookAtMe = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_LookAtMe.xml");
                sre.LoadGrammar(SGM_DIAL_LookAtMe);

                SGM_DIAL_Sleep = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Sleep.xml");
                sre.LoadGrammar(SGM_DIAL_Sleep);

                SGM_DIAL_Sleep_YesNo = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Sleep_YesNo.xml");

                SGM_DIAL_SwitchLanguageToJapanese = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_SwitchLanguageToJapanese.xml");
                sre.LoadGrammar(SGM_DIAL_SwitchLanguageToJapanese);


                SGM_DIAL_SwitchLanguageToJapanese_YesNo = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_SwitchLanguageToJapanese_YesNo.xml");


                SGM_DIAL_AskRobotNameAns = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_AskRobotNameAns.xml");

                ///////////////////////////////////////////////////////////////////////////////////////
                //SGM_FUNC_ControlTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_ControlTV.xml");
                //sre.LoadGrammar(SGM_FUNC_ControlTV);

                //SGM_FUNC_ControlProjector = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_ControlProjector.xml");
                //sre.LoadGrammar(SGM_FUNC_ControlProjector);

                //SGM_FUNC_PowerOnOffTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_PowerOnOffTV.xml");
                //SGM_FUNC_PowerOnOffTV.Priority = 1;
                //SGM_FUNC_PowerOnOffTV.Weight = 1f;

                //SGM_FUNC_MenuTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_MenuTV.xml");

                //SGM_FUNC_MuteTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_MuteTV.xml");

                //SGM_FUNC_ChangeInputTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_ChangeInputTV.xml");

                //SGM_FUNC_upTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_upTV.xml");

                //SGM_FUNC_downTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_downTV.xml");

                //SGM_FUNC_leftTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_leftTV.xml");

                //SGM_FUNC_rightTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_rightTV.xml");

                //SGM_FUNC_enterTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_enterTV.xml");

                //SGM_FUNC_ChannelPlusTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_ChannelPlusTV.xml");

                //SGM_FUNC_ChannelMinusTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_ChannelMinusTV.xml");

                //SGM_FUNC_VolumePlusTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_VolumePlusTV.xml");

                //SGM_FUNC_VolumeMinusTV = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlTV\SGM_FUNC_VolumeMinusTV.xml");


                SGM_FUNC_ControlFan = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlFan\SGM_FUNC_ControlFan.xml");
                sre.LoadGrammar(SGM_FUNC_ControlFan);

                SGM_FUNC_PowerOnOffFan = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlFan\SGM_FUNC_PowerOnOffFan.xml");

                SGM_FUNC_onUpDown = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlFan\SGM_FUNC_onUpDown.xml");

                SGM_FUNC_onLeftRightFan = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlFan\SGM_FUNC_onLeftRightFan.xml");
               

                SGM_FUNC_Speed = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlFan\SGM_FUNC_Speed.xml");

                SGM_FUNC_Timer = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlFan\SGM_FUNC_Timer.xml");

                SGM_FUNC_PowerOnLight = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlLight\SGM_FUNC_PowerOnLight.xml");
                sre.LoadGrammar(SGM_FUNC_PowerOnLight);

                SGM_FUNC_PowerOffLight = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlLight\SGM_FUNC_PowerOffLight.xml");
                sre.LoadGrammar(SGM_FUNC_PowerOffLight);

                SGM_FUNC_ControlRadio = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlRadio\SGM_FUNC_ControlRadio.xml");
                sre.LoadGrammar(SGM_FUNC_ControlRadio);

                SGM_FUNC_NextRadio = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlRadio\SGM_FUNC_NextRadio.xml");

                SGM_FUNC_PreviousRadio = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlRadio\SGM_FUNC_PreviousRadio.xml");

                SGM_FUNC_VolumeUpRadio = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlRadio\SGM_FUNC_VolumeUpRadio.xml");

                SGM_FUNC_VolumeDownRadio = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlRadio\SGM_FUNC_VolumeDownRadio.xml");

                SGM_FUNC_PowerOnOffRadio = new Grammar(@"C:\test\XmlGrammar\English\iRKit\ControlRadio\SGM_FUNC_PowerOnOffRadio.xml");
                ///////////////////////////////////////////////////////////////////////////////////////
                //SGM_FUNC_AskForRadioFunction = new Grammar(@"C:\test\XmlGrammar\English\RadioFunction\SGM_FUNC_AskForRadioFunction.xml");
                //sre.LoadGrammar(SGM_FUNC_AskForRadioFunction);

                SGM_FUNC_StartRadioStaion = new Grammar(@"C:\test\XmlGrammar\English\RadioFunction\SGM_FUNC_StartRadioStaion.xml");
                sre.LoadGrammar(SGM_FUNC_StartRadioStaion);
               
                SGM_FUNC_StopRadioStation = new Grammar(@"C:\test\XmlGrammar\English\RadioFunction\SGM_FUNC_StopRadioStation.xml");

                SGM_FUNC_StartRadioStationYesNo = new Grammar(@"C:\test\XmlGrammar\English\RadioFunction\SGM_FUNC_StartRadioStationYesNo.xml");

                SGM_FUNC_StopRadioStationYesNo = new Grammar(@"C:\test\XmlGrammar\English\RadioFunction\SGM_FUNC_StopRadioStationYesNo.xml");

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              SGM_FUNC_ReadNews = new Grammar(@"C:\test\XmlGrammar\English\ReadNewsFunction\SGM_FUNC_ReadNews.xml");
              sre.LoadGrammar(SGM_FUNC_ReadNews);

                SGM_FUNC_LanguageOption = new Grammar(@"C:\test\XmlGrammar\English\ReadNewsFunction\SGM_LanguageOption.xml");

                SGM_FUNC_NextNews = new Grammar(@"C:\test\XmlGrammar\English\ReadNewsFunction\SGM_FUNC_Next_News.xml");

                SGM_FUNC_StopReadNews = new Grammar(@"C:\test\XmlGrammar\English\ReadNewsFunction\SGM_FUNC_StopReadNews.xml");

                SGM_ContinueReadNews_YesNo = new Grammar(@"C:\test\XmlGrammar\English\ReadNewsFunction\SGM_ContinueReadNews_YesNo.xml");
                //////////////////////////////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_AskWhatTimeNow = new Grammar(@"C:\test\XmlGrammar\English\DataTimeFunction\SGM_FUNC_AskWhatTimeNow.xml");
                sre.LoadGrammar(SGM_FUNC_AskWhatTimeNow);

                SGM_FUNC_AskWhatDayIsToday = new Grammar(@"C:\test\XmlGrammar\English\DataTimeFunction\SGM_FUNC_AskWhatDayIsToday.xml");
                sre.LoadGrammar(SGM_FUNC_AskWhatDayIsToday);

                SGM_FUNC_AskWhatDateIsToday = new Grammar(@"C:\test\XmlGrammar\English\DataTimeFunction\SGM_FUNC_AskWhatDateIsToday.xml");
                sre.LoadGrammar(SGM_FUNC_AskWhatDateIsToday);
                /////////////////////////////////////////////////////////////////////////////////////////


                /////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_Char = new Grammar(@"C:\test\XmlGrammar\English\Register\SGM_FUNC_Char.xml");
                /////////////////////////////////////////////////////////////////////////////////////////

                /////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_RegisterMode_YesNo = new Grammar(@"C:\test\XmlGrammar\English\Register\SGM_FUNC_RegisterMode_YesNo.xml");
                /////////////////////////////////////////////////////////////////////////////////////////

                /////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_TellJokes = new Grammar(@"C:\test\XmlGrammar\English\TellJokes\SGM_FUNC_TellJokes.xml");
                sre.LoadGrammar(SGM_FUNC_TellJokes);
                /////////////////////////////////////////////////////////////////////////////////////////


                /////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_AskCountdown = new Grammar(@"C:\test\XmlGrammar\English\Countdown\SGM_FUNC_AskCountdown.xml");
                sre.LoadGrammar(SGM_FUNC_AskCountdown); ;

                SGM_FUNC_Countdown = new Grammar(@"C:\test\XmlGrammar\English\Countdown\SGM_FUNC_Countdown.xml");
                sre.LoadGrammar(SGM_FUNC_Countdown);

                SGM_FUNC_CountdownYesNo = new Grammar(@"C:\test\XmlGrammar\English\Countdown\SGM_FUNC_CountdownYesNo.xml");
                //////////////////////////////////////////////////////////////////////////////////////////

                
                //////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_AskTodayCustomWeather = new Grammar(@"C:\test\XmlGrammar\English\AskWeather\SGM_FUNC_AskTodayCustomWeather.xml");
                sre.LoadGrammar(SGM_FUNC_AskTodayCustomWeather);

                SGM_FUNC_AskTomorrowCustomWeather = new Grammar(@"C:\test\XmlGrammar\English\AskWeather\SGM_FUNC_AskTomorrowCustomWeather.xml");
                sre.LoadGrammar(SGM_FUNC_AskTomorrowCustomWeather);
                ///////////////////////////////////////////////////////////////////////////////////////////


                // SGM_DIAL_AskPresidentOrPrimeMinister = new Grammar(@"C:\test\AskPresidentOrPrimeMinister\SGM_DIAL_AskPresidentOrPrimeMinister.xml");
                // SGM_DIAL_AskPresidentOrPrimeMinister.Priority = 1;
                // SGM_DIAL_AskPresidentOrPrimeMinister.Weight = 1f;
                // sre.LoadGrammar(SGM_DIAL_AskPresidentOrPrimeMinister);
                ///////////////////////////////////////////////////////////////////////////////////////////
                SGM_DIAL_AskCountryPresidentOrPrimeMinister = new Grammar(@"C:\test\XmlGrammar\English\AskPresidentOrPrimeMinister\SGM_DIAL_AskCoutryPresidentOrPrimeMinister.xml");
                sre.LoadGrammar(SGM_DIAL_AskCountryPresidentOrPrimeMinister);
                ////////////////////////////////////////////////////////////////////////////////////////////


                /////////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_COMPLEX_SetReminder = new Grammar(@"C:\test\XmlGrammar\English\SetReminder\SGM_FUNC_COMPLEX_SetReminder.xml");
                SGM_FUNC_COMPLEX_SetReminder.Priority = 1;
                SGM_FUNC_COMPLEX_SetReminder.Weight = 1f;
                sre.LoadGrammar(SGM_FUNC_COMPLEX_SetReminder);

                SGM_FUNC_COMPLEX_SetReminderYesNo = new Grammar(@"C:\test\XmlGrammar\English\SetReminder\SGM_FUNC_COMPLEX_SetReminderYesNo.xml");
                SGM_FUNC_COMPLEX_SetReminderYesNo.Priority = 1;
                SGM_FUNC_COMPLEX_SetReminderYesNo.Weight = 1f;
                /////////////////////////////////////////////////////////////////////////////////////////////

                //Skype
                /////////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_Call = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_Call.xml");
                sre.LoadGrammar(SGM_FUNC_Call);

                SGM_FUNC_CallPerson = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_CallPerson.xml");

                SGM_FUNC_AskSkypeFunction = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_AskSkypeFunction.xml");
                //sre.LoadGrammar(SGM_FUNC_AskSkypeFunction);

                SGM_FUNC_SkypeCall = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SkypeCall.xml");
                //sre.LoadGrammar(SGM_FUNC_SkypeCall);

                SGM_FUNC_SkypePhoneCall = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SkypePhoneCall.xml");
                //sre.LoadGrammar(SGM_FUNC_SkypePhoneCall);

                SGM_FUNC_SkypeCall_PhoneCall_Country = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SkypeCall_PhoneCall_Country.xml");

                SGM_FUNC_SkypeCall_PhoneNumber = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SkypeCall_PhoneNumber.xml");


                SGM_FUNC_SkypeCall_PhoneCall_YesNo = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SkypeCall_PhoneCall_YesNo.xml");

                SGM_FUNC_SkypePhoneCallFinished = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SkypePhoneCallFinished.xml");

               // SGM_FUNC_SendMessage = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SendMessage.xml");
               // sre.LoadGrammar(SGM_FUNC_SendMessage);

                SGM_FUNC_SendSMS = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SendSMS.xml");
                sre.LoadGrammar(SGM_FUNC_SendSMS);

                SGM_FUNC_SendMessageOption = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SendMesageOption.xml");

                SGM_FUNC_SMSCountry = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SMSCountry.xml");

                SGM_FUNC_SMSPhoneNumber = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SMSPhoneNumber.xml");

                SGM_FUNC_SendSMSYesOrNo = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SendSMSYesOrNo.xml");

                SGM_FUNC_SpeechToTextMessageFinished = new Grammar(@"C:\test\XmlGrammar\English\Skype\SGM_FUNC_SpeechToTextMessageFinished.xml");

                //////////////////////////////////////////////////////////////////////////////////////////////
                SGM_GAME_PlayAnimalLegCounting = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_PlayAnimalLegCounting.xml");
                sre.LoadGrammar(SGM_GAME_PlayAnimalLegCounting);

                SGM_GAME_PlayAnimalLegCountingYesNo = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_PlayAnimalLegCountingYesNo.xml");

                SGM_GAME_AnimalLegCountingAnswer = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_AnimalLegCountingAnswer.xml");

                SGM_GAME_AnimalLegCountingQuestionRepeat = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_AnimalLegCountingQuestionRepeat.xml");

                SGM_GAME_AnimalLegCountingAnswerDontKnow = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_AnimalLegCountingAnswerDontKnow.xml");

                SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo.xml");

                SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo = new Grammar(@"C:\test\XmlGrammar\English\Games\AnimalLeg\SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo.xml");

                //////////////////////////////////////////////////////////////////////////////////////////////////////
               // SGM_DIAL_Dance = new Grammar(@"C:\test\XmlGrammar\English\Dance\SGM_DIAL_Dance.xml");
               // sre.LoadGrammar(SGM_DIAL_Dance);
                SGM_DIAL_Raise_Right_Hand = new Grammar(@"C:\test\XmlGrammar\English\Action\SGM_DIAL_Raise_Right_Hand.xml");
                sre.LoadGrammar(SGM_DIAL_Raise_Right_Hand);
                SGM_DIAL_Raise_Left_Hand = new Grammar(@"C:\test\XmlGrammar\English\Action\SGM_DIAL_Raise_Left_Hand.xml");
                sre.LoadGrammar(SGM_DIAL_Raise_Left_Hand);

                SGM_FUNC_GoingOut = new Grammar(@"C:\test\XmlGrammar\English\AskWeather\SGM_FUNC_GoingOut.xml");
                sre.LoadGrammar(SGM_FUNC_GoingOut);

                //Imagine Cup

                SGM_FUNC_ImagineCup = new Grammar(@"C:\test\XmlGrammar\English\ImagineCup\SGM_FUNC_ImagineCup.xml");
                sre.LoadGrammar(SGM_FUNC_ImagineCup);

                SGM_DIAL_Emotion= new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Emotion.xml");
                sre.LoadGrammar(SGM_DIAL_Emotion);


                SGM_DIAL_Help = new Grammar(@"C:\test\XmlGrammar\English\Greeting\SGM_DIAL_Help.xml");
                sre.LoadGrammar(SGM_DIAL_Help);
            }

            catch
            {
                srespeech.SpeakAsync("Load Grammar Error");
            }

        }

        public void SRGS_LoadGrammarModels()
        {
            try
            {
                sre.LoadGrammar(SGM_DIAL_SayHello);

                sre.LoadGrammar(SGM_DIAL_AskIntroduction);

                sre.LoadGrammar(SGM_DIAL_AskRobotHowAreYou);

                sre.LoadGrammar(SGM_DIAL_NiceToMeetYou);

                sre.LoadGrammar(SGM_DIAL_AskWhatIsSocialRobot);

                sre.LoadGrammar(SGM_DIAL_AskWhoDesign);

                sre.LoadGrammar(SGM_DIAL_GoodBye);

                sre.LoadGrammar(SGM_DIAL_Greeting);

                sre.LoadGrammar(SGM_DIAL_ThankYou);

                sre.LoadGrammar(SGM_DIAL_Scold);

                sre.LoadGrammar(SGM_DIAL_Compliment);

                sre.LoadGrammar(SGM_DIAL_AskRobotName);

                sre.LoadGrammar(SGM_DIAL_SwitchLanguageToChinese);

                sre.LoadGrammar(SGM_DIAL_LookAtMe);

                sre.LoadGrammar(SGM_DIAL_Sleep);

                sre.LoadGrammar(SGM_DIAL_SwitchLanguageToJapanese);

                sre.LoadGrammar(SGM_FUNC_ControlFan);

                sre.LoadGrammar(SGM_FUNC_PowerOnLight);

                sre.LoadGrammar(SGM_FUNC_PowerOffLight);

                sre.LoadGrammar(SGM_FUNC_ControlRadio);

                sre.LoadGrammar(SGM_FUNC_StartRadioStaion);

                sre.LoadGrammar(SGM_FUNC_AskWhatTimeNow);

                sre.LoadGrammar(SGM_FUNC_AskWhatDayIsToday);

                sre.LoadGrammar(SGM_FUNC_AskWhatDateIsToday);

                sre.LoadGrammar(SGM_FUNC_TellJokes);

                sre.LoadGrammar(SGM_FUNC_AskCountdown); ;

                sre.LoadGrammar(SGM_FUNC_Countdown);

                sre.LoadGrammar(SGM_FUNC_AskTodayCustomWeather);

                sre.LoadGrammar(SGM_FUNC_AskTomorrowCustomWeather);

                sre.LoadGrammar(SGM_DIAL_AskCountryPresidentOrPrimeMinister);

                SGM_FUNC_COMPLEX_SetReminder.Priority = 1;
                SGM_FUNC_COMPLEX_SetReminder.Weight = 1f;
                sre.LoadGrammar(SGM_FUNC_COMPLEX_SetReminder);

                SGM_FUNC_COMPLEX_SetReminderYesNo.Priority = 1;
                SGM_FUNC_COMPLEX_SetReminderYesNo.Weight = 1f;

                sre.LoadGrammar(SGM_FUNC_Call);

              //  sre.LoadGrammar(SGM_FUNC_SendMessage);

                sre.LoadGrammar(SGM_FUNC_SendSMS);

                sre.LoadGrammar(SGM_DIAL_Raise_Right_Hand);

                sre.LoadGrammar(SGM_DIAL_Raise_Left_Hand);

                sre.LoadGrammar(SGM_FUNC_GoingOut);

                sre.LoadGrammar(SGM_FUNC_ImagineCup);
            }

            catch
            {
                srespeech.SpeakAsync("Load Grammar Error");
            }

        }

        public void SRGS_UnloadGrammarModels()
        {
            try
            {
                sre.UnloadGrammar(SGM_DIAL_SayHello);

                sre.UnloadGrammar(SGM_DIAL_AskIntroduction);

                sre.UnloadGrammar(SGM_DIAL_AskRobotHowAreYou);

                sre.UnloadGrammar(SGM_DIAL_NiceToMeetYou);

                sre.UnloadGrammar(SGM_DIAL_AskWhatIsSocialRobot);

                sre.UnloadGrammar(SGM_DIAL_AskWhoDesign);

                sre.UnloadGrammar(SGM_DIAL_GoodBye);

                sre.UnloadGrammar(SGM_DIAL_Greeting);

                sre.UnloadGrammar(SGM_DIAL_ThankYou);

                sre.UnloadGrammar(SGM_DIAL_Scold);

                sre.UnloadGrammar(SGM_DIAL_Compliment);

                sre.UnloadGrammar(SGM_DIAL_AskRobotName);

                sre.UnloadGrammar(SGM_DIAL_SwitchLanguageToChinese);

                sre.UnloadGrammar(SGM_DIAL_LookAtMe);

                sre.UnloadGrammar(SGM_DIAL_Sleep);

                sre.UnloadGrammar(SGM_DIAL_SwitchLanguageToJapanese);

                sre.UnloadGrammar(SGM_FUNC_ControlFan);

                sre.UnloadGrammar(SGM_FUNC_PowerOnLight);

                sre.UnloadGrammar(SGM_FUNC_PowerOffLight);

                sre.UnloadGrammar(SGM_FUNC_ControlRadio);

                sre.UnloadGrammar(SGM_FUNC_StartRadioStaion);

                sre.UnloadGrammar(SGM_FUNC_AskWhatTimeNow);

                sre.UnloadGrammar(SGM_FUNC_AskWhatDayIsToday);

                sre.UnloadGrammar(SGM_FUNC_AskWhatDateIsToday);

                sre.UnloadGrammar(SGM_FUNC_TellJokes);

                sre.UnloadGrammar(SGM_FUNC_AskCountdown); ;

                sre.UnloadGrammar(SGM_FUNC_Countdown);

                sre.UnloadGrammar(SGM_FUNC_AskTodayCustomWeather);

                sre.UnloadGrammar(SGM_FUNC_AskTomorrowCustomWeather);

                sre.UnloadGrammar(SGM_DIAL_AskCountryPresidentOrPrimeMinister);

                SGM_FUNC_COMPLEX_SetReminder.Priority = 1;
                SGM_FUNC_COMPLEX_SetReminder.Weight = 1f;
                sre.UnloadGrammar(SGM_FUNC_COMPLEX_SetReminder);

                SGM_FUNC_COMPLEX_SetReminderYesNo.Priority = 1;
                SGM_FUNC_COMPLEX_SetReminderYesNo.Weight = 1f;

                sre.UnloadGrammar(SGM_FUNC_Call);

             //   sre.UnloadGrammar(SGM_FUNC_SendMessage);

                sre.UnloadGrammar(SGM_FUNC_SendSMS);

                sre.UnloadGrammar(SGM_DIAL_Raise_Right_Hand);

                sre.UnloadGrammar(SGM_DIAL_Raise_Left_Hand);

                sre.UnloadGrammar(SGM_FUNC_GoingOut);

                sre.UnloadGrammar(SGM_FUNC_ImagineCup);
            }

            catch
            {
                srespeech.SpeakAsync("Unload Grammar Error");
            }

        }
        public void HokkienGrammarLoad()
        {
            try
            {
                SGM_FUNC_HOK_关收音机 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_关收音机.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_关收音机);

                SGM_FUNC_HOK_关灯 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_关灯.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_关灯);

                SGM_FUNC_HOK_关电视机 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_关电视机.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_关电视机);

                SGM_FUNC_HOK_关风扇 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_关风扇.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_关风扇);

                SGM_FUNC_HOK_开收音机 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_开收音机.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_开收音机);

                SGM_FUNC_HOK_开灯 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_开灯.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_开灯);

                SGM_FUNC_HOK_开电视机 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_开电视机.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_开电视机);

                SGM_FUNC_HOK_开风扇 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_开风扇.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_开风扇);

                SGM_FUNC_HOK_打电话 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_打电话.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_打电话);

                SGM_FUNC_HOK_日期 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_日期.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_日期);

                SGM_FUNC_HOK_星期 = new Grammar(@"C:\test\XmlGrammar\hokkian\SGM_FUNC_HOK_星期.xml");
                srecn.LoadGrammar(SGM_FUNC_HOK_星期);

            }
            catch
            {
                srecnspeech.SpeakAsync("语法装载错误");
            }
            }


        public void ChineseGrammarLoad()
        {
            try
            {
                SGM_DIAL_你好 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_你好.xml");
                srecn.LoadGrammar(SGM_DIAL_你好);

                SGM_DIAL_谢谢 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_谢谢.xml");
                srecn.LoadGrammar(SGM_DIAL_谢谢);

                SGM_DIAL_你叫什么名字 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_你叫什么名字.xml");
                srecn.LoadGrammar(SGM_DIAL_你叫什么名字);

                SGM_DIAL_早上好 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_早上好.xml");
                srecn.LoadGrammar(SGM_DIAL_早上好);

                SGM_DIAL_中午好 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_中午好.xml");
                srecn.LoadGrammar(SGM_DIAL_中午好);

                SGM_DIAL_下午好 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_下午好.xml");
                srecn.LoadGrammar(SGM_DIAL_下午好);

                SGM_DIAL_晚上好 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_晚上好.xml");
                srecn.LoadGrammar(SGM_DIAL_晚上好);

                SGM_DIAL_自我介绍 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_自我介绍.xml");
                srecn.LoadGrammar(SGM_DIAL_自我介绍);

                SGM_DIAL_谁设计了你 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_谁设计了你.xml");
                srecn.LoadGrammar(SGM_DIAL_谁设计了你);

                SGM_DIAL_英文识别 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_英文识别.xml");
                srecn.LoadGrammar(SGM_DIAL_英文识别);

                SGM_DIAL_英文识别_是否 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_英文识别_是否.xml");

                SGM_DIAL_日文识别 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_日文识别.xml");
                srecn.LoadGrammar(SGM_DIAL_日文识别);

                SGM_DIAL_日文识别_是否 = new Grammar(@"C:\test\XmlGrammar\Chinese\打招呼\SGM_DIAL_日文识别_是否.xml");

                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_CHN_时间 = new Grammar(@"C:\test\XmlGrammar\Chinese\时间\SGM_FUNC_CHN_时间.xml");
                srecn.LoadGrammar(SGM_FUNC_CHN_时间);

                SGM_FUNC_CHN_日期 = new Grammar(@"C:\test\XmlGrammar\Chinese\时间\SGM_FUNC_CHN_日期.xml");
                srecn.LoadGrammar(SGM_FUNC_CHN_日期);

                SGM_FUNC_CHN_星期 = new Grammar(@"C:\test\XmlGrammar\Chinese\时间\SGM_FUNC_CHN_星期.xml");
                srecn.LoadGrammar(SGM_FUNC_CHN_星期);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_CHN_控制电视 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_控制电视.xml");
                srecn.LoadGrammar(SGM_FUNC_CHN_控制电视);

                SGM_FUNC_CHN_电源 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_电源.xml");

                SGM_FUNC_CHN_菜单 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_菜单.xml");

                SGM_FUNC_CHN_上 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_上.xml");

                SGM_FUNC_CHN_下 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_下.xml");

                SGM_FUNC_CHN_左 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_左.xml");

                SGM_FUNC_CHN_右 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_右.xml");

                SGM_FUNC_CHN_声音加 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_声音加.xml");

                SGM_FUNC_CHN_声音减 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_声音减.xml");

                SGM_FUNC_CHN_频道加 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_频道加.xml");

                SGM_FUNC_CHN_频道减 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_频道减.xml");

                SGM_FUNC_CHN_进入 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_进入.xml");

                SGM_FUNC_CHN_退出 = new Grammar(@"C:\test\XmlGrammar\Chinese\控制\SGM_FUNC_CHN_退出.xml");

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                SGM_FUNC_问问题 = new Grammar(@"C:\test\XmlGrammar\Chinese\XunFei\SGM_FUNC_问问题.xml");
                srecn.LoadGrammar(SGM_FUNC_问问题);

                SGM_FUNC_计算 = new Grammar(@"C:\test\XmlGrammar\Chinese\XunFei\SGM_FUNC_计算.xml");
                srecn.LoadGrammar(SGM_FUNC_计算);

                SGM_FUNC_打电话 = new Grammar(@"C:\test\XmlGrammar\Chinese\打电话发简讯\SGM_FUNC_打电话.xml");
                srecn.LoadGrammar(SGM_FUNC_打电话);

                SGM_FUNC_打电话_是否 = new Grammar(@"C:\test\XmlGrammar\Chinese\打电话发简讯\SGM_FUNC_打电话_是否.xml");
                //srecn.LoadGrammar(SGM_FUNC_打电话_是否);

                SGM_FUNC_打电话_结束 = new Grammar(@"C:\test\XmlGrammar\Chinese\打电话发简讯\SGM_FUNC_打电话_结束.xml");

                SGM_FUNC_发简讯 = new Grammar(@"C:\test\XmlGrammar\Chinese\打电话发简讯\SGM_FUNC_发简讯.xml");
                srecn.LoadGrammar(SGM_FUNC_发简讯);

                SGM_FUNC_发简讯_是否 = new Grammar(@"C:\test\XmlGrammar\Chinese\打电话发简讯\SGM_FUNC_发简讯_是否.xml");
            }

            catch
            {
                srecnspeech.SpeakAsync("语法装载错误");
            }
        }


        public void CantoneseGrammarLoad()
        {
            try
            {
                SGM_DIAL_HK_下午好 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打招呼\SGM_DIAL_HK_下午好.xml");
                srecan.LoadGrammar(SGM_DIAL_HK_下午好);

                SGM_DIAL_HK_你叫什么名字 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打招呼\SGM_DIAL_HK_你叫什么名字.xml");
                srecan.LoadGrammar(SGM_DIAL_HK_你叫什么名字);

                SGM_DIAL_HK_你好 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打招呼\SGM_DIAL_HK_你好.xml");
                srecan.LoadGrammar(SGM_DIAL_HK_你好);

                SGM_DIAL_HK_早上好 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打招呼\SGM_DIAL_HK_早上好.xml");
                srecan.LoadGrammar(SGM_DIAL_HK_早上好);

                SGM_DIAL_HK_晚上好 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打招呼\SGM_DIAL_HK_晚上好.xml");
                srecan.LoadGrammar(SGM_DIAL_HK_晚上好);

                SGM_DIAL_HK_自我介绍 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打招呼\SGM_DIAL_HK_自我介绍.xml");
               srecan.LoadGrammar(SGM_DIAL_HK_自我介绍);

                SGM_FUNC_HK_日期 = new Grammar(@"C:\test\XmlGrammar\Cantonese\时间\SGM_FUNC_HK_日期.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_日期);

                SGM_FUNC_HK_时间 = new Grammar(@"C:\test\XmlGrammar\Cantonese\时间\SGM_FUNC_HK_时间.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_时间);

                SGM_FUNC_HK_星期 = new Grammar(@"C:\test\XmlGrammar\Cantonese\时间\SGM_FUNC_HK_星期.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_星期);

                SGM_FUNC_HK_关灯 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_关灯.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_关灯);


                SGM_FUNC_HK_开灯 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_开灯.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_开灯);

                SGM_FUNC_HK_开收音机 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_开收音机.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_开收音机);

                SGM_FUNC_HK_关闭收音机 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_关闭收音机.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_关闭收音机);

                SGM_FUNC_HK_打电话 = new Grammar(@"C:\test\XmlGrammar\Cantonese\打电话发简讯\SGM_FUNC_HK_打电话.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_打电话);

                SGM_FUNC_HK_关电视 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_关电视.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_关电视);

                SGM_FUNC_HK_开电视 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_开电视.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_开电视);

                SGM_FUNC_HK_关风扇 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_关风扇.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_关风扇);

                SGM_FUNC_HK_开风扇 = new Grammar(@"C:\test\XmlGrammar\Cantonese\控制\SGM_FUNC_HK_开风扇.xml");
                srecan.LoadGrammar(SGM_FUNC_HK_开风扇);


            }

            catch
            {
                srecnspeech.SpeakAsync("语法装载错误");
            }

        }
        public void JapaneseGrammarLoad()
        {
            try
            {
                SGM_FUNC_こんにちは = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_こんにちは.xml");
                srejp.LoadGrammar(SGM_FUNC_こんにちは);

                SGM_FUNC_中国語に切り替え = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_中国語に切り替え.xml");
                srejp.LoadGrammar(SGM_FUNC_中国語に切り替え);

                SGM_FUNC_中国語に切り替え_確認 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_中国語に切り替え_確認.xml");

                SGM_FUNC_英語に切り替え = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_英語に切り替え.xml");
                srejp.LoadGrammar(SGM_FUNC_英語に切り替え);

                SGM_FUNC_英語に切り替え_確認 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_英語に切り替え_確認.xml");

            
                SGM_FUNC_天気 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_天気.xml");
                srejp.LoadGrammar(SGM_FUNC_天気);

                SGM_FUNC_時間 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_時間.xml");
                srejp.LoadGrammar(SGM_FUNC_時間);


                SGM_FUNC_曜日 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_曜日.xml");
                srejp.LoadGrammar(SGM_FUNC_曜日);

                SGM_FUNC_名前 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_名前.xml");
                srejp.LoadGrammar(SGM_FUNC_名前);

                SGM_FUNC_おやすみ = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_おやすみ.xml");
                srejp.LoadGrammar(SGM_FUNC_おやすみ);

                SGM_FUNC_おやすみ_確認 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_おやすみ_確認.xml");

                SGM_FUNC_ラッスンゴレライ = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_ラッスンゴレライ.xml");
                srejp.LoadGrammar(SGM_FUNC_ラッスンゴレライ);

                SGM_FUNC_ラッスンゴレライ_昨日の晩飯 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_ラッスンゴレライ_昨日の晩飯.xml");
                srejp.LoadGrammar(SGM_FUNC_ラッスンゴレライ_昨日の晩飯);

                SGM_FUNC_血液型 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_血液型.xml");
                srejp.LoadGrammar(SGM_FUNC_血液型);

                SGM_FUNC_自己紹介 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_自己紹介.xml");
                srejp.LoadGrammar(SGM_FUNC_自己紹介);

                SGM_FUNC_なんか言って = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_なんか言って.xml");
                srejp.LoadGrammar(SGM_FUNC_なんか言って);

                SGM_FUNC_休憩 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_休憩.xml");
                srejp.LoadGrammar(SGM_FUNC_休憩);

                SGM_FUNC_地球 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_地球.xml");
                srejp.LoadGrammar(SGM_FUNC_地球);

                SGM_FUNC_ばか = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_ばか.xml");
                srejp.LoadGrammar(SGM_FUNC_ばか);

                SGM_FUNC_日 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_日.xml");
                srejp.LoadGrammar(SGM_FUNC_日);

                SGM_FUNC_元気 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_元気.xml");
                srejp.LoadGrammar(SGM_FUNC_元気);

/*              SGM_FUNC_Skype起動 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_Skype起動.xml");
                srejp.LoadGrammar(SGM_FUNC_Skype起動);

                SGM_FUNC_Skype電話 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_Skype電話.xml");
                srejp.LoadGrammar(SGM_FUNC_Skype電話);

                SGM_FUNC_Skype電話_国 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_Skype電話_国.xml");
                srejp.LoadGrammar(SGM_FUNC_Skype電話_国);

                SGM_FUNC_Skype電話_電話番号 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_Skype電話_電話番号.xml");
                srejp.LoadGrammar(SGM_FUNC_Skype電話_電話番号);

                SGM_FUNC_Skype電話_電話番号_確認 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_Skype電話_電話番号_確認.xml");

                SGM_FUNC_Skype電話終了 = new Grammar(@"C:\test\XmlGrammar\Japanese\ご挨拶\SGM_FUNC_Skype電話終了.xml");
                srejp.LoadGrammar(SGM_FUNC_Skype電話終了);
*/
            }

            catch
            {
                srecnspeech.SpeakAsync("语法装载错误");//Japanese
            }
        }

        public bool MouthMuteMark = false;

        public void LayerGrammarLoadAndUnload(string RuleName, string ResultName)
        {
            IRCommand = new List<Grammar>{
                                      SGM_FUNC_PowerOnOffTV,
                                      SGM_FUNC_MenuTV, 
                                      SGM_FUNC_MuteTV,
                                      SGM_FUNC_ChangeInputTV, 
                                      SGM_FUNC_upTV,
                                      SGM_FUNC_downTV,
                                      SGM_FUNC_leftTV,
                                      SGM_FUNC_rightTV,
                                      SGM_FUNC_enterTV,
                                      SGM_FUNC_ChannelPlusTV,
                                      SGM_FUNC_ChannelMinusTV ,
                                      SGM_FUNC_VolumePlusTV,
                                      SGM_FUNC_VolumeMinusTV, 
                                        };

            IRCommandFan = new List<Grammar>{
                                      SGM_FUNC_PowerOnOffFan,
                                      SGM_FUNC_onUpDown,
                                      SGM_FUNC_onLeftRightFan,
                                      SGM_FUNC_Speed,
                                      SGM_FUNC_Timer,
                                        };

            IRCommandRadio = new List<Grammar>{
                SGM_FUNC_PowerOnOffRadio,
                SGM_FUNC_VolumeUpRadio,
                SGM_FUNC_VolumeDownRadio,
                SGM_FUNC_NextRadio,
                SGM_FUNC_PreviousRadio,

            };

            控制 = new List<Grammar>{
                                //   SGM_FUNC_CHN_控制电视,
                                   SGM_FUNC_CHN_电源,
                                   SGM_FUNC_CHN_菜单,
                                   SGM_FUNC_CHN_上,
                                   SGM_FUNC_CHN_下,
                                   SGM_FUNC_CHN_左,
                                   SGM_FUNC_CHN_右,
                                   SGM_FUNC_CHN_声音加,
                                   SGM_FUNC_CHN_声音减,
                                   SGM_FUNC_CHN_频道加,
                                   SGM_FUNC_CHN_频道减,
                                   SGM_FUNC_CHN_进入,
                                   SGM_FUNC_CHN_退出
                                    };


            DIAL = new List<Grammar>{
                                      SGM_DIAL_SayHello,
                                      SGM_DIAL_AskIntroduction,
                                      SGM_DIAL_AskRobotHowAreYou,
                                      SGM_DIAL_NiceToMeetYou,
                                      SGM_DIAL_AskWhatIsSocialRobot,
                                      SGM_DIAL_AskWhoDesign,
                                      SGM_DIAL_GoodBye,
                                      SGM_DIAL_Greeting,
                                      SGM_DIAL_ThankYou,
                                      SGM_DIAL_Scold,
                                      SGM_DIAL_Compliment,
                                      SGM_DIAL_AskRobotName,
                                      SGM_DIAL_LookAtMe,
                                      SGM_DIAL_Sleep,
                                      SGM_DIAL_SwitchLanguageToChinese,
                                      SGM_DIAL_SwitchLanguageToJapanese,
                                      SGM_DIAL_Emotion,
                                     };


            Layerone = new List<Grammar> { 
                                      
                                           SGM_FUNC_AskWhatTimeNow,
                                           SGM_FUNC_AskWhatDayIsToday,
                                           SGM_FUNC_AskWhatDateIsToday,

                                           SGM_FUNC_TellJokes,

                                           SGM_FUNC_AskCountdown,
                                           SGM_FUNC_Countdown,

                                           SGM_FUNC_AskTodayCustomWeather,
                                           SGM_FUNC_AskTomorrowCustomWeather,

                                          
                                           SGM_FUNC_COMPLEX_SetReminder,

                                           SGM_FUNC_ControlTV,
                                           SGM_FUNC_ControlProjector,

                                           SGM_FUNC_AskForRadioFunction,
                                           SGM_FUNC_StartRadioStaion,

                                           SGM_FUNC_AskSkypeFunction,
                                           SGM_FUNC_SkypeCall,
                                           SGM_FUNC_SkypePhoneCall,
                                           SGM_FUNC_SendSMS,
                                           SGM_FUNC_Call



                                         
                                        };

            中文语法 = new List<Grammar>
            {
                   SGM_FUNC_CHN_时间,
                   SGM_FUNC_CHN_日期,
                   SGM_FUNC_CHN_星期,
                   SGM_DIAL_谢谢,
                   SGM_DIAL_你好,
                   SGM_DIAL_你叫什么名字,
                   SGM_DIAL_早上好,
                   SGM_DIAL_中午好,
                   SGM_DIAL_下午好,
                   SGM_DIAL_晚上好,
                   SGM_DIAL_自我介绍,
                   SGM_DIAL_谁设计了你,
                   //SGM_DIAL_功能,
                   SGM_DIAL_英文识别,
                   SGM_FUNC_问问题,
                   SGM_FUNC_计算,
                   SGM_FUNC_打电话,
                   SGM_FUNC_发简讯,
                   SGM_FUNC_CHN_控制电视
            
            };

            // Layerone.AddRange(DIAL);

            try
            {
                if(true)//Function.Vision.WakeUp
                {
                    switch (RuleName)
                    {
                        case "SGM_FUNC_ReadNews":
                            sre.LoadGrammar(SGM_FUNC_LanguageOption);
                            sre.UnloadGrammar(SGM_FUNC_ReadNews);
                            sre.LoadGrammar(SGM_FUNC_StopReadNews);
                            break;

                        case "SGM_FUNC_LanguageOption":
                                sre.LoadGrammar(SGM_FUNC_NextNews);
                                sre.UnloadGrammar(SGM_FUNC_LanguageOption);
                            break;

                        case "SGM_ContinueReadNews_YesNo":
                            sre.UnloadGrammar(SGM_ContinueReadNews_YesNo);
                            MainWindow.GrammarTimer.Stop();
                            break;

                        case "SGM_FUNC_StopReadNews":
                            sre.UnloadGrammar(SGM_FUNC_NextNews);
                            sre.UnloadGrammar(SGM_FUNC_StopReadNews);
                            sre.LoadGrammar(SGM_FUNC_ReadNews);
                            break;

                        //////////////////////////////////////////////
                        case "SGM_FUNC_Countdown":
                            sre.LoadGrammar(SGM_FUNC_CountdownYesNo);
                            break;

                        case "SGM_FUNC_CountdownYesNo":
                            sre.UnloadGrammar(SGM_FUNC_CountdownYesNo);
                            break;

                        //////////////////////////////////////////////
                        case "SGM_FUNC_COMPLEX_SetReminder":
                            //      foreach (Grammar DIALGrammar  in DIAL)
                            //      {
                            //         sre.UnloadGrammar(DIALGrammar);
                            //    }
                            sre.UnloadGrammar(SGM_DIAL_SayHello);
                            sre.UnloadGrammar(SGM_DIAL_LookAtMe);
                            sre.LoadGrammar(SGM_FUNC_COMPLEX_SetReminderYesNo);
                            break;

                        case "SGM_FUNC_COMPLEX_SetReminderYesNo":
                            sre.UnloadGrammar(SGM_FUNC_COMPLEX_SetReminderYesNo);
                            sre.LoadGrammar(SGM_DIAL_SayHello);
                            sre.LoadGrammar(SGM_DIAL_LookAtMe);

                            //   foreach (Grammar DIALGrammar in DIAL)
                            //    {
                            //        sre.LoadGrammar(DIALGrammar);
                            //     }
                            break;

                        ////////////////////////////////////////////////
                        case "SGM_FUNC_ControlTV":

                            if (IRCommand_flag == false)
                            {
                                foreach (Grammar SubLayer_IRCommand in IRCommand)
                                {
                                    sre.LoadGrammar(SubLayer_IRCommand);
                                }
                                IRCommand_flag = true;
                            }
                            else if (IRCommand_flag == true)
                            {

                            }
                            break;

                        case "SGM_FUNC_ControlProjector":
                            if (IRCommand_flag == false)
                            {
                                foreach (Grammar SubLayer_IRCommand in IRCommand)
                                {
                                    sre.LoadGrammar(SubLayer_IRCommand);
                                }
                                IRCommand_flag = true;
                            }
                            else if (IRCommand_flag == true)
                            {

                            }
                            break;

                        case "SGM_FUNC_ControlFan":
                            if (IRCommand_flag == false)
                            {
                                foreach (Grammar SubLayer_IRCommand in IRCommandFan)
                                {
                                    sre.LoadGrammar(SubLayer_IRCommand);
                                }
                                IRCommand_flag = true;
                            }
                            else if (IRCommand_flag == true)
                            {

                            }

                            break;

                        case "SGM_FUNC_ControlRadio":
                            if (IRCommand_flag == false)
                            {
                                foreach (Grammar SubLayer_IRCommand in IRCommandRadio)
                                {
                                    sre.LoadGrammar(SubLayer_IRCommand);
                                }
                                IRCommand_flag = true;
                            }
                            else if (IRCommand_flag == true)
                            {

                            }

                            break;



                        ///////////////////////////////////////////////////



                        case "SGM_FUNC_SkypePhoneCallFinished":
                            if (Function.Vision.UserName != "Unknown")
                            {
                                sre.UnloadGrammar(SGM_FUNC_SkypePhoneCallFinished);
                            }
                            //foreach (Grammar Greeting in DIAL)
                            //{
                            //    sre.LoadGrammar(Greeting);
                            //}
                            //foreach (Grammar Greeting in Layerone)
                            //{
                            //    sre.LoadGrammar(Greeting);
                            //}
                            break;

                        case "SGM_FUNC_Call":
                            //foreach (Grammar Greeting in DIAL)
                            //{
                            //    sre.UnloadGrammar(Greeting);
                            //}
                            //foreach (Grammar Greeting in Layerone)
                            //{
                            //    sre.UnloadGrammar(Greeting);
                            //}
                            //sre.UnloadAllGrammars();
                            sre.LoadGrammar(SGM_FUNC_CallPerson);
                            break;

                        case "SGM_FUNC_CallPerson":
                            sre.UnloadGrammar(SGM_FUNC_CallPerson);
                            sre.LoadGrammar(SGM_FUNC_SkypePhoneCallFinished);
                            MouthMuteMark = true;
                            break;
                        //////////////////////////////////////////////////////

                        case "SGM_FUNC_SendMessage":
                            sre.LoadGrammar(SGM_FUNC_SendMessageOption);
                            break;

                        case "SGM_FUNC_SendMesageOption":
                            if (ResultName == "SMS")
                            {
                                foreach (Grammar Layer1 in Layerone)
                                {
                                    sre.UnloadGrammar(Layer1);
                                }
                                sre.UnloadGrammar(SGM_FUNC_SendMessageOption);
                                sre.LoadGrammar(SGM_FUNC_SMSCountry);
                            }

                            if (ResultName == "message")
                            {

                            }
                            break;


                        case "SGM_FUNC_SendSMS":
                            foreach (Grammar Greeting in DIAL)
                            {
                                sre.UnloadGrammar(Greeting);
                            }
                            foreach (Grammar Layer1 in Layerone)
                            {
                                sre.UnloadGrammar(Layer1);
                            }
                            sre.LoadGrammar(SGM_FUNC_SMSCountry);
                            break;

                        case "SGM_FUNC_SMSCountry":
                            sre.UnloadGrammar(SGM_FUNC_SMSCountry);
                            sre.LoadGrammar(SGM_FUNC_SMSPhoneNumber);
                            break;

                        case "SGM_FUNC_SMSPhoneNumber":


                            //if (onetimeGrammar2 == false)
                            //{
                            //    sre.LoadGrammar(SGM_FUNC_SendSMSYesOrNo);
                            //    onetimeGrammar2 = true;
                            //}
                            break;

                        case "SGM_FUNC_SendSMSYesOrNo":
                            //sre.UnloadAllGrammars();
                            sre.LoadGrammar(SGM_FUNC_SpeechToTextMessageFinished);
                            //onetimeGrammar2 = false;
                            break;

                        case "SGM_FUNC_SpeechToTextMessageFinished":
                            sre.UnloadGrammar(SGM_FUNC_SendSMSYesOrNo);
                            sre.UnloadGrammar(SGM_FUNC_SpeechToTextMessageFinished);
                            foreach (Grammar LayerOne in Layerone)
                            {
                                sre.LoadGrammar(LayerOne);
                            }
                            foreach (Grammar Greeting in DIAL)
                            {
                                sre.UnloadGrammar(Greeting);
                            }
                            break;

                        case "SGM_GAME_PlayAnimalLegCounting":
                            //sre.LoadGrammar(SGM_GAME_PlayAnimalLegCountingYesNo);
                            if (Game_flag == false)
                            {
                                sre.LoadGrammar(SGM_GAME_AnimalLegCountingAnswer);
                                sre.LoadGrammar(SGM_GAME_AnimalLegCountingAnswerDontKnow);
                                sre.LoadGrammar(SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo);
                                sre.LoadGrammar(SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo);
                                Game_flag = true;
                            }
                            else
                            {

                            }
                            break;

                        case "SGM_GAME_PlayAnimalLegCountingYesNo":
                            sre.UnloadGrammar(SGM_GAME_PlayAnimalLegCountingYesNo);
                            sre.LoadGrammar(SGM_GAME_AnimalLegCountingAnswer);
                            sre.LoadGrammar(SGM_GAME_AnimalLegCountingAnswerDontKnow);
                            sre.LoadGrammar(SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo);
                            sre.LoadGrammar(SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo);
                            break;

                        case "SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo":
                            sre.UnloadGrammar(SGM_GAME_AnimalLegCountingAnswer);
                            sre.UnloadGrammar(SGM_GAME_AnimalLegCountingAnswerDontKnow);
                            sre.UnloadGrammar(SGM_GAME_NEXT_SGM_GAME_PlayAnimalLegCountingYesNo);
                            sre.UnloadGrammar(SGM_GAME_STOP_SGM_GAME_PlayAnimalLegCountingYesNo);
                            break;

                        case "SGM_DIAL_SwitchLanguageToChinese":
                            sre.UnloadGrammar(SGM_DIAL_SwitchLanguageToChinese);
                            sre.LoadGrammar(SGM_DIAL_SwitchLanguageToChinese_YesNo);
                            MainWindow.GrammarTimer.Start();
                            break;

                        case "SGM_DIAL_SwitchLanguageToChinese_YesNo":
                            sre.UnloadGrammar(SGM_DIAL_SwitchLanguageToChinese_YesNo);
                            MainWindow.GrammarTimer.Stop();
                            sre.LoadGrammar(SGM_DIAL_SwitchLanguageToChinese);
                            break;

                        case "SGM_DIAL_SwitchLanguageToJapanese":
                            sre.UnloadGrammar(SGM_DIAL_SwitchLanguageToJapanese);
                            sre.LoadGrammar(SGM_DIAL_SwitchLanguageToJapanese_YesNo);
                            MainWindow.GrammarTimer.Start();
                            break;

                        case "SGM_DIAL_SwitchLanguageToJapanese_YesNo":
                            sre.UnloadGrammar(SGM_DIAL_SwitchLanguageToJapanese_YesNo);
                            MainWindow.GrammarTimer.Stop();
                            sre.LoadGrammar(SGM_DIAL_SwitchLanguageToJapanese);
                            break;

                        case "SGM_FUNC_RegisterMode_YesNo":
                            sre.UnloadGrammar(SGM_FUNC_RegisterMode_YesNo);
                            MainWindow.GrammarTimer.Stop();
                            break;

                        case "RegisterModeCompleted":
                            foreach (Grammar Greeting in DIAL)
                            {
                                sre.LoadGrammar(Greeting);
                            }
                            foreach (Grammar Layer1 in Layerone)
                            {
                                sre.LoadGrammar(Layer1);
                            }
                            break;

                        case "SGM_DIAL_Sleep":
                            sre.UnloadGrammar(SGM_DIAL_Sleep);
                            sre.LoadGrammar(SGM_DIAL_Sleep_YesNo);
                            MainWindow.GrammarTimer.Start();
                            break;

                        case "SGM_DIAL_Sleep_YesNo":
                            sre.UnloadGrammar(SGM_DIAL_Sleep_YesNo);
                            MainWindow.GrammarTimer.Stop();
                            sre.LoadGrammar(SGM_DIAL_Sleep);
                            break;

                        case "SGM_DIAL_AskRobotName":
                            sre.LoadGrammar(SGM_DIAL_AskRobotNameAns);
                            sre.UnloadGrammar(SGM_DIAL_SayHello);
                            sre.UnloadGrammar(SGM_DIAL_Raise_Right_Hand);
                            break;

                        case "SGM_DIAL_AskRobotNameAns":
                            sre.UnloadGrammar(SGM_DIAL_AskRobotNameAns);
                            sre.LoadGrammar(SGM_DIAL_SayHello);
                            sre.LoadGrammar(SGM_DIAL_Raise_Right_Hand);
                            break;
                        //////////////////////////////////////////////////////////////////////////////////////////////

                        case "SGM_FUNC_CHN_控制电视":
                            foreach (Grammar 遥控 in 控制)
                            {
                                srecn.LoadGrammar(遥控);
                            }
                            break;

                        case "SGM_DIAL_英文识别":
                            srecn.LoadGrammar(SGM_DIAL_英文识别_是否);
                            srecn.UnloadGrammar(SGM_DIAL_英文识别);
                            break;

                        case "SGM_DIAL_英文识别_是否":
                            srecn.UnloadGrammar(SGM_DIAL_英文识别_是否);
                            srecn.LoadGrammar(SGM_DIAL_英文识别);
                            break;

                        case "SGM_DIAL_日文识别":
                            srecn.LoadGrammar(SGM_DIAL_日文识别_是否);
                            srecn.UnloadGrammar(SGM_DIAL_日文识别);
                            break;

                        case "SGM_DIAL_日文识别_是否":
                            srecn.UnloadGrammar(SGM_DIAL_日文识别_是否);
                            srecn.LoadGrammar(SGM_DIAL_日文识别);
                            break;

                        case "SGM_FUNC_问问题":
                            srecn.UnloadAllGrammars();
                            break;

                        case "SGM_FUNC_计算":
                            srecn.UnloadAllGrammars();
                            break;

                        case "SGM_FUNC_问问题_完成":
                            foreach (Grammar 中文 in 中文语法)
                            {
                                srecn.LoadGrammar(中文);
                            }
                            srecn.RecognizeAsync(RecognizeMode.Multiple);
                            break;

                        //case "SGM_FUNC_打电话":
                        //    srecn.LoadGrammar(SGM_FUNC_打电话_是否);
                        //    break;

                        case "SGM_FUNC_打电话_是否":
                            srecn.UnloadGrammar(SGM_FUNC_打电话_是否);
                            if (ResultName == "是的" || ResultName == "是")
                            {
                                srecn.LoadGrammar(SGM_FUNC_打电话_结束);
                            }
                            break;

                        case "SGM_FUNC_打电话_结束":
                            srecn.UnloadAllGrammars();
                            ChineseGrammarLoad();
                            break;

                        //case "SGM_FUNC_发简讯":
                        //    srecn.LoadGrammar(SGM_FUNC_发简讯_是否);
                        //    break;

                        case "SGM_FUNC_发简讯_是否":
                            srecn.UnloadGrammar(SGM_FUNC_发简讯_是否);
                            break;



                        ////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////

                        case "SGM_FUNC_中国語に切り替え":
                            srejp.LoadGrammar(SGM_FUNC_中国語に切り替え_確認);
                            srejp.UnloadGrammar(SGM_FUNC_中国語に切り替え);
                            break;

                        case "SGM_FUNC_中国語に切り替え_確認":
                            srejp.UnloadGrammar(SGM_FUNC_中国語に切り替え_確認);
                            srejp.LoadGrammar(SGM_FUNC_中国語に切り替え);
                            break;

                        case "SGM_FUNC_英語に切り替え":
                            srejp.LoadGrammar(SGM_FUNC_英語に切り替え_確認);
                            srejp.UnloadGrammar(SGM_FUNC_英語に切り替え);
                            break;

                        case "SGM_FUNC_英語に切り替え_確認":
                            srejp.UnloadGrammar(SGM_FUNC_英語に切り替え_確認);
                            srejp.LoadGrammar(SGM_FUNC_英語に切り替え);
                            break;

                        case "SGM_FUNC_おやすみ":
                            srejp.LoadGrammar(SGM_FUNC_おやすみ_確認);
                            srejp.UnloadGrammar(SGM_FUNC_おやすみ);
                            break;

                        case "SGM_FUNC_おやすみ_確認":
                            srejp.UnloadGrammar(SGM_FUNC_おやすみ_確認);
                            srejp.LoadGrammar(SGM_FUNC_おやすみ);
                            break;

                        ////////////////////////////////////////
                        case "SGM_FUNC_StartRadioStaion":
                            sre.UnloadGrammar(SGM_FUNC_StartRadioStaion);
                            sre.LoadGrammar(SGM_FUNC_StartRadioStationYesNo);
                            break;

                        case "SGM_FUNC_StartRadioStationYesNo":
                            sre.UnloadGrammar(SGM_FUNC_StartRadioStationYesNo);
                            sre.LoadGrammar(SGM_FUNC_StopRadioStation);
                            break;

                        case "SGM_FUNC_StopRadioStation":
                            sre.UnloadGrammar(SGM_FUNC_StopRadioStation);
                            sre.LoadGrammar(SGM_FUNC_StopRadioStationYesNo);
                            break;

                        case "SGM_FUNC_StopRadioStationYesNo":
                            sre.UnloadGrammar(SGM_FUNC_StopRadioStationYesNo);
                            sre.LoadGrammar(SGM_FUNC_StartRadioStaion);
                            break;






                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            /*                  case "SGM_FUNC_Skype電話_国":
                                                    srejp.UnloadGrammar(SGM_FUNC_Skype電話_国);
                                                    srejp.LoadGrammar(SGM_FUNC_Skype電話_電話番号);
                                                    break;

                                                case "SGM_FUNC_Skype電話_電話番号":

                                                    //if (onetimeGrammar1 == false)
                                                    //{
                                                    //    sre.LoadGrammar(SGM_FUNC_SkypeCall_PhoneCall_YesNo);
                                                    //    onetimeGrammar1 = true;
                                                    //}

                                                    break;

                                                case "SGM_FUNC_Skype電話_電話番号_確認":
                                                    srejp.LoadGrammar(SGM_FUNC_Skype電話終了);
                                                    //sre.UnloadGrammar(SGM_FUNC_SkypeCall_PhoneNumber);
                                                    srejp.UnloadGrammar(SGM_FUNC_Skype電話_電話番号_確認);
                                                    //onetimeGrammar1 = false;                   
                                                    break;

                                                case "SGM_FUNC_Skype電話_終了":
                                                    srejp.UnloadGrammar(SGM_FUNC_Skype電話終了);
                                                    break;
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            */
                    }
                }
            }

            catch
            {
                //srespeech.SpeakAsync("Load Unload Grammar Error");
            }
        }

        void AudioLevelUpdate()
        {
            sre.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(sre_AudioLevelUpdated);
        }

        private void sre_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            sre.AudioLevelUpdated -= sre_AudioLevelUpdated;
        }


        //Speak Progress [function]

        public void srespeech_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            MainWindow.FacialExpressionMark = false;
            try
            {
                if(!MouthMuteMark)
                {
                    FaceLED.Instance.Speaking();
                }
            }
            catch { }
            try
            {
                sre.RecognizeAsyncCancel();
            }
            catch
            {
                srespeech.SpeakAsync("Speech recognition cancel error");
            }

        }

        public void srespeech_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            MainWindow.FacialExpressionMark = true;
            srespeech.SpeakCompleted -= srespeech_SpeakCompleted;
            //flag_speak_completed = true;
            // MessageBox.Show("complete");
            try
            {
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                //MessageBox.Show("Speech recogition start error");
            }
            srespeech.SpeakCompleted += srespeech_SpeakCompleted;
            MouthMuteMark = false;
        }



        public void srecnspeech_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            try
            {
                FaceLED.Instance.Speaking();
             
            }
            catch { }
            try
            {
                srecn.RecognizeAsyncCancel();
            }
            catch
            {
                srecnspeech.SpeakAsync("识别引擎取消错误");
            }
        }

        public void srecnspeech_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            try
            {
                srecn.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                srecnspeech.SpeakAsync("识别引擎打开错误");
            }
        }

        public void srejpspeech_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            try
            {
                FaceLED.Instance.Speaking();
            }
            catch { }
            try
            {
                srejp.RecognizeAsyncCancel();
            }
            catch
            {
                srejpspeech.SpeakAsync("認識エンジンをキャンセルしたことが失敗しました。");
            }
        }

        public void srejpspeech_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            try
            {
                srejp.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                srejpspeech.SpeakAsync("認識エンジンをスタットしたことが失敗しました。");
            }
        }

        public void srecanspeech_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            try
            {
                FaceLED.Instance.Speaking();
            }
            catch { }
            try
            {
                srecan.RecognizeAsyncCancel();
            }
            catch
            {
                srecanspeech.SpeakAsync("识别引擎取消错误");
            }
        }

        public void srecanspeech_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            try
            {
                srecan.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                srecanspeech.SpeakAsync("识别引擎打开错误");
            }
        }


        public Grammar SGM_FUNC_Skype電話 { get; set; }
    }
}
