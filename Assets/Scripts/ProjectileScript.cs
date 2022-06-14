using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float projectileSpeed;
    public int damage;
    public float destroyTimer;

    public void FireProjectile()
    {
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
