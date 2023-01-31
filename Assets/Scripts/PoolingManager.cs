using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [System.Serializable]
    public struct PoolingSystems
    {
        public PoolingEnum name;
        public GameObject gameObjectToSpawn;
        public int IntialToSpawn;
    }

    public PoolingSystems[] poolingSystems;
    List<GameObject> parentPools = new List<GameObject>();

    public enum PoolingEnum
    {
        Coins,
        XP,
        Bullet,
        Enemy1,
        Enemy2,
        Enemy3,
        Enemy4,
        Obstacles,
    }

    public void SpawnIntialObjects()
    {
        for (int i = 0; i < poolingSystems.Length; i++)
        {
            GameObject parent = new GameObject(poolingSystems[i].name.ToString() + "PoolSystem");
            parentPools.Add(parent);
            parentPools[i].transform.parent = transform;

            for (int j = 0; j < poolingSystems[i].IntialToSpawn; j++)
            {
                GameObject obj = Instantiate(poolingSystems[i].gameObjectToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
                obj.transform.parent = parent.transform;
                obj.SetActive(false);
            }
        }

        //Setting the projectile object in the player script
        //PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        //if (player.projectile != null) return;

        //player.projectile = FindFreeObject(parentPools[(int)PoolingEnum.Bullet].transform, false);
    }

    public bool CheckIfPoolFree(PoolingEnum _poolingEnum)
    {
        return (FindFreeObject(parentPools[(int)_poolingEnum].transform) != null);
    }

    public GameObject SpawnObject(PoolingEnum _poolingEnum, Vector3 _position, Quaternion _rotation)
    {
        GameObject spawnObj = FindFreeObject(parentPools[(int)_poolingEnum].transform);

        if (spawnObj != null)
        {
            spawnObj.SetActive(true);
            spawnObj.transform.position = _position;
            spawnObj.transform.rotation = _rotation;
        }

        return spawnObj;
    }

    public void DespawnObject(GameObject _object)
    {
        //Check if there's a particle system to disable
        ParticleSystem particle = _object.GetComponent<ParticleSystem>();
        if (particle != null)
        {
            ParticleSystem.MainModule main = particle.main;
            main.loop = false;

            particle.Stop();
        }

        _object.SetActive(false);
        _object.transform.position = new Vector3(0, 0, 0);
    }

    GameObject FindFreeObject(Transform _parent)
    {
        foreach (Transform child in _parent)
        {
            //Check if theres a particle system
            ParticleSystem childParticles = child.gameObject.GetComponent<ParticleSystem>();
            if (!child.gameObject.activeSelf && childParticles == null)
            {
                return child.gameObject;
            }
            else if (childParticles != null && childParticles.main.loop == false)
            {

                return child.gameObject;
            }
        }

        Debug.LogWarning($"Couldn't find a free object in {_parent.name} pool (try increasing the size)");
        return null;
    }

    //Check how many items in the pool is currently active
    public int GetPoolAmount(PoolingEnum _poolEnum)
    {
        int activeAmount = 0;

        foreach (Transform child in parentPools[(int)_poolEnum].transform)
        {
            if (child.gameObject.activeSelf)
            {
                activeAmount++;
            }
        }

        return activeAmount;
    }

    /*PoolingSystems FindPoolingSystem(PoolingEnum _poolingEnum)
    {
        for(int i = 0; i < poolingSystems.Length; i++)
        {
            if (poolingSystems[i].name == _poolingEnum)
                return poolingSystems[i];
        }

        Debug.LogError("Couldn't find enum");
        return poolingSystems[0];
    }*/
}