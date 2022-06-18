using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPScript : MonoBehaviour
{
    Transform player;
    public float despawnDistance;
    public float speed;
    float magnetRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        magnetRange = player.GetComponent<PlayerScript>().GetUpgradableStats().magnetStrength;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > despawnDistance)
        {
            Destroy(gameObject);
        }

        if(distance < magnetRange)
        {
            Vector3 direction = player.position - transform.position;

            transform.position += direction * Time.deltaTime * (Mathf.Pow(speed, magnetRange - distance) / 10);
        }
    }
}
