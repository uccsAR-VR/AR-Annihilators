using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;


public class GameLogic : MonoBehaviour
{

    //void CreateText()
    //{
    // string path = Application.dataPath + "/Log.txt";
    //if (!File.Exists(path))
    //{
    //File.WriteAllText(path, "Login log\n");
    //}
    //}

    /*static void run_cmd(string cmd)
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "C:\\Users\\change\\AppData\\Local\\Programs\\Python\\Python310\\python.exe";
        start.Arguments = string.Format("{0}", cmd);
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                UnityEngine.Debug.Log(result);
            }
        }
    }*/
    public AudioSource winningNoise;
    public AudioSource losingNoise;
    private GameResult result;
    private int playerHand;
    private int dealerHand;
    private bool playGame = false;
        private enum GameResult
        {
            PUSH, PLAYER_WIN, PLAYER_BUST, DEALER_WIN, SURRENDER, CONTINUE_PLAYING
        }

    // Start is called before the first frame update
    //int playerHand;
    //int dealerHand;
    //bool playGame = false;
    //BlackjackGame game = new BlackjackGame();
    void Start()
    {
        //string str1 = Application.dataPath;
        //CreateText();
        string str1 = Application.persistentDataPath;
        UnityEngine.Debug.Log(str1);
        Process p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        //start.FileName = "C:\\Users\\change\\AppData\\Local\\Programs\\Python\\Python310\\python.exe";
        startInfo.FileName = "C:\\WINDOWS\\system32\\cmd.exe";
        startInfo.Arguments = string.Format("{0}", @"python C:\\Users\\change\\source\\unity\\NewUnityHololens2\\Assets\\Scripts\\blackjack.py");
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardInput = true;
        p.StartInfo = startInfo;
        p.Start();

        using (StreamWriter sw = p.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine("cd C:\\Users\\change\\source\\unity\\NewUnityHololens3\\Assets\\Scripts");
                sw.WriteLine("python ./blackjack.py");
            }
        }

        p.WaitForExit();

        /*using (StreamReader sw = p.StandardOutput)
        {
            using (StreamReader reader = p.StandardOutput)
            {
                string result = reader.ReadToEnd();
                UnityEngine.Debug.Log(result);
            }
        }*/

        //Process process2 = Process.Start(start);
        //process2.WaitForExit();
        //}
        //run_cmd("python ./blackjack.py");
        UnityEngine.Debug.Log("cmd ran");
        //C: \Users\change\source\unity\NewUnityHololens - Copy\Assets\Scripts
        //string[] lines = System.IO.File.ReadAllLines(@".\\Assets\\Scripts\\probs.txt");
        string[] lines = File.ReadAllLines(".\\Assets\\Scripts\\probs.txt");
        foreach (string line in lines)
        {
            // Use a tab to indent each line of the file.
            UnityEngine.Debug.Log("\t" + line);
        }

        playGame = true;
        playerHand = 0;
        dealerHand = 0;
    }//end start

    // Update is called once per frame
    void Update()
    {
            //Get player hands
            //Play sound for casino
            //Display probability stats 

            playerHand = 17;
            dealerHand = 21;

            if (playerHand == 0)
            {
                result = GameResult.SURRENDER;
            }
            else if (playerHand > 21)
            {
                result = GameResult.PLAYER_BUST;
            }
            else if (dealerHand <= 16)
            {
                result = GameResult.CONTINUE_PLAYING;
            }
            else if (playerHand > dealerHand && dealerHand != 22)
            {
                winningNoise.Play();
                result = GameResult.PLAYER_WIN;
            }
            else if (dealerHand > 21 && dealerHand != 22)
            {
                result = GameResult.PLAYER_WIN;
            }
            else if (dealerHand > playerHand && dealerHand != 22)
            {
                losingNoise.Play();
                result = GameResult.DEALER_WIN;
            }
            else
            {
                result = GameResult.PUSH;
            }

            switch (result)
            {
                case GameResult.PUSH:
                    UnityEngine.Debug.Log("Player and Dealer Push.");
                    playGame = false;
                    break;
                case GameResult.PLAYER_WIN:
                    UnityEngine.Debug.Log("Player Wins ");
                    playGame = false;
                    break;
                case GameResult.PLAYER_BUST:
                    UnityEngine.Debug.Log("Player Busts");
                    playGame = false;
                    break;
                case GameResult.DEALER_WIN:
                    UnityEngine.Debug.Log("Dealer Wins.");
                    playGame = false;
                    break;
                case GameResult.SURRENDER:
                    UnityEngine.Debug.Log("Player Surrenders ");
                    playGame = false;
                    break;
                case GameResult.CONTINUE_PLAYING:
                    playGame = true;
                    break;
            }
            if (!playGame)
            {
                new WaitForSeconds(9000);

                Application.Quit();
            }
    }
}
