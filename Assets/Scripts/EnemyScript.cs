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
    PoolingManager poolingManager;
    Rigidbody rb;
    public GameObject xpPellet;
    public GameObject criticalText;
    public GameObject impactVFX;

    private void Start()
    {
        player         = GameObject.FindGameObjectWithTag("Player");
        poolingManager = GameObject.Find("PoolingManager").GetComponent<PoolingManager>();
        playerScript   = player.GetComponent<PlayerScript>();
        rb             = GetComponent<Rigidbody>();
        t              = attackWait;
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
            DamageEnemy(playerScript.GetUpgradableStats().bulletDamage, true);
        }
        if(other.CompareTag("Explosion"))
        {
            DamageEnemy(playerScript.GetUpgradableStats().explosionDamage, true);
        }
    }

    void HitByBullet(GameObject _bullet)
    {
        ProjectileScript projectile = _bullet.GetComponent<ProjectileScript>();
        projectile.StartExplosion(transform.position);
        DamageEnemy(projectile.GetDamage(), false);
        Knockback();

        Vector3 direction = transform.position - _bullet.transform.position;
        Destroy(Instantiate(impactVFX, _bullet.transform.position, Quaternion.LookRotation(-direction)), impactVFX.GetComponent<ParticleSystem>().main.duration);

        if (projectile.GetPiercing() <= 0)
        {
            poolingManager.DespawnObject(_bullet);
            //Destroy(_bullet);
        }
        else
        {
            projectile.SetPiercing();
        }
    }

    void Knockback()
    {
        rb.AddForce(-transform.forward * playerScript.GetUpgradableStats().bulletKnockback, ForceMode.Force);
    }

    public void SetPaused(bool _pause)
    {
        paused = _pause;
    }

    public void DamageEnemy(float _damage, bool _set)
    {
        int rand = Random.Range(0, 100);

        if(rand < playerScript.GetUpgradableStats().criticalChance)
        {
            _damage *= 2;
            Destroy(Instantiate(criticalText, transform.position, Quaternion.identity), critcalTimer);
        }

        if(!_set && playerScript.GetUpgradableStats().damageDistance > 0)
        {
            //float newDamage = _damage * ((Vector3.Distance(player.transform.position, transform.position) * playerScript.GetUpgradableStats().damageDistance));
            _damage += (Vector3.Distance(player.transform.position, transform.position) / 10) * playerScript.GetUpgradableStats().damageDistance;
            print("_damage: " + _damage);
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
        //GameObject xpObj = Instantiate(xpPellet, transform.position, Quaternion.Euler(0, 45, 0));
        GameObject xpObj = poolingManager.SpawnObject(PoolingManager.PoolingEnum.XP, transform.position, Quaternion.Euler(0, 45, 0));
        xpObj.GetComponent<XPScript>().Init();
        xpObj.GetComponent<XPScript>().SetXPGain(xp);
        Destroy(gameObject);
    }
}
