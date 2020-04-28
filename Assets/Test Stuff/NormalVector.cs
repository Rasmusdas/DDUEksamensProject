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
        Debug.DrawLine(normal + mesh.bounds.center, mesh.bounds.center, Color.blue, 100);
        Instantiate(stick, mesh.bounds.center, Quaternion.identity, null).transform.up = normal.normalized;
    }
    Vector3 GetNormalVector(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 normal = Vector3.Cross(vertices[2] - vertices[4], vertices[2] - vertices[3]);
        return normal;
    }
}
