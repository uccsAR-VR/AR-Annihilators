using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class BlackjackGame
    {
        private enum GameResult
        {
            PUSH, PLAYER_WIN, PLAYER_BUST, DEALER_WIN, SURRENDER, CONTINUE_PLAYING
        }
        private enum Face
        {
            Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
        }

        static bool GameLogic(int playerHand, int dealerHand)
        {
            bool keepPlayingGame = true;

            if (playerHand == 0)
            {
                keepPlayingGame = EndGame(GameResult.SURRENDER);
            }
            else if(playerHand > 21)
            {
                keepPlayingGame = EndGame(GameResult.PLAYER_BUST);
            }
            else if(dealerHand <= 16)
            {
                keepPlayingGame = EndGame(GameResult.CONTINUE_PLAYING);
            }
            else if(playerHand > dealerHand && dealerHand != 22)
            {
                keepPlayingGame = EndGame(GameResult.PLAYER_WIN);
            }
            else if (dealerHand > 21 && dealerHand != 22)
            {
                keepPlayingGame = EndGame(GameResult.PLAYER_WIN);
            }
            else if (dealerHand > playerHand && dealerHand != 22)
            {
                keepPlayingGame = EndGame(GameResult.DEALER_WIN);
            }
            else if (playerHand == dealerHand || dealerHand == 22) //No one wins or loses 
            {
                keepPlayingGame = EndGame(GameResult.PUSH);
            }
            return keepPlayingGame;
        }
        static bool EndGame(GameResult result)
        {
            bool keepPlayingGame = true;

            switch (result)
            {
                case GameResult.PUSH:
                    Console.WriteLine("Player and Dealer Push.");
                    keepPlayingGame = false;
                    break;
                case GameResult.PLAYER_WIN:
                    Console.WriteLine("Player Wins ");
                    //play sound and graphics
                    keepPlayingGame = false;
                    break;
                case GameResult.PLAYER_BUST:
                    Console.WriteLine("Player Busts");
                    keepPlayingGame = false;
                    break;
                case GameResult.DEALER_WIN:
                    Console.WriteLine("Dealer Wins.");
                    //play sound and graphics
                    keepPlayingGame = false;
                    break;
                case GameResult.SURRENDER:
                    Console.WriteLine("Player Surrenders ");
                    keepPlayingGame = false;
                    break;
                case GameResult.CONTINUE_PLAYING:
                    keepPlayingGame = true;
                    break;
            }
            return keepPlayingGame;
        }
        public int GetHandValue()
        {
            int cardValue = 0;
            //get card count from yolo
            return cardValue;
        }
        static void Main()
        {
            int playerHand;
            int dealerHand;
            bool playGame = false;
            BlackjackGame game = new BlackjackGame();

            //display start button
            //if start button is pressed play game

            while (playGame)
            {
                //Get player hands
                //Play sound for casino
                //Display probability stats 
                playerHand = game.GetHandValue();
                dealerHand = game.GetHandValue();

                //check the status of the game 
                playGame = GameLogic(playerHand, dealerHand);
            }
        }
    }
}

