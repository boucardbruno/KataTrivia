using System;
using Trivia;

namespace TriviaRunner
{
    public static class GameRunner
    {

        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            Game aGame = new Game();

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            Random rand = new Random();

            do
            {

                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    _notAWinner = aGame.WrongAnswer();
                }
                else
                {
                    _notAWinner = aGame.WasCorrectlyAnswered();
                }



            } while (_notAWinner);

        }


    }

}

