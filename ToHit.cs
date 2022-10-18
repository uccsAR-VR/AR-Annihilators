using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ToHit : MonoBehaviour
{
    private int health = 5;
    public TMP_Text healthText;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Hit " + health;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            health--;
        }
    }//end update
}
