using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money;
    private 
    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void get_money(int amount)
    {
        money += amount;

        //Display
    }

    public void give_money(int amount)
    {
        if (money - amount >= 0)
        {
            money -= amount;
            //Display
        }
    }
}