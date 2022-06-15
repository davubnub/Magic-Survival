using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    float projectileSpeed;
    float destroyTimer;
    int damage;

    public void FireProjectile(float _projectileSpeed, float _destroyTimer, int _damage)
    {
        projectileSpeed = _projectileSpeed;
        destroyTimer = _destroyTimer;
        damage = _damage;
        StartCoroutine(DestroyWait());
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

    public int GetDamage()
    {
        return damage;
    }

    private IEnumerator DestroyWait()
    {
        yield return new WaitForSeconds(destroyTimer);
        Destroy(gameObject);
    }
}
