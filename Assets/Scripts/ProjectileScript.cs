using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    float projectileSpeed;
    float destroyTimer;
    float damage;
    int piercing;

    public void FireProjectile(float _projectileSpeed, float _destroyTimer, float _damage, int _piercing, float _accuracy)
    {
        projectileSpeed = _projectileSpeed;
        destroyTimer = _destroyTimer;
        damage = _damage;
        piercing = _piercing;
        transform.forward += new Vector3(Random.Range(-_accuracy, _accuracy), 0, 0);
        StartCoroutine(DestroyWait());
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
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
        Destroy(gameObject);
    }
}
