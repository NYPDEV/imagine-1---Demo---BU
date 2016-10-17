using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace SocialRobot.Application
{
    public class DateTimeApp
    {

        public Function.Speech_Rcognition_Grammar Speak = new Function.Speech_Rcognition_Grammar();
        public Application.musicplayer music = new Application.musicplayer();

        string Time_now = "";
        string Date_now = "";
        string Day_now = "";

        string hour_now;
        string minute_now;
        string month_now;
        string date_now;
        string day_now;
        string 星期;
        string 曜日;



        public void timeNow(string language)
        {
            
            if (language == "eng")
            {
                Speak.srespeech.SelectVoice("IVONA 2 Amy");
                //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
                Time_now = DateTime.Now.ToString("hh") + ":" + DateTime.Now.ToString("mm") + " " + DateTime.Now.ToString("tt");
                Speak.srespeech.SpeakAsync("The time is " + Time_now);
            }

            if (language == "cn")
            {
                Speak.srecnspeech.SelectVoice("Microsoft Huihui Desktop");
                //   Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                hour_now = DateTime.Now.Hour.ToString();
                minute_now = DateTime.Now.Minute.ToString();
                Speak.srecnspeech.SpeakAsync("现在是" + hour_now + "点" + minute_now + "分");
            }

            if (language == "can")
            {
                Speak.srecanspeech.SelectVoice("Microsoft Tracy Desktop");
                //   Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                hour_now = DateTime.Now.Hour.ToString();
                minute_now = DateTime.Now.Minute.ToString();
                Speak.srecanspeech.SpeakAsync("依家" + hour_now + "点" + minute_now + "分");
            }
        }

        public void dateNow(string language)
        {
            if (language == "eng")
            {
                Date_now = DateTime.Now.ToString("M");
                Speak.srespeech.SelectVoice("IVONA 2 Amy");
                //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
                Speak.srespeech.SpeakAsync("Today is " + Date_now);
            }

            if (language == "cn")
            {
                Speak.srecnspeech.SelectVoice("Microsoft Huihui Desktop");
                //   Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                month_now = DateTime.Now.Month.ToString();
                Date_now = DateTime.Now.ToString("MM") + "月" + DateTime.Now.ToString("dd") + "日";
                Speak.srecnspeech.SpeakAsync("今天是" + Date_now);
            }

            if (language == "jp")
            {
                //  Speak.srecnspeech.SelectVoice("VW Liang");
                //   Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                //Speak.srejpspeech.SelectVoice("VW Misaki");
                string 月 = DateTime.Now.ToString("MM");
                string 日 = DateTime.Now.ToString("dd");
                //Speak.srejpspeech.SpeakAsync(月);
               switch (月)
               {
                   case "1":
                       月 = "一月";
                       break;
                   case "2":
                       月 = "二月";
                       break;
                   case "3":
                       月 = "三月";
                       break;
                   case "4":
                       月 = "四月";
                       break;
                   case "5":
                       月 = "五月";
                       break;
                   case "6":
                       月 = "六月";
                       break;
                   case "7":
                       月 = "七月";
                       break;
                   case "8":
                       月 = "八月";
                       break;
                   case "9":
                       月 = "九月";
                       break;
                   case "10":
                       月 = "十月";
                       break;
                   case "11":
                       月 = "十一月";
                       break;
                   case "12":
                       月 = "十二月";
                       break;

               }
               
                Speak.srejpspeech.SpeakAsync("今日は" + 月 +"月"+ 日+"日" + "です");
            }

            if (language == "can")
            {
                Speak.srecanspeech.SelectVoice("Microsoft Tracy Desktop");
                month_now = DateTime.Now.Month.ToString();
                Date_now = DateTime.Now.ToString("MM") + "月" + DateTime.Now.ToString("dd") + "日";
                Speak.srecanspeech.SpeakAsync("今日是" + Date_now);
            }
        }

        public void dayNow(string language)
        {
            if (language == "eng")
            {
                Day_now = DateTime.Now.ToString("dddd");
                Speak.srespeech.SelectVoice("IVONA 2 Amy");
                //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
                Speak.srespeech.SpeakAsync("Today is " + Day_now);
            }

            else if (language == "cn")
            {
                Speak.srecnspeech.SelectVoice("Microsoft Huihui Desktop");
                //    Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                day_now = DateTime.Now.DayOfWeek.ToString();
                switch (day_now)
                {
                    case "Monday":
                        星期 = "星期一";
                        break;

                    case "Tuesday":
                        星期 = "星期二";
                        break;

                    case "Wednesday":
                        星期 = "星期三";
                        break;

                    case "Thursday":
                        星期 = "星期四";
                        break;

                    case "Friday":
                        星期 = "星期五";
                        break;

                    case "Saturday":
                        星期 = "星期六";
                        break;

                    case "Sunday":
                        星期 = "星期天";
                        break;

                }
                Speak.srecnspeech.SpeakAsync("今天是" + 星期);
            }

            else if (language == "can")
            {
                Speak.srecanspeech.SelectVoice("Microsoft Tracy Desktop");
                //    Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                day_now = DateTime.Now.DayOfWeek.ToString();
                switch (day_now)
                {
                    case "Monday":
                        星期 = "星期一";
                        break;

                    case "Tuesday":
                        星期 = "星期二";
                        break;

                    case "Wednesday":
                        星期 = "星期三";
                        break;

                    case "Thursday":
                        星期 = "星期四";
                        break;

                    case "Friday":
                        星期 = "星期五";
                        break;

                    case "Saturday":
                        星期 = "星期六";
                        break;

                    case "Sunday":
                        星期 = "星期天";
                        break;

                }
                Speak.srecanspeech.SpeakAsync("今日是" + 星期);
            }


            else if (language == "hok")
            {
                //Speak.srecnspeech.SelectVoice("Microsoft Huihui Desktop");
                //    Speak.srecnspeech.SetOutputToDefaultAudioDevice();
                day_now = DateTime.Now.DayOfWeek.ToString();
                switch (day_now)
                {
                    case "Monday":
                        星期 = "星期一";
                        music.music("今天是星期一");
                        break;

                    case "Tuesday":
                        星期 = "星期二";
                        music.music("今天是星期二");
                        break;

                    case "Wednesday":
                        星期 = "星期三";
                        music.music("今天是星期三");
                        break;

                    case "Thursday":
                        星期 = "星期四";
                        music.music("今天是星期四");
                        break;

                    case "Friday":
                        星期 = "星期五";
                        music.music("今天是星期五");
                        break;

                    case "Saturday":
                        星期 = "星期六";
                        music.music("今天是星期六");
                        break;

                    case "Sunday":
                        星期 = "星期天";
                        music.music("今天是星期天");
                        break;

                }
              
            }

        }
    }

}

