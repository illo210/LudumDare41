using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int number = 0;
    public float spawnInterval = 2;
}

[System.Serializable]
public class Pattern
{
    public Enemy[] Enemies;
}

[System.Serializable]
public class Wave
{
    public Pattern pattern;
    public int maxEnemies = 20;
}

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints;
    public Wave[] waves;
    public int timeBetweenWaves = 5;
    private float lastSpawnTime;
    protected GameManagerBehavior gameManager;
    private int enemiesSpawned = 0;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length)
        {
            // 2
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].pattern.Enemies[0].spawnInterval;
            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) ||
                 timeInterval > spawnInterval) &&
                enemiesSpawned < waves[currentWave].maxEnemies)
            {
                // 3  
                lastSpawnTime = Time.time;
                int len_patt = waves[currentWave].pattern.Enemies.Length;
                GameObject newEnemy = (GameObject)
                    Instantiate(waves[currentWave].pattern.Enemies[0].enemyPrefab);
                newEnemy.GetComponent<BaseEnemy>().waypoints = waypoints;
                newEnemy.transform.position = waypoints[0].transform.position;
                enemiesSpawned++;
            }

            // 4 
            if (enemiesSpawned == waves[currentWave].maxEnemies &&
                GameObject.FindGameObjectWithTag("EntityEnemy") == null)
            {
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }

            // 5 
        }
        else
        {
            // gameManager.gameOver = true;
            //GameObject gameOverText = GameObject.FindGameObjectWithTag ("GameWon");
            //gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }
}