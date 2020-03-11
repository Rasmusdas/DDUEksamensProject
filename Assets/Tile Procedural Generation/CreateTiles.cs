using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiles : MonoBehaviour
{
    public int xSize, ySize;
    public static int randomVariation;

    public GameObject tile;
    public GameObject player;
    public static List<GameObject> generatedTiles = new List<GameObject>();

    private void Awake()
    {
        randomVariation = Random.Range(-1000000,1000000);       
    }

    private void Start()
    {
        player = PlayerMovement.playerReference;
    }

    void Update()
    {
        GenerateTiles();
    }


    void GenerateTiles()
    {
        for (int i = (-xSize / 2); i < (xSize / 2); i++)
        {
            for (int j = (-ySize / 2); j < (ySize / 2); j++)
            {
                bool generateTile = true;
                foreach(GameObject gb in generatedTiles)
                {
                    if (new Vector2(gb.transform.position.x, gb.transform.position.z) == new Vector2(i+ Mathf.FloorToInt(player.transform.position.x), j+ Mathf.FloorToInt(player.transform.position.z)))
                    {
                        generateTile = false;
                        break;
                    }
                }
                if(generateTile)
                {
                    float height = Mathf.PerlinNoise((i + randomVariation + Mathf.FloorToInt(player.transform.position.x)) * 0.1f, (j + randomVariation+ Mathf.FloorToInt(player.transform.position.z)) * 0.1f);
                    GameObject newTile = Instantiate(tile, new Vector3(i+ Mathf.FloorToInt(player.transform.position.x), 3+height, j+ Mathf.FloorToInt(player.transform.position.z)), Quaternion.identity, null);
                    if (height > 0.90f)
                    {
                        newTile.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    if (height > 0.75f)
                    {
                        newTile.GetComponent<MeshRenderer>().material.color = Color.grey;
                    }
                    else if (height > 0.5f)
                    {
                        newTile.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else if (height > 0.35f)
                    {
                        newTile.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    else if (height <= 0.35f)
                    {
                        newTile.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    newTile.GetComponent<Tile>().heightTarget = height;
                    generatedTiles.Add(newTile);
                }
            }
        }
    }
}
