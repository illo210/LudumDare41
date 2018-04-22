using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int money;

    private UnityEngine.UI.Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        text.text = money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void get_money(int amount)
    {
        money += amount;
        text.text = money.ToString();
    }

    public void give_money(int amount)
    {
        if (money - amount >= 0)
        {
            money -= amount;
            text.text = money.ToString();
        }
    }
}