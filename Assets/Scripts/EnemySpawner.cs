using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public float radius;
    [Tooltip("Amount to spawn in a group")]
    public int spawnGroupAmount;
    [Tooltip("Wait this amount of spawns before increasing amount of enemies that spawn in a group")]
    public int groupSpawnIncrModulus;
    [Tooltip("Wait this amount of spawns before increases the addition of adding difficult enemies to the enemiesToSpawn list")]
    public int addHarderEnemyAmountModulus;
    [Tooltip("Wait this amount of spawns before adding enemies to list")]
    public int harderEnemyAddModulus;
    [Tooltip("Amount of items in enemiesToSpawn list that will be replaced")]
    public int harderEnemyReplaceAmount;
    [Tooltip("Length of enemiesToSpawn list")]
    public int enemiesToSpawnLength;

    int spawnTracker = 0;
    int enemyToAdd = 1;

    public GameObject[] enemies;
    public PoolingManager poolingManager;

    List<GameObject> enemiesToSpawn = new List<GameObject>();

    GameObject player;

    public void StartPressed()
    {
        StartCoroutine(WaitToSpawn());
        player = GameObject.FindGameObjectWithTag("Player");

        for(int i = 0; i < enemiesToSpawnLength; i++)
        {
            //add first enemy type to the list
            enemiesToSpawn.Add(enemies[0]);
        }
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnRate);
        SpawnEnemy(spawnGroupAmount);

        StartCoroutine(WaitToSpawn());
    }

    void SpawnEnemy(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float angle = Random.Range(0, 2f * Mathf.PI);
            Vector3 pos = player.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            pos = new Vector3(pos.x, 0.5f, pos.z);

            Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)], pos, Quaternion.identity);
            //poolingManager.SpawnObject(PoolingManager.PoolingEnum.Enemy, pos, Quaternion.identity);
        }

        spawnTracker++;

        if(spawnTracker % groupSpawnIncrModulus == 0)
        {
            spawnGroupAmount++;
        }
        if (spawnTracker % harderEnemyAddModulus == 0 && enemyToAdd < enemies.Length)
        {
            for (int i = 0; i < harderEnemyReplaceAmount; i++)
            {
                //add first enemy type to the list
                enemiesToSpawn.RemoveAt(0);
                enemiesToSpawn.Add(enemies[enemyToAdd]);
            }
        }
        if (spawnTracker % addHarderEnemyAmountModulus == 0)
        {
            enemyToAdd++;
        }
    }
}
