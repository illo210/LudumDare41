using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    public Text goldLabel;
    public Text waveLabel;
    public bool gameOver = false;
    
    private int gold;

    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = gold.ToString();
        }
    }

    private int wave;

    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;
            waveLabel.text = "Wave: " + (wave + 1);
        }
    }

    // Use this for initialization
    void Start()
    {
        Gold = 20;
        Wave = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
}