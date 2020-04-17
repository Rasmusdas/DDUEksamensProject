using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IPoolObject
{
    public TileEntity entity;
    private bool destroying;
    public float heightTarget;
    public float lerpValue;
    public TileEntity lastEntity;
    public bool keepTiles;
    public PoolType type;

    public GameObject xWall;
    public GameObject yWall;
    public GameObject cWall;
    public GameObject tree;
    public Vector2 placement;
    public House house;
    public bool verbose;

    public GameObject test;
    Mesh mesh;

    public void Deactivate()
    {
        if(tree && xWall && yWall && cWall)
        {
            xWall.SetActive(false);
            yWall.SetActive(false);
            cWall.SetActive(false);
            tree.SetActive(false);
        }
    }

    public void PoolStart()
    {
        Vector3 normalVector = GetNormalVector();
        if (!name.Contains("Building"))
        {
            FixEdges();
        }
        else
        {
            normalVector = Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 100f) ? GetNormalVector(hit.transform.GetComponent<MeshFilter>().mesh) : GetNormalVector(GetComponent<MeshFilter>().mesh);
            FixEdges();
            FixRotation(transform, normalVector);
        }
        if(tree && xWall && yWall && cWall)
        {
            FixRotation(xWall.transform, normalVector);
            FixRotation(yWall.transform, normalVector);
            FixRotation(cWall.transform, normalVector);
            FixRotation(tree.transform, normalVector, true);
        }
        
    }

    void FixedUpdate()
    {
        
        if (!destroying)
        {
            float lastdist = Mathf.Infinity;
            lastEntity = entity;
            foreach (TileEntity tE in CreateTiles.tileEntities)
            {
                float dist = Vector3.Distance(transform.position, tE.gameObject.transform.position);
                if (Vector3.Distance(transform.position, tE.gameObject.transform.position) < lastdist)
                {
                    lastdist = dist;
                    lastEntity = tE;
                }
            }
            entity = lastEntity;
            if(lerpValue < 1)
            {
                lerpValue += Time.deltaTime;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0,lerpValue), transform.position.z);
            }
            
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(entity.gameObject.transform.position.x, entity.gameObject.transform.position.z)) > entity.dissolveRange + 1)
            {
                lerpValue = 0;
                destroying = true;
                StartCoroutine(DestroyTile());
            }
        }
    }

    IEnumerator DestroyTile()
    {
        if(transform.position.y <= 2.5f+heightTarget)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 3, lerpValue), transform.position.z);
            lerpValue += Time.deltaTime;
        }
        else
        {
            lerpValue = 0;
            ObjectPooling.objectPool.poolDictionary[type].Enqueue(gameObject);
            destroying = false;
            if(type == PoolType.Normal)
            {
                CreateTiles.tilePlacement[Mathf.FloorToInt(placement.x), Mathf.FloorToInt(placement.y)] = false;
            }
            else if(type == PoolType.House)
            {
                house.generated = false;
            }
            Deactivate();
            gameObject.SetActive(false);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(DestroyTile());
    }

    void FixEdges()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for (int z = 0; z < vertices.Length; z++)
        {
            float height = Mathf.Exp(Mathf.PerlinNoise((transform.position.x + 500 + vertices[z].x) * 0.01f, (transform.position.z + 500 + vertices[z].z) * 0.01f)*3) * 2;
            vertices[z] = new Vector3(vertices[z].x, height + vertices[z].y, vertices[z].z);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void FixRotation(Transform trans, Vector3 normalVector, bool affectChildObjects = false)
    {
        if(affectChildObjects)
        {
            for (int i = 0; i < trans.transform.childCount; i++)
            {
                Transform treeChild = trans.transform.GetChild(i);
                treeChild.localPosition = mesh.bounds.center;
                treeChild.up = normalVector.normalized;
            }
        }
        else
        {
            trans.localPosition = mesh.bounds.center;
            trans.up = normalVector.normalized;
        }
    }

    Vector3 GetNormalVector()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        return GetNormalVector(mesh);
    }

    Vector3 GetNormalVector(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 normal = Vector3.Cross(vertices[2] - vertices[3], vertices[2] - vertices[4]);
        return normal;
    }
}
