using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectGenerator : MonoBehaviour
{
    public GameObject ObjecttoSpawn;
    public int SpawnSize = 100;
    public float SpawnChance = 1.0f;

    [Header("Debugging Purpose Only")]
    public int Seed = 0;

    private List<Vector2> ObjectLocations = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Seed);

        ObjectLocations = GenerateObjectLocations();

        for (int i = 0; i < ObjectLocations.Count; i++)
        {
            GameObject Object = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Object.transform.position = new Vector3(ObjectLocations[i].x, this.transform.position.y, ObjectLocations[i].y);
        }
    }

    List<Vector2> GenerateObjectLocations()
    {
        List<Vector2> Result1 = new List<Vector2>();
        List<Vector2> Result2 = new List<Vector2>();

        // In a cube of size x, we generate every integer coordinate
        // and we only accept numbers if they are less than a random number

        List<Vector2> PossibleSpawnLocations = new List<Vector2>();
        List<Vector2> PossibleSpawnLocations2 = new List<Vector2>();
        List<Vector2> PossibleSpawnLocations3 = new List<Vector2>();

        for (int x = 1; x < SpawnSize; x++)
        {
            for (int y = 1; y < SpawnSize; y++)
            {
                if (Random.Range(1, (SpawnSize*2) + 1) % (x + y) < Random.Range(0, SpawnSize))
                {
                    PossibleSpawnLocations.Add(new Vector2(x, y));
                }
            }
        }

        for (int x = 1; x < SpawnSize; x++)
        {
            for (int y = 1; y < SpawnSize; y++)
            {
                if (Random.Range(1, (SpawnSize * 2) + 1) % (x + y) > Random.Range(0, SpawnSize))
                {
                    PossibleSpawnLocations2.Add(new Vector2(x, y));
                }
            }
        }

        int CountSize = 0;

        if (PossibleSpawnLocations.Count > PossibleSpawnLocations2.Count)
        {
            CountSize = PossibleSpawnLocations2.Count;
        }
        else if (PossibleSpawnLocations2.Count > PossibleSpawnLocations.Count)
        {
            CountSize = PossibleSpawnLocations.Count;
        }
        else
        {
            CountSize = PossibleSpawnLocations.Count;
        }

        for (int i = 0; i < CountSize; i++)
        {
            if (Mathf.PerlinNoise(PossibleSpawnLocations[i].x, PossibleSpawnLocations[i].y) < Random.Range(0, SpawnSize + 1))
            {
                Result1.Add(PossibleSpawnLocations[i]);
            }

            if (Mathf.PerlinNoise(PossibleSpawnLocations2[i].x, PossibleSpawnLocations2[i].y) < Random.Range(0, SpawnSize + 1))
            {
                Result1.Add(PossibleSpawnLocations2[i]);
            }
        }

        //Run through List of Possible Spawn Locations and if

        //The value of the Perlin Noise is less than the chance value
        //that will be our list of locations we can use

        for (int i = 0; i < Result1.Count; i++)
        {
            if (Random.Range(0, Result1[i].x + Result1[i].y + 1) < SpawnChance)
            {
                Result2.Add(Result1[i]);
            }
        }

        return Result2;
    }
}
