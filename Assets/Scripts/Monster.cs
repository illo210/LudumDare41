using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GameObject vie = GameObject.Find("Base");
        if (vie)
            GetComponent<NavMeshAgent>().destination = vie.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}