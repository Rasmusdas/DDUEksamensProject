using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MapGeneration : MonoBehaviour
{
    public float xDist, yDist, heightMod;
    public int xSize, ySize;
    private Mesh mesh;
    private Vector3[] vertices;
    List<GameObject> trees = new List<GameObject>();
    public GameObject tree;

    public Transform playerPosition;
    private void Start()
    {
        
    }

    private void Update()
    {
        while (trees.Count > 0)
        {
            DestroyImmediate(trees[0]);
            trees.RemoveAt(0);
        }
        Generate();
    }

    private void Generate()
    {
        trees = new List<GameObject>();
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                Vector3 mapPosition = playerPosition.position + new Vector3(x-xSize/2,0, y-ySize/2);
                float height = Mathf.PerlinNoise(mapPosition.x * xDist,mapPosition.z * yDist);
                if(height > 0.9f)
                {
                    Physics.Raycast(mapPosition+Vector3.up * playerPosition.position.y,-playerPosition.up, out RaycastHit hit,10);
                }
                vertices[i] = new Vector3(mapPosition.x, height*heightMod, mapPosition.z);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.vertices = vertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
