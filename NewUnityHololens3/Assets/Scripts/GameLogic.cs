using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameLogic : MonoBehaviour
{
    static List<string> argumentList = new List<string>();

    static string[] GetProbs(List<string> list1)
    {
        string str1 = Application.persistentDataPath;
        string strTemp;
        UnityEngine.Debug.Log(str1);
        Process p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        string CMD_execute_with_args = "python ./blackjack.py";

        for(int i=0; i < list1.Count; i++)
        {
                strTemp = " " + list1[i];
                CMD_execute_with_args += strTemp;
           

            if(list1[i] == "p")
            {
                strTemp = "Found p: " + list1[i];
                UnityEngine.Debug.Log(strTemp);
                //addPlayerCards = true;
            }
            else if (list1[i] == "d")
            {
                strTemp = "Found d: " + list1[i];
                UnityEngine.Debug.Log(strTemp);
                //addDealerCards = true;
                //addPlayerCards = false;
            }
        }//end for loop



        startInfo.FileName = "C:\\WINDOWS\\system32\\cmd.exe";
        startInfo.Arguments = string.Format("{0}", @"python C:\\Users\\change\\source\\unity\\NewUnityHololens2\\Assets\\Scripts\\blackjack.py");
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardInput = true;
        p.StartInfo = startInfo;
        p.Start();

        using (StreamWriter sw = p.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine("cd C:\\Users\\change\\source\\unity\\NewUnityHololens3\\Assets\\Scripts");
                //sw.WriteLine("python ./blackjack.py");
                sw.WriteLine(CMD_execute_with_args);
            }
        }

        p.WaitForExit();

        UnityEngine.Debug.Log("cmd ran");
        string[] lines = File.ReadAllLines(".\\Assets\\Scripts\\probs.txt");

        string[] probsStrDataArr = new string[5];
        int count = 0;
        foreach (string line in lines)
        {
            // Use a tab to indent each line of the file.
            UnityEngine.Debug.Log("\t" + line);
            probsStrDataArr[count] = line;
            count += 1;
        }
        return probsStrDataArr;
    }//end GetProbs



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


    void Start()
    {
        
        playGame = true;
        argumentList.Add("p");
        argumentList.Add("5");
        argumentList.Add("5");
        argumentList.Add("3");

        argumentList.Add("d");
        argumentList.Add("9");
        argumentList.Add("7");
        argumentList.Add("3");
        string[] probsDataArr = GetProbs(argumentList);//elem 0 is sum player, elem 2 is sum dealer, 3rd elem is stand prob, 4th elem is hit prob,
        //5th elem is checking for split.
        GameObject ToHit = GameObject.Find("ToHit");
        GameObject ToStay = GameObject.Find("ToStay");
        TMP_Text ToHitText;
        TMP_Text ToStayText;
        ToHitText = ToHit.GetComponent<TMP_Text>();
        ToStayText = ToStay.GetComponent<TMP_Text>();
        ToHitText.text = "Hit: " + probsDataArr[2] + "%";
        ToStayText.text = "Stay: " + probsDataArr[3] + "%";

    }//end start

    // Update is called once per frame
    void Update()
    {
        
        playerHand = 13;//test game results
        dealerHand = 19;

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
                //StartCoroutine(playWinMusic());
                winningNoise.Play();
                result = GameResult.PLAYER_WIN;
            }
            else if (dealerHand > 21 && dealerHand != 22)
            {
                result = GameResult.PLAYER_WIN;
            }
            else if (dealerHand > playerHand && dealerHand != 22)
            {
                //StartCoroutine(playLossMusic());
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
                    //result = GameResult.CONTINUE_PLAYING;
                    break;
                case GameResult.PLAYER_BUST:
                    UnityEngine.Debug.Log("Player Busts");
                    playGame = false;
                    //result = GameResult.CONTINUE_PLAYING;
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
    }//end update
}
