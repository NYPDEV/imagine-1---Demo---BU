using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml;
using System.Text.RegularExpressions;


namespace SocialRobot.Application
{
   public class Spotify
    {

        bool flag_spotifystart = false;
        bool flag_search_song = false;

       public Function.Speech_Rcognition_Grammar Speak = new Function.Speech_Rcognition_Grammar();
        public void SpotifyRadio(int CurrentGenreIndex)
        {
            switch (CurrentGenreIndex)
            {
                case 1:
                    System.Diagnostics.Process.Start("spotify:radio:genre:alternative");
                    break;
                case 2:
                    System.Diagnostics.Process.Start("spotify:radio:genre:blues");
                    break;
                case 3:
                    System.Diagnostics.Process.Start("spotify:radio:genre:classical");
                    break;
                case 4:
                    System.Diagnostics.Process.Start("spotify:radio:genre:country");
                    break;
                case 5:
                    System.Diagnostics.Process.Start("spotify:radio:genre:emo");
                    break;
                case 6:
                    System.Diagnostics.Process.Start("spotify:radio:genre:dance");
                    break;
                case 7:
                    System.Diagnostics.Process.Start("spotify:radio:genre:latin");
                    break;
                case 8:
                    System.Diagnostics.Process.Start("spotify:radio:genre:folk");
                    break;
                case 9:
                    System.Diagnostics.Process.Start("spotify:radio:genre:indie");
                    break;
                case 10:
                    System.Diagnostics.Process.Start("spotify:radio:genre:jazz");
                    break;
                case 11:
                    System.Diagnostics.Process.Start("spotify:radio:genre:pop");
                    break;
                case 12:
                    System.Diagnostics.Process.Start("spotify:radio:genre:soul");
                    break;
                case 13:
                    System.Diagnostics.Process.Start("spotify:radio:genre:60s");
                    break;
                case 14:
                    System.Diagnostics.Process.Start("spotify:radio:genre:00s");
                    break;
            }
        }

        public void ContinuePlaySpotify()
        {
            if (flag_spotifystart == false)
            {
                flag_spotifystart = true;
                SwitchWindow_toSpotify();
                InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
            }

        }
        public void PauseSpotify()
        {
            if (flag_spotifystart == true)
            {
                {
                    SwitchWindow_toSpotify();
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                    flag_spotifystart = false;
                }
            }
        }

        public void NextMusicSpotify()
        {
            if (flag_spotifystart == true)
            {
                SwitchWindow_toSpotify();
                InputSimulator.SimulateKeyDown(VirtualKeyCode.LCONTROL);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
                Thread.Sleep(1000);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.LCONTROL);
            }
        }

        public void VolumeDown_Spotify()
        {
            SwitchWindow_toSpotify();
            InputSimulator.SimulateKeyDown(VirtualKeyCode.LCONTROL);
            InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
            Thread.Sleep(500);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.LCONTROL);

        }
        public void VolumeUp_Spotify()
        {
            SwitchWindow_toSpotify();
            InputSimulator.SimulateKeyDown(VirtualKeyCode.LCONTROL);
            InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
            Thread.Sleep(500);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.LCONTROL);
        }


        [DllImport("user32.dll")]
        static extern void SwitchToThisWindow(IntPtr hWnd);

        String ProcWindow_spotify = "spotify";
        String ProcWindow_robot = "vshost32.exe";
        String ProcWindow_skype = "skype";

        public void SwitchWindow_toSpotify()
        {
            Process[] procs = Process.GetProcessesByName(ProcWindow_spotify);
            ProcessStartInfo start_spotify = new ProcessStartInfo();
            start_spotify.FileName = @"C:\Users\He\AppData\Roaming\Spotify\spotify.exe";
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
                Process.Start(start_spotify);
                Thread.Sleep(3000);
            }

        }

        public async void SearchSong(string input)
        {
            await Task.Run(() => spotify_track(input));
        }

        public void spotify_track(string spotify_track_search)//yihao
        {

            spotify_track_search = Regex.Replace(spotify_track_search, @" ", "%20", RegexOptions.IgnoreCase);
            string query = string.Format("https://query.yahooapis.com/v1/public/yql?q=SELECT%20*%20FROM%20spotify.search.track%20where%20track%20%3D%20'" + spotify_track_search + "'%3B&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            XmlDocument wData = new XmlDocument();


            try
            {
                wData.Load(query);
                XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").FirstChild;

                string track_name = channel.FirstChild.InnerText;
                string track_id = channel.Attributes["href"].Value;              
                Speak.srespeech.SpeakAsync("I will play" + track_name);
                Thread.Sleep(4000);
                System.Diagnostics.Process.Start(track_id);
                flag_search_song = false;
                flag_spotifystart = true;
            }
            catch
            {
                Speak.srespeech.SpeakAsync("Sorry, I can't provide you that song");
            }
            //    string track_name = channel.SelectSingleNode("name").InnerText;
            //         foreach (XmlNode node in all_tracks_spotify)
            //     {
            //      string track_id = node.Attributes["href"].Value;

            //
            // }

        }

    }
}
