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
    public List<Enemy> Enemies;
}

[System.Serializable]
public class Wave
{
    public Pattern pattern;
}

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints;
    public Wave[] waves;
    public int timeBetweenWaves = 5;
    private float lastSpawnTime;
    protected GameManagerBehavior gameManager;
    private int enemiesSpawned, enemiesS = 0;
    private int currentEnemy = 0;


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

            // 4 
            if (currentEnemy == waves[currentWave].pattern.Enemies.Count &&
                GameObject.FindGameObjectWithTag("EntityEnemy") == null)
            {
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
                currentEnemy = 0;
            }
            else if (currentEnemy < waves[currentWave].pattern.Enemies.Count)
            {
                // 2
                float timeInterval = Time.time - lastSpawnTime;
                float spawnInterval = waves[currentWave].pattern.Enemies[currentEnemy].spawnInterval;
                if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) ||
                     timeInterval > spawnInterval) &&
                    enemiesS < waves[currentWave].pattern.Enemies[currentEnemy].number)
                {
                    // 3  
                    lastSpawnTime = Time.time;

                    GameObject newEnemy = (GameObject)
                        Instantiate(waves[currentWave].pattern.Enemies[currentEnemy].enemyPrefab);
                    newEnemy.GetComponent<BaseEnemy>().waypoints = waypoints;
                    newEnemy.transform.position = waypoints[0].transform.position;
                    enemiesSpawned++;
                    enemiesS++;
                    Debug.Log(enemiesS);
                    if (enemiesS == waves[currentWave].pattern.Enemies[currentEnemy].number)
                    {
                        enemiesS = 0;
                        currentEnemy = currentEnemy + 1;
                        Debug.Log("Current Enemy :" + currentEnemy);
                    }
                }
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