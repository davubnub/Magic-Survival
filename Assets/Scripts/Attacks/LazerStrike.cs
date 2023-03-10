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
    [SerializeField] private GameObject collider;
    public bool ObjectInPool = true;
    private PoolingManager pool;

    // Start is called before the first frame update
    void Start()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (pool == null) pool = FindObjectOfType<PoolingManager>();

        if (ps.time >= minAttackTime && ps.time < maxAttackTime)
        {
            collider.SetActive(true);
        }
        else if (collider.activeSelf == true && ps.time >= maxAttackTime)
        {
            collider.SetActive(false);
        }

        if (!ps.isPlaying)
        {
            if (ObjectInPool)
            {
                pool.DespawnObject(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        ps.Play();
    }
}
