using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    Transform player;
    public float despawnDistance;
    public float speed;
    public float maxSpeed;
    float magnetRange;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Init();
    }

    public void Init()
    {
        magnetRange = player.GetComponent<PlayerScript>().GetUpgradableStats().magnetStrength;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > despawnDistance)
        {
            player.GetComponent<PlayerScript>().GetPoolingManager().DespawnObject(gameObject);
            //Destroy(gameObject);
        }

        if (distance < magnetRange)
        {
            Vector3 direction = player.position - transform.position;

            transform.position += direction * Time.deltaTime * Mathf.Clamp(Mathf.Pow(speed, magnetRange - distance) / 10, 0, maxSpeed);
        }
    }
}
