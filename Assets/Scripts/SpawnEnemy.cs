using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints;

    public GameObject testEnemyPrefab;

    // Use this for initialization
    void Start()
    {
        GameObject enemy = Instantiate(testEnemyPrefab);
        enemy.GetComponent<BaseEnemy>().waypoints = waypoints;
        enemy.transform.position = waypoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}