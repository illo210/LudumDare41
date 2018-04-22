using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    TextMesh tm;

    // Use this for initialization
    void Start()
    {
        tm = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
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