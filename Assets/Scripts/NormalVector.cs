using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalVector : MonoBehaviour
{
    public bool nextPoint;
    public GameObject point;
    public int pointIndex;
    public Vector3[] vertices;
    public Mesh mesh;
    public GameObject stick;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i].z != vertices[i].x)
            {
                vertices[i] += Vector3.up/2;
            }
            if(vertices[i].z == vertices[i].x && vertices[i].z > 0)
            {
                vertices[i] += Vector3.up;
            }
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        Vector3 normal = GetNormalVector(mesh);
        Debug.DrawLine(normal+ mesh.bounds.center, mesh.bounds.center, Color.blue, 100);
        GameObject owo = Instantiate(stick, mesh.bounds.center, Quaternion.identity, null);
        owo.transform.up = normal;


    }
    Vector3 GetNormalVector(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 normal = Vector3.Cross(vertices[1] - vertices[0], vertices[1] - vertices[2]);
        return normal;
    }

    private void Update()
    {
        if(nextPoint)
        {
            nextPoint = false;
            Instantiate(point, vertices[pointIndex], Quaternion.identity);
            pointIndex++;
        }
    }
}
