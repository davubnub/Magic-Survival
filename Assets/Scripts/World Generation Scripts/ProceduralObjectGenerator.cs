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
    private List<GameObject> Objects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Seed);

        SpawnObjects();
    }

    void SpawnObjects()
    {
        ObjectLocations = GenerateObjectLocations();

        for (int i = 0; i < ObjectLocations.Count; i++)
        {
            GameObject Object = GameObject.Instantiate(ObjecttoSpawn, new Vector3(ObjectLocations[i].x, this.transform.position.y, ObjectLocations[i].y), Quaternion.identity);

            Objects.Add(Object);
        }
    }

    Vector2 CalculateSpawnArea(string _identifier)
    {
        Vector2 Result = new Vector2();

        Vector2 PlayerLocation = new Vector2(this.transform.position.x, this.transform.position.y);

        float Xmin = PlayerLocation.x - (SpawnSize / 2);
        float Xmax = PlayerLocation.x + (SpawnSize / 2);

        float Ymin = PlayerLocation.y - (SpawnSize / 2);
        float Ymax = PlayerLocation.y + (SpawnSize / 2);

        if (_identifier == "min")
        {
            Result = new Vector2(Xmin, Ymin);
        }
        else if (_identifier == "max")
        {
            Result = new Vector2(Xmax, Ymax);
        }

        return Result;
    }

    List<Vector2> GenerateObjectLocations()
    {
        List<Vector2> Result1 = new List<Vector2>();
        List<Vector2> FinalResult = new List<Vector2>();

        List<Vector2> PossibleSpawnLocations = new List<Vector2>();
        List<Vector2> PossibleSpawnLocations2 = new List<Vector2>();

        Vector2 MinSpawnLocation = CalculateSpawnArea("min");
        Vector2 MaxSpawnLocation = CalculateSpawnArea("max");

        // Get a Random Set of Coordinates within the Spawn Size (Results Skewed to Bottom Left Side)
        for (float x = MinSpawnLocation.x; x < MaxSpawnLocation.x; x++)
        {
            for (float y = MinSpawnLocation.y; y < MaxSpawnLocation.y; y++)
            {
                if (Random.Range(1, (SpawnSize*2) + 1) % (x + y) < Random.Range(0, SpawnSize))
                {
                    PossibleSpawnLocations.Add(new Vector2(x, y));
                }
            }
        }

        // Get a Random Set of Coordinates within the Spawn Size (Results Skewed to Bottom Right Side)
        for (float x = MinSpawnLocation.x; x < MaxSpawnLocation.x; x++)
        {
            for (float y = MinSpawnLocation.y; y < MaxSpawnLocation.y; y++)
            {
                if (Random.Range(1, (SpawnSize * 2) + 1) % (x + y) > Random.Range(0, SpawnSize))
                {
                    PossibleSpawnLocations2.Add(new Vector2(x, y));
                }
            }
        }

        // Using the 2 Skewed Results and Perlin Noise Get a List of coordinates to use
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

        // Run through List of Possible Spawn Locations and if
        // a random number from 0 to the total of x, y is less than the spawn chance 
        // that's the location we'll use to spawn the objects
        for (int i = 0; i < Result1.Count; i++)
        {
            if (Random.Range(0, Result1[i].x + Result1[i].y + 1) < SpawnChance)
            {
                FinalResult.Add(Result1[i]);
            }
        }

        return FinalResult;
    }
}
