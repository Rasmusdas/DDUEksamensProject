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

    private void Awake()
    {
        randomVariation = Random.Range(-1000000,1000000);       
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
        foreach(TileEntity tE in tileEntities)
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

                    if (Physics.Raycast(pos + (Vector3.down * 10), Vector3.up, out RaycastHit hit, 100f))
                    {
                        if (hit.transform.tag != "Player")
                        {
                            continue;
                        }
                    }

                    if (generateTile)
                    {
                        float treeGeneration = Mathf.PerlinNoise((i + Mathf.FloorToInt(tE.gameObject.transform.position.x)) * 0.1f, (j + Mathf.FloorToInt(tE.gameObject.transform.position.z)) * 0.1f);
                        float height = Mathf.PerlinNoise((i + randomVariation + Mathf.FloorToInt(tE.gameObject.transform.position.x)) * 0.1f, (j + randomVariation + Mathf.FloorToInt(tE.gameObject.transform.position.z)) * 0.1f);
                        GameObject newTile = objectPools.InstantiateFromPool(PoolType.Normal, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3 + height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)));
                            //Instantiate(tile, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3 + height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)), Quaternion.identity, null);
                        if (height > 0.90f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = white;
                        }
                        else if (height > 0.75f)
                        {
                            newTile.GetComponent<MeshRenderer>().material = grey;
                        }
                        else if (height > 0.5f && treeGeneration > 0.7f)
                        {
                            DestroyImmediate(newTile);
                            newTile = Instantiate(treeTile, new Vector3(i + Mathf.FloorToInt(tE.gameObject.transform.position.x), 3 + height, j + Mathf.FloorToInt(tE.gameObject.transform.position.z)), Quaternion.identity, null);
                        }
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
                        newTile.GetComponent<Tile>().heightTarget = height;
                        newTile.GetComponent<Tile>().entity = tE;
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
