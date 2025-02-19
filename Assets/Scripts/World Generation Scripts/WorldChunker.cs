using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChunker : MonoBehaviour
{
    private const int MaxNumberChunks = 9;  //For Each Chunk Around the Center Chunk

    private GameObject CurrentChunk;
    private float ChunkXSize;
    private float ChunkZSize;

    private Vector3 InitPos;

    [Header("Tiles")]
    public List<GameObject> WorldChunks = new List<GameObject>();

    [Header("Tile Materials")]
    public List<Material> WorldMaterials = new List<Material>();

    [Header("Map Generation Values")]
    public Vector3 TileChangeDistance = new Vector3(100.0f, 0.0f, 100.0f);

    // Start is called before the first frame update
    void Start()
    {
        //Set Center Chunk
        CurrentChunk = WorldChunks[0];

        //Set Scale Diff (If placed flush to center the Top Right Corner will give Positive Size Values)
        ChunkXSize = WorldChunks[3].transform.position.x;
        ChunkZSize = WorldChunks[3].transform.position.z;

        //Set the Initial Position
        InitPos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        WorldReArranger(other);
    }

    void WorldReArranger(Collider _other)
    {
        if (CurrentChunk.name != _other.transform.parent.name)
        {
            //Update Current Chunk
            for (int i = 0; i < MaxNumberChunks; i++)
            {
                if (WorldChunks[i] == _other.transform.parent.gameObject)
                {
                    //Position in Array (To Match the Chunk Locations)
                    WorldChunks[0] = _other.transform.parent.gameObject;
                    WorldChunks[i] = CurrentChunk;

                    Transform TempPos = WorldChunks[i].transform;
                    WorldChunks[i].transform.position = WorldChunks[0].transform.position;
                    WorldChunks[0].transform.position = TempPos.position;

                    //Set to 0 Directional Material
                    Material new_material = Instantiate(WorldMaterials[Random.Range(0, WorldMaterials.Count)]);

                    WorldChunks[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;

                    CurrentChunk = WorldChunks[0];

                    break;
                }
            }

            for (int i = 0; i < MaxNumberChunks; i++)
            {
                ChunkPositioner(i);

                TileChanger(WorldChunks[i]);
            }
        }
    }

    int WorldTilePicker(Vector3 _tilepos)
    {
        int MaterialIndex = 0;

        //Beyond Material 2
        if (Vector3.Distance(InitPos, _tilepos) > Vector3.Distance(InitPos, (InitPos + (2 * TileChangeDistance))))
        {
            MaterialIndex = 1;
        }

        //Material 2
        if (Vector3.Distance(InitPos, _tilepos) < Vector3.Distance(InitPos, (InitPos + (2 * TileChangeDistance))) && Vector3.Distance(InitPos, _tilepos) > Vector3.Distance(InitPos, (InitPos + TileChangeDistance)))
        {
            MaterialIndex = 1;
        }

        //Material 1
        if (Vector3.Distance(InitPos, _tilepos) < Vector3.Distance(InitPos, (InitPos + TileChangeDistance)))
        {
            MaterialIndex = 0;
        }

        return MaterialIndex;
    }

    void TileChanger(GameObject _tile)
    {
        Material new_material = Instantiate(WorldMaterials[WorldTilePicker(_tile.transform.position)]);

        _tile.transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
    }

    void TileRotator(int _i, float _Xpos, float _Zpos)
    {
        float oldperlin = Mathf.PerlinNoise(WorldChunks[_i].transform.position.x, WorldChunks[_i].transform.position.z);

        if (_i == 1)
        {
            //Top Left
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", 135.0f);
            }
        }

        //Top
        if (_i == 2)
        {
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", 90.0f);
            }
        }

        if (_i == 3)
        {
            //Top Right
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", 45.0f);
            }
        }

        if (_i == 4)
        {
            //Left
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", 180.0f);
            }
        }

        if (_i == 5)
        {
            //Right
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", 360.0f);
            }
        }

        if (_i == 6)
        {
            //Bottom Left
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", -135.0f);
            }
        }

        //Bottom
        if (_i == 7)
        {
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", -90.0f);
            }
        }

        if (_i == 8)
        {
            //Bottom Right
            if (Mathf.Abs(oldperlin - Mathf.PerlinNoise(_Xpos, _Zpos)) <= 0.1f)
            {
                Material new_material = Instantiate(WorldMaterials[0]);

                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material = new_material;
                WorldChunks[_i].transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Rotation", -45.0f);
            }
        }
    }

    void ChunkPositioner(int _i)
    {
        if (_i == 1)
        {
            //Top Left
            float Xpos = WorldChunks[0].transform.position.x - (WorldChunks[_i].transform.localScale.x * ChunkXSize);
            float Zpos = WorldChunks[0].transform.position.z + (WorldChunks[_i].transform.localScale.z * ChunkZSize);

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 2)
        {
            //Top
            float Xpos = WorldChunks[0].transform.position.x;
            float Zpos = WorldChunks[0].transform.position.z + (WorldChunks[_i].transform.localScale.z * ChunkZSize);

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 3)
        {
            //Top Right
            float Xpos = WorldChunks[0].transform.position.x + (WorldChunks[_i].transform.localScale.x * ChunkXSize);
            float Zpos = WorldChunks[0].transform.position.z + (WorldChunks[_i].transform.localScale.z * ChunkZSize);

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 4)
        {
            //Left
            float Xpos = WorldChunks[0].transform.position.x - (WorldChunks[_i].transform.localScale.x * ChunkXSize);
            float Zpos = WorldChunks[0].transform.position.z;

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 5)
        {
            //Right
            float Xpos = WorldChunks[0].transform.position.x + (WorldChunks[_i].transform.localScale.x * ChunkXSize);
            float Zpos = WorldChunks[0].transform.position.z;

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 6)
        {
            //Bottom Left
            float Xpos = WorldChunks[0].transform.position.x - (WorldChunks[_i].transform.localScale.x * ChunkXSize);
            float Zpos = WorldChunks[0].transform.position.z - (WorldChunks[_i].transform.localScale.z * ChunkZSize);

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 7)
        {
            //Bottom
            float Xpos = WorldChunks[0].transform.position.x;
            float Zpos = WorldChunks[0].transform.position.z - (WorldChunks[_i].transform.localScale.z * ChunkZSize);

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }

        if (_i == 8)
        {
            //Bottom Right
            float Xpos = WorldChunks[0].transform.position.x + (WorldChunks[_i].transform.localScale.x * ChunkXSize);
            float Zpos = WorldChunks[0].transform.position.z - (WorldChunks[_i].transform.localScale.z * ChunkZSize);

            WorldChunks[_i].transform.position = new Vector3(Xpos, 0, Zpos);
        }
    }
}
