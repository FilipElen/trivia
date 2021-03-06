﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trivia;

namespace UglyTrivia
{
    public class Game 
    {
        List<string> players = new List<string>();

        int[] places = new int[6];
        int[] purses = new int[6];

        bool[] inPenaltyBox = new bool[6];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

        private readonly IOutputter _outputter;
        public Game( IOutputter outputter) : this()
        {
            _outputter = outputter;
        }
        public Game()
        {
            const int MAXAMOUNTQUESTIONS = 50;
            for (int i = 0; i < MAXAMOUNTQUESTIONS; i++)
            {
                popQuestions.AddLast("Pop Question " + i);
                scienceQuestions.AddLast(("Science Question " + i));
                sportsQuestions.AddLast(("Sports Question " + i));
                rockQuestions.AddLast(createRockQuestion(i));
            }
        }

        public String createRockQuestion(int currentRockQuestion)
        {
            return "Rock Question " + currentRockQuestion;
        }

        public bool isPlayable()
        {
            const int MAXPLAYERS = 2;
            return (howManyPlayers() >= MAXPLAYERS);
        }

        public bool add(String playerName)
        {
           

            players.Add(playerName);
            places[howManyPlayers()] = 0;
            purses[howManyPlayers()] = 0;
            inPenaltyBox[howManyPlayers()] = false;

            _outputter.GenerateOutput(playerName + " was added");
            _outputter.GenerateOutput("They are player number " + players.Count);
            return true;
        }

        public int howManyPlayers()
        {
            return players.Count;
        }

        public void roll(int roll)
        {
           _outputter.GenerateOutput(players[currentPlayer] + " is the current player");
           _outputter.GenerateOutput("They have rolled a " + roll);
            const int TRIGGERRESETPLACE = 11;
            const int RESETPLACE = 12;

            if (inPenaltyBox[currentPlayer])
            {
                if (IsUneven(roll))
                {
                    isGettingOutOfPenaltyBox = true;

                    _outputter.GenerateOutput(players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > TRIGGERRESETPLACE) places[currentPlayer] = places[currentPlayer] - RESETPLACE;

                    _outputter.GenerateOutput(players[currentPlayer]
                             + "'s new location is "
                             + places[currentPlayer]);
                    _outputter.GenerateOutput("The category is " + currentCategory());
                    askQuestion();
                }
                else
                {
                    _outputter.GenerateOutput(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {

                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > TRIGGERRESETPLACE) places[currentPlayer] = places[currentPlayer] - RESETPLACE;

               _outputter.GenerateOutput(players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
               _outputter.GenerateOutput("The category is " + currentCategory());
                askQuestion();
            }

        }

        private static bool IsUneven(int roll)
        {
            return roll % 2 != 0;
        }

        private void askQuestion()
        {
            if (currentCategory() == "Pop")
            {
               _outputter.GenerateOutput(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (currentCategory() == "Science")
            {
               _outputter.GenerateOutput(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == "Sports")
            {
               _outputter.GenerateOutput(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == "Rock")
            {
               _outputter.GenerateOutput(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }


        private String currentCategory()
        {
            if (places[currentPlayer] == 0) return "Pop";
            if (places[currentPlayer] == 4) return "Pop";
            if (places[currentPlayer] == 8) return "Pop";
            if (places[currentPlayer] == 1) return "Science";
            if (places[currentPlayer] == 5) return "Science";
            if (places[currentPlayer] == 9) return "Science";
            if (places[currentPlayer] == 2) return "Sports";
            if (places[currentPlayer] == 6) return "Sports";
            if (places[currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool wasCorrectlyAnswered()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                   _outputter.GenerateOutput("Answer was correct!!!!");
                    purses[currentPlayer]++;
                   _outputter.GenerateOutput(players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    bool winner = didPlayerWin();
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;
                    return true;
                }



            }
            else
            {

               _outputter.GenerateOutput("Answer was corrent!!!!");
                purses[currentPlayer]++;
               _outputter.GenerateOutput(players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                bool winner = didPlayerWin();
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool wrongAnswer()
        {
           _outputter.GenerateOutput("Question was incorrectly answered");
           _outputter.GenerateOutput(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }


        private bool didPlayerWin()
        {
            return !(purses[currentPlayer] == 6);
        }

        
    }

}
