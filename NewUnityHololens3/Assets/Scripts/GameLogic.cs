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
    //static ParticleSystem = GetComponent<ParticleSystem>().enableEmission = false;
    //GameObject partSystem = GameObject.Find("particleSystem");
    /*IEnumerator playWinMusic()
    {
        winningNoise.Play();
        yield return new WaitForSeconds(winningNoise.clip.length);
        winningNoise.Stop();
    }

    IEnumerator playLossMusic()
    {
        losingNoise.Play();
        yield return new WaitForSeconds(losingNoise.clip.length);
        losingNoise.Stop();

    }*/

    static string[] GetProbs(List<string> list1)
    {
        //string str1 = Application.dataPath;
        //CreateText();
        string str1 = Application.persistentDataPath;
        string strTemp;
        //bool addPlayerCards = false;
        //bool addDealerCards = false;
        UnityEngine.Debug.Log(str1);
        Process p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        string CMD_execute_with_args = "python ./blackjack.py";

        for(int i=0; i < list1.Count; i++)
        {
            //if(addPlayerCards)
            //{
                strTemp = " " + list1[i];
                CMD_execute_with_args += strTemp;
            //}
            //else if(addDealerCards)
            //{
                //strTemp = " " + list1[i];
                //CMD_execute_with_args += strTemp;
            //}


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

        //addDealerCards = false;



        //start.FileName = "C:\\Users\\change\\AppData\\Local\\Programs\\Python\\Python310\\python.exe";
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
        /*string str1 = Application.persistentDataPath;
        UnityEngine.Debug.Log(str1);
        Process p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
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
                sw.WriteLine("python ./blackjack.py");
            }
        }

        p.WaitForExit();

        UnityEngine.Debug.Log("cmd ran");
        string[] lines = File.ReadAllLines(".\\Assets\\Scripts\\probs.txt");
        foreach (string line in lines)
        {
            // Use a tab to indent each line of the file.
            UnityEngine.Debug.Log("\t" + line);
        }*/
        playGame = true;
        //playerHand = 0;
        //dealerHand = 0;
        //playerHand = 17;//test game results
        //dealerHand = 21;
        //testfrom one of jacks ex. player gets 5, 5, and 3. Dealer gets 9, 7.
        //add to dynamic list each card played for player and dealer, and input as an argument for getprobs()
        argumentList.Add("p");
        argumentList.Add("5");
        argumentList.Add("5");
        argumentList.Add("3");

        argumentList.Add("d");
        argumentList.Add("9");
        argumentList.Add("7");
        argumentList.Add("3");
        //playerHand = 13;//test game results
        //dealerHand = 16;
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

        //GameObject partSystem = GameObject.Find("particleSystem");
        //ParticleSystem particle_sys = partSystem.GetComponent<ParticleSystem>();


        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //GetComponent<ParticleSystem>().enableEmission = false;
        //}

    }//end start

    // Update is called once per frame
    void Update()
    {
        //Get player hands
        //Play sound for casino
        //Display probability stats 

        //playerHand = 17;//test game results
        //dealerHand = 21;

        //logic pseudocode for next time 11/4/2022:
        //get card values from object detection, 
        //call getprobs, which will write a text file for python script to read the card values,
        //and then at end of script, python creates the prob.txt which has the probabilities, and that is read again in c# for UI.

        /*GameObject ToHit = GameObject.Find("ToHit");
        GameObject ToStay = GameObject.Find("ToStay");
        TMP_Text ToHitText;
        TMP_Text ToStayText;
        ToHitText = ToHit.GetComponent<TMP_Text>();
        ToStayText = ToStay.GetComponent<TMP_Text>();
        ToHitText.text = "hi1";
        ToStayText.text = "hi2";*/
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
