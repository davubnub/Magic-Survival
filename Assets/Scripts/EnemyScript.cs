using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public int damage;
    public float speed;
    public float range;
    public float attackWait;

    float t;

    GameObject player;
    public GameObject xpPellet;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        t = attackWait;
    }

    private void FixedUpdate()
    {
        //rotate to look at player
        transform.LookAt(player.transform);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        //move towards player
        transform.position += transform.forward * Time.deltaTime * speed;

        t -= Time.deltaTime;

        if (t <= 0)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward, Color.green, 1);

            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    player.GetComponent<PlayerScript>().UpdateHealth(damage);
                }
            }

            t = attackWait + Random.Range(0.00f, 0.15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            DamageEnemy(other.GetComponent<ProjectileScript>().GetDamage());
            Destroy(other.gameObject);
        }
    }

    public void DamageEnemy(int _damage)
    {
        health -= _damage;

        if(health <= 0)
        {
            EnemyDied();
        }
    }

    void EnemyDied()
    {
        Instantiate(xpPellet, transform.position, Quaternion.Euler(0, 45, 0));
        Destroy(gameObject);
    }
}
