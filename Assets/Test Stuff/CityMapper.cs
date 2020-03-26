using System.Collections.Generic;
using UnityEngine;

public class CityMapper : MonoBehaviour
{
    public bool trigger;
    Vector2[,] newGrid;
    public GameObject cube;
    public GameObject miniCube;
    public GameObject line;

    void Start()
    {
        DiskSampling.GenerateDiskSamples(3,35,15,15, out Vector2[,] grid);
        int xCount = 0;
        int yCount = 0;
        List<List<Vector2>> cityPoints = new List<List<Vector2>>();
        for (int j = 0; j < grid.GetLength(0); j++)
        {
            List<Vector2> newList = new List<Vector2>();
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                if(grid[i, j].x != Mathf.NegativeInfinity && grid[i, j].y != Mathf.NegativeInfinity)
                {
                    newList.Add(grid[i,j]);
                }
            }
            if(newList.Count != 0)
            {
                cityPoints.Add(newList);
            }
        }

        foreach (var l in cityPoints)
        {
            foreach(Vector2 loc in l)
            {
                Instantiate(cube, new Vector3(loc.x,0,loc.y), Quaternion.Euler(0,Random.Range(0,4)*90,0), null);
            }
        }

    }
}
