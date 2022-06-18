using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public float spawnRate;
    public float radius;
    public int maxSpawn;

    public GameObject coinObj;

    GameObject player;

    private void Start()
    {
        StartCoroutine(WaitToSpawn());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnRate);
        if (Resources.FindObjectsOfTypeAll(typeof(CoinScript)).Length <= maxSpawn)
        {
            SpawnCoin();
        }

        StartCoroutine(WaitToSpawn());
    }

    void SpawnCoin()
    {
        float angle = 0;
        Vector3 pos;
        bool foundSpot = false;

        do
        {
            angle = Random.Range(0, 2f * Mathf.PI);
            pos = player.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            RaycastHit hit;

            if (Physics.Raycast(pos, -transform.up, out hit, 2))
            {
                if(hit.transform.CompareTag("Ground"))
                {
                    foundSpot = true;
                }
            }
        } while (!foundSpot);

        Instantiate(coinObj, pos, Quaternion.identity);
    }
}
