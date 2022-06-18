using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public float radius;
    public int spawnGroupAmount;
    public int difficultIncrTime;

    public GameObject[] enemies;

    GameObject player;

    private void Start()
    {
        StartCoroutine(WaitToSpawn());
        StartCoroutine(DifficultIncreaseWait());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator DifficultIncreaseWait()
    {
        yield return new WaitForSeconds(difficultIncrTime);
        spawnGroupAmount++;

        StartCoroutine(DifficultIncreaseWait());
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

            int pick = Random.Range(0, 100);
            if(pick <= 95)
            {
                Instantiate(enemies[0], pos, Quaternion.identity);
            }
            else
            {
                Instantiate(enemies[Random.Range(1, 3)], pos, Quaternion.identity);
            }
        }
    }
}
