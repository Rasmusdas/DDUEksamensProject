using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiles : MonoBehaviour
{
    public int xSize, ySize;
    public static int randomVariation;

    public Material blue, grey, yellow, white, green;
    public GameObject tile;
    public ObjectPooling objectPools;
    public GameObject treeTile;
    public TileEntity[] entities;
    public static TileEntity[] tileEntities;
    public static List<GameObject> generatedTiles = new List<GameObject>();
    public int mapSize;
    public static bool[,] tilePlacement;
    

    private void Awake()
    {
        randomVariation = Random.Range(-1000000,1000000);     
        tilePlacement = new bool[mapSize, mapSize];
    }

    private void Start()
    {
        objectPools = ObjectPooling.objectPool;
        tileEntities = entities;
    }

    void FixedUpdate()
    {
        GenerateTiles();
    }


    void GenerateTiles()
    {
        foreach (TileEntity tE in tileEntities)
        {
            for (int i = (-tE.xSize / 2); i < (tE.xSize / 2); i++)
            {
                for (int j = (-tE.xSize / 2); j < (tE.ySize / 2); j++)
                {
                    bool generateTile = true;
                    Vector3 pos = new Vector3(Mathf.FloorToInt(tE.gameObject.transform.position.x) + i, 0, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j);

                    if (Vector3.Distance(tE.gameObject.transform.position, pos) >= tE.dissolveRange)
                    {
                        continue;
                    }

                    if(tilePlacement[Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2])
                    {
                        continue;
                    }

                    if (generateTile)
                    {
                        tilePlacement[Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2] = true;
                        Vector2 tilePlace = new Vector2(Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2);
                        float treeGeneration = Mathf.PerlinNoise((i + Mathf.FloorToInt(tE.gameObject.transform.position.x)) * 0.1f, (j + Mathf.FloorToInt(tE.gameObject.transform.position.z)) * 0.1f);
                        float height = Mathf.PerlinNoise((i + randomVariation + Mathf.FloorToInt(tE.gameObject.transform.position.x)) * 0.01f, (j + randomVariation + Mathf.FloorToInt(tE.gameObject.transform.position.z)) * 0.01f);
                        GameObject newTile = objectPools.InstantiateFromPool(PoolType.Normal, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3+height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)));
                            //Instantiate(tile, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3 + height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)), Quaternion.identity, null);
                        if (height > 0.90f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = white;
                        }
                        else if (height > 0.75f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = grey;
                        }
                        //else if (height > 0.5f && treeGeneration > 0.7f)
                        //{
                        //    newTile = Instantiate(treeTile, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3 + height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)), Quaternion.identity, null);
                        //}
                        else if (height > 0.5f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = green;
                        }
                        else if (height > 0.35f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = yellow;
                        }
                        else if (height <= 0.35f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = blue;
                        }

                        Mesh mesh = newTile.GetComponent<MeshFilter>().mesh;
                        Vector3[] vertices = mesh.vertices;
                        Tile tile = newTile.GetComponent<Tile>();
                        tile.heightTarget = height;
                        tile.entity = tE;
                        tile.placement = tilePlace;
                        
                        for (int z = 0; z < vertices.Length; z++)
                        {
                            height = Mathf.PerlinNoise((i + randomVariation + Mathf.RoundToInt(tE.gameObject.transform.position.x) + (vertices[z].x * 2)) * 0.01f, (i + randomVariation + Mathf.RoundToInt(tE.gameObject.transform.position.z) + (vertices[z].z * 2)) * 0.01f);
                            vertices[z] = new Vector3(vertices[z].x, height - newTile.transform.position.y + vertices[z].y, vertices[z].z);
                        }
                        mesh.vertices = vertices;

                    }
                }
            }
        }
    }
}


[System.Serializable]
public class TileEntity
{
    public GameObject gameObject;
    public int xSize;
    public int ySize;
    public float dissolveRange;
}
