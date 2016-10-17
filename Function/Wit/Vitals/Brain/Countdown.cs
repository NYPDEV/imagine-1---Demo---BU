﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialRobot.Wit.Vitals.Brain
{
    class Countdown
    {
        private Objects.O_NLP.RootObject o_NLP = new Objects.O_NLP.RootObject();
        double conf = 0D;

        public string makeSentence(Objects.O_NLP.RootObject _o_NLP)
        {
            try
            {
                // Bind to the wit.ai NLP response class
                o_NLP = _o_NLP;
                conf = (o_NLP.outcomes.confidence * 100);

                string sentence = "";
               // string Day = o_NLP.outcomes.entities.day[0].value;
              //  string Country = o_NLP.outcomes.entities.location[0].value;
                string normalized = o_NLP.outcomes.entities.duration[0].normalized[0].value;
                sentence += Environment.NewLine + "You want: " + normalized;   
                return sentence;

            }
            catch (Exception ex)
            {
                return "Uh oh, something went wrong: " + ex.Message;
            }
        }
    }
}
