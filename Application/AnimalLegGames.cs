using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialRobot.Application
{
   public class AnimalLegGames
    {

        Function.Speech_Rcognition_Grammar Speech = new Function.Speech_Rcognition_Grammar();

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
            Speech.srespeech.SelectVoice("IVONA 2 Amy");
            //Speech.srespeech.SelectVoice("Microsoft Zira Desktop");
            Speech.srespeech.SpeakAsync("How many legs do " + animal_number1 + " " + animals_game[n1_animals] + " and " + animal_number2 + " " + animals_game[n2_animals] + " have?");
            total_leg_game = animal_number1 * leg_1 + animal_number2 * leg_2;
        //    flag_speak_completed = false;

        }

        public void AnimalLegCounting_CheckAnswer(int leg_answer)
        {
            if (leg_answer == total_leg_game)
            {
             //   flag_speak_completed = false;
                Speech.srespeech.SelectVoice("IVONA 2 Amy");
                //Speech.srespeech.SelectVoice("Microsoft Zira Desktop");
                Speech.srespeech.SpeakAsync("Correct, Let's try more");
                AnimalLegCountingGame();
            }
            else
            {
                //flag_speak_completed = false;
                Speech.srespeech.SelectVoice("IVONA 2 Amy");
                //Speech.srespeech.SelectVoice("Microsoft Zira Desktop");
                Speech.srespeech.SpeakAsync("Wrong answer. It's" + total_leg_game + ". Let's try again");
                AnimalLegCountingGame();
            }
        }





    }
}
