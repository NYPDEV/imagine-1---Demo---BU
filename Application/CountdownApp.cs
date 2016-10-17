using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Synthesis;

namespace SocialRobot.Application
{
    public class CountdownApp
    {
        public Function.Speech_Rcognition_Grammar Speak = new Function.Speech_Rcognition_Grammar();

        public string conversion = "";
        public string currentcountdown = "";
        bool flag_countdown_started = false;
        public string countdown_state = "";

        public string TimeConversion(string time)
        {
            currentcountdown = time;
            int tempsecs = 0;
            int tempminutes = 0;
            int temphours = 0;
            string tempstring = "";
            tempsecs = Convert.ToInt32(time);
            for (; ; )
            {
                if (tempsecs < 60)
                {
                    break;
                }
                else if (tempsecs >= 3600)
                {
                    tempsecs = tempsecs - 3600;
                    temphours++;
                }
                else if (tempsecs >= 60)
                {
                    tempsecs = tempsecs - 60;
                    tempminutes++;
                }

            }

            if (temphours == 0 && tempminutes == 0)
            {

                tempstring = tempsecs + " seconds";
            }
            else if (temphours == 0 && tempminutes != 0)
            {
                if (tempsecs == 0)
                {
                    tempstring = tempminutes + " minutes";
                }
                else if (tempsecs != 0)
                {
                    tempstring = tempminutes + " minutes " + tempsecs + " seconds ";
                }
            }
            else if (temphours != 0)
            {
                if (tempminutes != 0)
                {
                    if (tempsecs == 0)
                    {
                        tempstring = temphours + " hours " + tempminutes + " minutes";
                    }
                    else if (tempsecs != 0)
                    {
                        tempstring = temphours + " hours " + tempminutes + " minutes " + tempsecs + " seconds ";
                    }
                }
                else if (tempminutes == 0)
                {
                    if (tempsecs == 0)
                    {
                        tempstring = temphours + " hours";
                    }
                    else if (tempsecs != 0)
                    {
                        tempstring = temphours + " hours " + tempsecs + " seconds ";
                    }
                }
            }

            //tempstring = tempsecs.ToString();
            return tempstring;
        }


       public async void CountDownFunction()
        {
            flag_countdown_started = true;
            int temppp = 0;
            temppp = Convert.ToInt32(currentcountdown);
            await Task.Run(() => CountDownTimer(temppp));
        }

        void CountDownTimer(int time)
        {
            int x;
            int c = 0;
            int five;
            for (x = 0; x < time; x++)
            {
                Thread.Sleep(900);
                c = time - x;
                five = c % 5;

                if (flag_countdown_started == false)
                {
                    break;
                }

                if (c <= 10)
                {
                    CountDownAnnouncement(c);
                }
                else if (c != 10 && five == 0 && c < 60)
                {
                    CountDownAnnouncement(c + " seconds");
                }
                else if (c >= 60 && five == 0)
                {
                    string ctostring = c.ToString();
                    string justread = TimeConversion(ctostring);
                    CountDownAnnouncement(justread);
                }

            }
            Speak.srespeech.SelectVoice("IVONA 2 Amy");
            //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
            Speak.srespeech.SpeakAsync("Countdown finished.");
            countdown_state = "finished";
        }

       

        void CountDownAnnouncement(int input)
        {
            Speak.srespeech.SelectVoice("IVONA 2 Amy");
            //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
            Speak.srespeech.SpeakAsync(input + "");
        }
        void CountDownAnnouncement(string input)
        {
            Speak.srespeech.SelectVoice("IVONA 2 Amy");
            //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
            Speak.srespeech.SpeakAsync(input + "");
        }




    }
}
