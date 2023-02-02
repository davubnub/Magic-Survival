using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectGenerator1 : MonoBehaviour
{
    public GameObject ObjecttoSpawn;
    public int num_points;
    public float radius;

    [Header("Debugging Purpose Only")]
    public int Seed = 0;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Seed);

        List<Vector2> spawnlocations = Convert_float_to_Vector(num_points, radius);

        for (int i = 0; i < spawnlocations.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(ObjecttoSpawn, new Vector3(spawnlocations[i].x, this.transform.position.y, spawnlocations[i].y), Quaternion.identity);
        }
    }

    float random_gaussian()
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.Range(0.0f, 1.0f) - 1.0f;
            v = 2.0f * Random.Range(0.0f, 1.0f) - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        return u * fac;
    }

    List<List<float>> random_directions(int _num_points, int _dimension)
    {
        List<List<float>> random_directions = new List<List<float>>();

        for (int i = 0; i < _dimension; i++)
        {
            List<float> dimension = new List<float>();

            for (int j = 0; j < _num_points; j++)
            {
                dimension.Add(random_gaussian());
            }

            random_directions.Add(dimension);
        }

        return random_directions;
    }

    float frobeniusNorm(List<List<float>> matrix)
    {
        // To store the sum of squares of the
        // elements of the given matrix
        float sumSq = 0;
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                sumSq += Mathf.Pow(matrix[i][j], 2.0f);
            }
        }

        // Return the square root of
        // the sum of squares
        float res = Mathf.Sqrt(sumSq);

        return res;
    }

    List<List<float>> apply_frobeniusNorm_to_random_directions(List<List<float>> random_directions)
    {
        float frobeniusNorm_value = frobeniusNorm(random_directions);

        List<List<float>> frobeniusNorm_applied = new List<List<float>>();

        for (int i = 0; i < random_directions.Count; i++)
        {
            List<float> dimension = new List<float>();

            for (int j = 0; j < random_directions[i].Count; j++)
            {
                float applied = random_directions[i][j] / frobeniusNorm_value;

                dimension.Add(applied);
            }

            frobeniusNorm_applied.Add(dimension);
        }

        return random_directions;
    }

    List<List<float>> points_in_given_radius(List<List<float>> random_directions, int num_points, float dimension)
    {
        float random_radii = Mathf.Pow(Random.Range(0, num_points + 1), (1 / dimension));

        for (int i = 0; i < random_directions.Count; i++)
        {
            for (int j = 0; j < random_directions[i].Count; j++)
            {
                random_directions[i][j] *= random_radii;
            }
        }

        return random_directions;
    }

    List<List<float>> transpose_points(List<List<float>> points)
    {
        float longest;

        if (points.Count != 0)
        {
            float max = 0;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points[i].Count; j++)
                {
                    if (max < points[i][j])
                    {
                        max = points[i][j];
                    }
                }
            }

            longest = max;
        }
        else
        {
            longest = 0;
        }

        List<List<float>> outer = new List<List<float>>();
        for (int i = 0; i < longest; i++)
            outer.Add(new List<float>(points.Count));
        for (int j = 0; j < points.Count; j++)
            for (int i = 0; i < longest; i++)
                outer[i].Add(points[j].Count > i ? points[j][i] : default(float));
        return outer;
    }

    List<List<float>> points_on_radius(List<List<float>> points, float radius)
    {
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = 0; j < points[i].Count; j++)
            {
                points[i][j] *= radius;
            }
        }

        return points;
    }

    List<List<float>> random_locations(int _num_points, int _dimension, float _radius)
    {
        List<List<float>> random_directions_list = random_directions(_num_points, _dimension);
        List<List<float>> random_directions_normalized = apply_frobeniusNorm_to_random_directions(random_directions_list);

        List<List<float>> random_directions_radius = points_in_given_radius(random_directions_normalized, _num_points, _dimension);
        List<List<float>> random_directions_transposed = transpose_points(random_directions_radius);
        List<List<float>> final_points = points_on_radius(random_directions_transposed, _radius);

        return final_points;
    }

    List<Vector2> Convert_float_to_Vector(int num_points, float radius)
    {
        // 2 because the game is 2-dimensional (I may need to rework the math if we change the no. dimensions)
        List<List<float>> points = random_locations(num_points, 2, radius);

        List<Vector2> spawnpoints = new List<Vector2>();

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 spawnlocation = new Vector2(points[i][0], points[i][1]);

            spawnpoints.Add(spawnlocation);
        }

        return spawnpoints;
    }
}