using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int points;
    public int wave = 0;
    public int enemyCount = 0;
    public float spawnRadius = 20f;
    public int spawnAmount = 5;
    public GameObject player;
    public GameObject enemyPrefab;
    
    public static GameManager GM;

    public TextMeshProUGUI pointsUI;
    public TextMeshProUGUI waveUI;

    bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        GM = this;
    }

    // Update is called once per frame
    void Update()
    {
        pointsUI.SetText("Points: " + points);
        waveUI.SetText("Wave: " + wave);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnEnemies();
        }

        NextWave();
    }

    IEnumerator WaveDelay()
    {
        yield return new WaitForSeconds(2f);
        spawnAmount = wave * 2 + spawnAmount;
        wave++;
        StartCoroutine(SpawnEnemies());
        yield return new WaitForSeconds(5f);
        canSpawn = true;
    }
    void NextWave()
    {
        if (enemyCount <= 0 && canSpawn) 
        {
            canSpawn = false;
            StartCoroutine(WaveDelay());
        }
        if (enemyCount < 0)
        {
            enemyCount = 0;
        }
        
    }
    IEnumerator SpawnEnemies()
    {  
        for (int i = 0; i < spawnAmount; i++)
        {
            var spawnPoint = RandomPointOnCircleEdge(spawnRadius);
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }    
    }
    public Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        return vector2;
    }

}
