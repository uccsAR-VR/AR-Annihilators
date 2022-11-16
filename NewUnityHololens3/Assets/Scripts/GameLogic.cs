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
    public static List<string> argumentList = new List<string>();
    public static List<string> tempListPlayer = new List<string>();
    public static List<string> tempListDealer = new List<string>();
    public static List<string> tempListFinal = new List<string>();
    static GameObject ParticleSys;
    static ParticleSystem ParticleSysComp;
    public static int countDealerHitsMAX = 0;
    public static int countPlayerHitsMAX = 0;

    static void Delay()
    {
        for(int i = 0; i < 50000; i++)
        {
            for(int j=0; j< 25000; j++)
            {
                //
            }
        }
    }

    static List<string> updateCardList(List<string> playerList, List<string> dealerList)
    {
        List<string> returnedList = new List<string>();
        for (int i=0; i < playerList.Count; i++)
        {
            returnedList.Add(playerList[i]);
        }

        for(int j =0; j < dealerList.Count; j++)
        {
            returnedList.Add(dealerList[j]);
        }
        return returnedList;
    }//updateCardList

    static void SetDealerPlayerHitCounts()
    {
        int dIndex = 0;
        int dIndex2 = 0;
        for(int i=1; i < argumentList.Count; i++)
        {
            if(argumentList[i] == "d")
            {
                dIndex = i;
            }
        }//end for

        for (int i = 1; i < argumentList.Count; i++)
        {
            if (i < dIndex)
            {
                countPlayerHitsMAX += 1;
            }
        }//end for

        for (int i = 0; i < argumentList.Count; i++)
        {
            if(argumentList[i] == "d")
            {
                dIndex2 = i;
            }
        }//end for

        for (int i = 0; i < argumentList.Count; i++)
        {
            if (i > dIndex2)
            {
                countDealerHitsMAX += 1;
            }
        }//end for
    }//end SetDealerPlayerHitCounts

    static int GetPlayerHand(int numberOfHits)
    {
        if(argumentList[0]=="p")
        {
            return Int32.Parse(argumentList[numberOfHits]);
        }
        else
        {
            return 0;
        }
    }//end GetPlayerCard
    static int GetDealerHand(int numberOfHits)
    {
        for(int i = 0; i < argumentList.Count; i++)
        {
            if(argumentList[i]=="d")
            {
                return Int32.Parse(argumentList[i + numberOfHits]);
            }
        }
        return 0;
    }

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
            }
            else if (list1[i] == "d")
            {
                strTemp = "Found d: " + list1[i];
                UnityEngine.Debug.Log(strTemp);
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
    private int playerHand = 0;
    private int dealerHand = 0;
    private bool playGame = false;
    private int numOfHitsDealer = 0;
    private int numOfHitsPlayer = 0;
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

        //string[] probsDataArr = GetProbs(argumentList);//elem 0 is sum player, elem 2 is sum dealer, 3rd elem is stand prob, 4th elem is hit prob,
        //5th elem is checking for split.
        //GameObject ToHit = GameObject.Find("ToHit");
        //GameObject ToStay = GameObject.Find("ToStay");
        //TMP_Text ToHitText;
        //TMP_Text ToStayText;
        //ToHitText = ToHit.GetComponent<TMP_Text>();
        //ToStayText = ToStay.GetComponent<TMP_Text>();
        //ToHitText.text = "Hit: " + probsDataArr[2] + "%";
        //ToStayText.text = "Stay: " + probsDataArr[3] + "%";

        ParticleSys = GameObject.Find("particleSystem");
        ParticleSysComp = ParticleSys.GetComponent<ParticleSystem>();
        ParticleSysComp.enableEmission = false;

        SetDealerPlayerHitCounts();
        string str1 = "" + countPlayerHitsMAX;
        string str2 = "" + countDealerHitsMAX;
        UnityEngine.Debug.Log(str1 + "\n");
        UnityEngine.Debug.Log(str2 + "\n");


        tempListPlayer.Add("p");
        tempListDealer.Add("d");

    }//end start

    // Update is called once per frame
    void Update()
    {
        GameObject ToHit = GameObject.Find("ToHit");
        GameObject ToStay = GameObject.Find("ToStay");
        TMP_Text ToHitText;
        TMP_Text ToStayText;
        string[] probsDataArr;

        numOfHitsPlayer += 1;
        numOfHitsDealer += 1;
        if (numOfHitsPlayer <= countPlayerHitsMAX)
        {
            playerHand = GetPlayerHand(numOfHitsPlayer) + playerHand;//test game results
            string str1 = GetPlayerHand(numOfHitsPlayer) + "";
            tempListPlayer.Add(str1);
            string temp = "str1: " + str1;
            string temp2 = "playerHand: " + playerHand;
            UnityEngine.Debug.Log(temp);
            UnityEngine.Debug.Log(temp2);
        }

        if (numOfHitsDealer <= countDealerHitsMAX)
        {
            dealerHand = GetDealerHand(numOfHitsDealer) + dealerHand;//test game results
            string str2 = GetDealerHand(numOfHitsPlayer) + "";
            tempListDealer.Add(str2);
            string temp = "str2: " + str2;
            string temp2 = "dealerHand: " + dealerHand;
            UnityEngine.Debug.Log(temp);
            UnityEngine.Debug.Log(temp2);
        }
        
        tempListFinal = updateCardList(tempListPlayer, tempListDealer);
        probsDataArr = GetProbs(tempListFinal);//elem 0 is sum player, elem 2 is sum dealer, 3rd elem is stand prob, 4th elem is hit prob,
        //5th elem is checking for split.
        ToHitText = ToHit.GetComponent<TMP_Text>();
        ToStayText = ToStay.GetComponent<TMP_Text>();
        ToHitText.text = "Hit: " + probsDataArr[2] + "%";
        ToStayText.text = "Stay: " + probsDataArr[3] + "%";
        if (result != GameResult.PLAYER_WIN)
        {
            Delay();
        }

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
                //ParticleSysComp.enableEmission = true;
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
            new WaitForSeconds(30);
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
                
                Application.Quit();
            }
    }//end update
}
