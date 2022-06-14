using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public int spawnGroupAmount;
    public float radius;

    public GameObject[] enemies;

    GameObject player;

    private void Start()
    {
        StartCoroutine(WaitToSpawn());
        player = GameObject.FindGameObjectWithTag("Player");
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
            Instantiate(enemies[0], pos, Quaternion.identity);
        }
    }
}
