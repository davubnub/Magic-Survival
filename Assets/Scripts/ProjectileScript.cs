using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    float projectileSpeed;
    float destroyTimer;
    float damage;
    float homingStrength;
    float explosionSize;
    int piercing;
    bool homing = false;
    bool explosion = false;
    Transform closestEnemy;
    Rigidbody rb;
    PlayerScript player;
    private ParticleSystem particle;

    public GameObject explosionObj;
    public float explosionWait;
    public bool isParticle = false;         //Adding this variable in to prevent it interfering with the placeholder

    private void Start()
    {
        homing = (homingStrength != 0);
        if (gameObject.GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();

        if (gameObject.GetComponent<ParticleSystem>() != null) particle = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    public void FireProjectile(float _projectileSpeed, float _destroyTimer, float _damage, int _piercing, float _accuracy, 
        float _homing, float _explosion, float _fireRate = 0.0f, bool _isParticle = false)
    {
        damage = _damage;
        homingStrength = _homing;
        homing = (homingStrength != 0);
        explosionSize = _explosion;
        explosion = (explosionSize != 0);

        if (!isParticle)
        {
            projectileSpeed = _projectileSpeed;
            piercing = _piercing;
            destroyTimer = _destroyTimer;
        }
        else
        {
            particle.Play();
            ParticleSystem.MainModule main = particle.main;
            main.startSpeed = projectileSpeed;
            main.loop = true;

            ParticleSystem.EmissionModule emission = particle.emission;
            emission.rateOverTime = _fireRate;
        }
        //if (particle != null)
        //{
        //    //ParticleSystem.MainModule main = particle.main;

        //    //main.startSpeed = projectileSpeed;
        //}

        //if (homing)
        //{
        //    float closestDistance = Mathf.Infinity;
        //    float checkForward    = 5;
        //    Vector3 newPos        = transform.position + (transform.forward * checkForward);

        //    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        //    {
        //        float distance = Vector3.Distance(newPos, enemy.transform.position);

        //        if (distance < closestDistance)
        //        {
        //            closestEnemy = enemy.transform;
        //            closestDistance = distance;
        //        }
        //    }
        //}

        transform.forward += new Vector3(Random.Range(-_accuracy, _accuracy), 0, 0);
        if (!isParticle) StartCoroutine(DestroyWait());
    }

    private void FixedUpdate()
    {
        //transform.position += transform.forward * Time.deltaTime * projectileSpeed;
        if (rb != null) rb.velocity = transform.forward * projectileSpeed;

        //if (homing && closestEnemy != null)
        //{
        //    Vector3 direction = closestEnemy.position - transform.position;
        //    Quaternion rotation = Quaternion.LookRotation(direction);
        //    rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, homingStrength * Time.deltaTime));
        //}
    }

    public void StartExplosion(Vector3 _pos)
    {
        if (explosion)
        {
            GameObject obj = Instantiate(explosionObj, transform.position, Quaternion.identity);
            obj.transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
            Destroy(obj, explosionWait);
            Destroy(obj.GetComponent<SphereCollider>(), 0.1f);
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public int GetPiercing()
    {
        return piercing;
    }
    public void SetPiercing()
    {
        piercing--;
        damage /= 2;
    }

    private IEnumerator DestroyWait()
    {
        yield return new WaitForSeconds(destroyTimer);
        player.GetPoolingManager().DespawnObject(gameObject);
        //Destroy(gameObject);
    }
}
