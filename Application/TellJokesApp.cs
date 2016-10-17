using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Speech.Synthesis;
using Newtonsoft.Json.Linq;


namespace SocialRobot.Application
{
   public class TellJokesApp
    {
        public Function.Speech_Rcognition_Grammar Speak = new Function.Speech_Rcognition_Grammar();

        public async void RandomJokesAsync()
        {
            Speak.srespeech.SelectVoice("IVONA 2 Amy");
            //Speak.srespeech.SelectVoice("Microsoft Zira Desktop");
            Speak.srespeech.SpeakAsync("Hold on please.");
            string temp_jokes_inside = await Task.Run(() => RandomJokes());

            Speak.srespeech.SpeakAsync(temp_jokes_inside);
        }

        private string RandomJokes()
        {
            HttpClient client = new HttpClient();
            string query_jokes = string.Format("http://api.icndb.com/jokes/random?firstName=Adam&amp&lastName=");
            string json_jokes = client.GetStringAsync(query_jokes).Result;
            JObject jokes_chuck = JObject.Parse(json_jokes);
            string jokes_inside = (string)jokes_chuck["value"]["joke"];

            return jokes_inside;
        }
    }
}
