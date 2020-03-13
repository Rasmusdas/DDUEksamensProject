using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeReshaper : MonoBehaviour
{
    public GameObject cube;
    public int size;
    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float height = Mathf.PerlinNoise(i * 0.2f, j * 0.2f)*5;
                GameObject cubeSpawned = Instantiate(cube, new Vector3(i, height, j), Quaternion.identity, null);
                cubeSpawned.name = "CUBE" + j;
                Mesh mesh = cubeSpawned.GetComponent<MeshFilter>().mesh;
                Vector3[] vertices = mesh.vertices;
                for (int z = 0; z < vertices.Length; z++)
                {
                    height = Mathf.PerlinNoise((i + vertices[z].x) * 0.2f, (j + vertices[z].z) * 0.2f)*3;
                    Debug.Log(cubeSpawned.name + " " + height);
                    vertices[z] = new Vector3(vertices[z].x, height - cubeSpawned.transform.position.y + vertices[z].y, vertices[z].z);
                }
                mesh.vertices = vertices;
            }
        }
    }
}
