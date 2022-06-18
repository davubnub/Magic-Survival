using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float health;
    public float damage;
    public float speed;
    public float range;
    public float attackWait;
    public int scoreIncrease;

    float t;

    GameObject player;
    PlayerScript playerScript;
    Rigidbody rb;
    public GameObject xpPellet;

    private void Start()
    {
        player          = GameObject.FindGameObjectWithTag("Player");
        playerScript    = player.GetComponent<PlayerScript>();
        rb              = GetComponent<Rigidbody>();
        t               = attackWait;
    }

    private void FixedUpdate()
    {
        //rotate to look at player
        transform.LookAt(player.transform);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        //move towards player
        //transform.position += transform.forward * Time.deltaTime * speed;

        rb.AddForce(transform.forward * Time.deltaTime * speed, ForceMode.Force);

        t -= Time.deltaTime;

        if (t <= 0)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward, Color.green, 1);

            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    playerScript.UpdateHealth(damage);
                }
            }

            t = attackWait + Random.Range(0.00f, 0.15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            HitByBullet(other.gameObject);
        }
    }

    void HitByBullet(GameObject _bullet)
    {
        ProjectileScript projectile = _bullet.GetComponent<ProjectileScript>();
        DamageEnemy(projectile.GetDamage());
        Knockback();

        if (projectile.GetPiercing() <= 0)
        {
            Destroy(_bullet);
        }
        else
        {
            projectile.SetPiercing();
        }
    }

    void Knockback()
    {
        //transform.position -= transform.forward * playerScript.GetUpgradableStats().projectileKnockback;
        rb.AddForce(-transform.forward * playerScript.GetUpgradableStats().projectileKnockback, ForceMode.Force);
    }

    public void DamageEnemy(float _damage)
    {
        health -= _damage;

        if(health <= 0)
        {
            EnemyDied();
        }
    }

    void EnemyDied()
    {
        playerScript.IncreaseScore(scoreIncrease);
        Instantiate(xpPellet, transform.position, Quaternion.Euler(0, 45, 0));
        Destroy(gameObject);
    }
}
