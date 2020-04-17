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
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i].z != vertices[i].x)
            {
                vertices[i] += Vector3.up;
            }
            if(vertices[i].z == vertices[i].x && vertices[i].z > 0)
            {
                vertices[i] += Vector3.up*3;
            }
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        Vector3 normal = Vector3.Cross(Vector3.Scale(transform.localScale, vertices[2] - vertices[3]), Vector3.Scale(transform.localScale, vertices[2] - vertices[4]));
        Instantiate(point, Vector3.Scale(transform.localScale, mesh.bounds.center), Quaternion.identity, null);
        Debug.DrawLine(normal + Vector3.Scale(transform.localScale, mesh.bounds.center), Vector3.Scale(transform.localScale, mesh.bounds.center), Color.blue, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
