using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Big Boi Script

public class CreateTiles : MonoBehaviour
{
    public int xSize, ySize;
    public static int seed;

    public Material blue, grey, yellow, white, green;
    public GameObject tile;
    public ObjectPooling objectPools;
    public GameObject treeTile;
    public TileEntity[] entities;
    public static TileEntity[] tileEntities;
    public static List<GameObject> generatedTiles = new List<GameObject>();
    public int mapSize;
    public static bool[,] tilePlacement;
    public List<Pair> pairList = new List<Pair>();

    public static Dictionary<Color, Material> mats = new Dictionary<Color, Material>();
    

    private void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        tilePlacement = new bool[1000, 1000];
        foreach (var v in pairList)
        {
            mats.Add(v.col, v.mat);
        }
        objectPools = ObjectPooling.objectPool;
        tileEntities = entities;
    }

    void FixedUpdate()
    {
        GenerateFromTileMap();
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
                        float height = Mathf.PerlinNoise((i + seed + Mathf.FloorToInt(tE.gameObject.transform.position.x)), (j + seed + Mathf.FloorToInt(tE.gameObject.transform.position.z)));
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
                        /*
                        for (int z = 0; z < vertices.Length; z++)
                        {
                            height = Mathf.PerlinNoise((i + randomVariation + Mathf.RoundToInt(tE.gameObject.transform.position.x) + (vertices[z].x * 2)) * 0.01f, (i + randomVariation + Mathf.RoundToInt(tE.gameObject.transform.position.z) + (vertices[z].z * 2)) * 0.01f);
                            vertices[z] = new Vector3(vertices[z].x, height - newTile.transform.position.y + vertices[z].y, vertices[z].z);
                        }
                        mesh.vertices = vertices;
                        */
                    }
                }
            }
        }
    }

    void GenerateFromTileMap()
    {
        foreach (TileEntity tE in tileEntities)
        {
            for (int i = (-tE.xSize / 2); i < (tE.xSize / 2); i++)
            {
                for (int j = (-tE.xSize / 2); j < (tE.ySize / 2); j++)
                {
                    bool generateTile = true;
                    Vector3 pos = new Vector3(Mathf.FloorToInt(tE.gameObject.transform.position.x) + i, 0, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j);
                    if (Vector2.Distance(new Vector2(tE.gameObject.transform.position.x, tE.gameObject.transform.position.z), new Vector2(pos.x,pos.z)) >= tE.dissolveRange)
                    {
                        continue;
                    }

                    if (tilePlacement[Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2])
                    {
                        continue;
                    }

                    if (generateTile)
                    {
                        tilePlacement[Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2] = true;
                        Vector2 tilePlace = new Vector2(Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2);
                        GameObject newTile = objectPools.InstantiateFromPool(PoolType.Normal, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)));
                        //Instantiate(tile, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3 + height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)), Quaternion.identity, null);
                        Texture2D gameMap = ExportTileMap.gameMap;
                        Color pixelColor = gameMap.GetPixel(Mathf.FloorToInt(tE.gameObject.transform.position.x) + i + mapSize / 2, Mathf.FloorToInt(tE.gameObject.transform.position.z) + j + mapSize / 2);

                        foreach(var v in pairList)
                        {
                            if(ColorCloseEnough(v.col, pixelColor))
                            {
                                pixelColor = v.col;
                            }
                        }
                        if(mats.ContainsKey(pixelColor))
                        {
                            newTile.GetComponent<MeshRenderer>().material = mats[pixelColor];
                        }
                        else
                        {
                            newTile.GetComponent<MeshRenderer>().material = pairList[0].mat;
                        }
                        
                        Tile tile = newTile.GetComponent<Tile>();
                        tile.entity = tE;
                        tile.placement = tilePlace;
                       
                        if (pixelColor == new Color(0.2f,0.4f,0.2f))
                        {
                            tile.tree.SetActive(true);
                        }
                        if (pixelColor == Color.red)
                        {
                            tile.yWall.SetActive(true);
                        }
                        if (pixelColor == new Color(0.8f, 0.8f, 0.8f))
                        {
                            tile.xWall.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public bool ColorCloseEnough(Color c,Color b)
    {
        if(Mathf.Abs(c.r - b.r) < 0.03f && Mathf.Abs(c.g - b.g) < 0.03f && Mathf.Abs(c.b - b.b) < 0.03f)
        {
            return true;
        }
        else
        {
            return false;
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

[System.Serializable]
public class Pair
{
    public Material mat;
    public Color col;

    public Pair(Material mat, Color col)
    {
        this.mat = mat;
        this.col = col;
    }
}
