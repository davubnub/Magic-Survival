using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The lazer strike script plays a particle system that shoots a beam to the ground. Once the playback reaches
//a certain time, it will damage enemies surrounding the beam
public class LazerStrike : MonoBehaviour
{
    [Header("Particle stuff")]
    [SerializeField] private ParticleSystem ps;
    public float minAttackTime = 0.0f, maxAttackTime = 3.0f;

    [Header("Other stuff")]
    [SerializeField] private Collider collider;
    private PoolingManager pool;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pool == null) pool = FindObjectOfType<PoolingManager>();

        if (ps.time >= minAttackTime && ps.time < maxAttackTime)
        {
            collider.enabled = true;
        }
        else if (collider.enabled == true && ps.time >= maxAttackTime)
        {
            collider.enabled = false;
        }

        if (!ps.isPlaying)
        {
            pool.DespawnObject(gameObject);
        }
    }

    private void OnEnable()
    {
        ps.Play();
    }
}
