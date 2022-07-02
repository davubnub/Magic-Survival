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
    public int xp;

    bool paused = false;

    const float critcalTimer = 1.5f;

    int layerMask = 1 << 8;

    float t;

    GameObject player;
    PlayerScript playerScript;
    Rigidbody rb;
    public GameObject xpPellet;
    public GameObject criticalText;

    private void Start()
    {
        player       = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        rb           = GetComponent<Rigidbody>();
        t            = attackWait;
    }

    private void FixedUpdate()
    {
        if (!paused)
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

                if (Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        playerScript.UpdateHealth(damage);
                    }
                }

                t = attackWait + Random.Range(0.00f, 0.15f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            HitByBullet(other.gameObject);
        }
        if(other.CompareTag("Saw"))
        {
            DamageEnemy(playerScript.GetUpgradableStats().bulletDamage);
        }
        if(other.CompareTag("Explosion"))
        {
            HitByExplosion();
        }
    }

    void HitByBullet(GameObject _bullet)
    {
        ProjectileScript projectile = _bullet.GetComponent<ProjectileScript>();
        projectile.StartExplosion(transform.position);
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

    void HitByExplosion()
    {
        DamageEnemy(playerScript.GetUpgradableStats().explosionDamage);
    }

    void Knockback()
    {
        rb.AddForce(-transform.forward * playerScript.GetUpgradableStats().bulletKnockback, ForceMode.Force);
    }

    public void SetPaused(bool _pause)
    {
        paused = _pause;
    }

    public void DamageEnemy(float _damage)
    {
        int rand = Random.Range(0, 100);

        if(rand < playerScript.GetUpgradableStats().criticalChance)
        {
            _damage *= 2;
            Destroy(Instantiate(criticalText, transform.position, Quaternion.identity), critcalTimer);
        }

        health -= _damage;

        if(health <= 0)
        {
            EnemyDied();
        }
    }

    void EnemyDied()
    {
        playerScript.IncreaseScore(scoreIncrease);
        GameObject xpObj = Instantiate(xpPellet, transform.position, Quaternion.Euler(0, 45, 0));
        xpObj.GetComponent<XPScript>().SetXPGain(xp);
        Destroy(gameObject);
    }
}
