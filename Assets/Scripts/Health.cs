using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Text tm;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int current()
    {
        return tm.text.Length;
    }

    public void decrease(int nb)
    {
        if (current() > nb)
        {
            tm.text = tm.text.Remove(tm.text.Length - nb);
            //add animation
        }
        else
            Destroy(transform.parent.gameObject);
    }
}