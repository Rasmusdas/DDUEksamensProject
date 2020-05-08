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
        Vector3 normalVector = Vector3.zero;
        if (!name.Contains("Building"))
        {
            FixEdges();
            Debug.DrawLine(normalVector + new Vector3(transform.position.x, heightTarget + mesh.bounds.center.y, transform.position.z), new Vector3(transform.position.x, heightTarget + mesh.bounds.center.y, transform.position.z), Color.blue, 100);
        }
        if (tree && xWall && yWall && cWall && !name.Contains("Building"))
        {
            normalVector = GetNormalVector();
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
                lerpValue += Time.fixedDeltaTime;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0+heightTarget,lerpValue), transform.position.z);
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
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 3+heightTarget, lerpValue), transform.position.z);
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
            if (house != null)
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
            vertices[z] = new Vector3(vertices[z].x, height, vertices[z].z);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void FixRotation(Transform trans, Vector3 normalVector, bool affectChildObjects = false)
    {
        if (affectChildObjects)
        {
            for (int i = 0; i < trans.transform.childCount; i++)
            {
                trans.transform.GetChild(i).gameObject.SetActive(false);
            }

            Transform treeChild = trans.transform.GetChild(Random.Range(0,trans.transform.childCount));
            treeChild.gameObject.SetActive(true);
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);
            treeChild.localPosition = mesh.bounds.center + new Vector3(x, (Mathf.Exp(Mathf.PerlinNoise((trans.position.x + x + 500) * 0.01f, (trans.position.z + y + 500) * 0.01f) * 3) * 2) - (Mathf.Exp(Mathf.PerlinNoise((trans.position.x + 500) * 0.01f, (trans.position.z + 500) * 0.01f) * 3) * 2), y);
            treeChild.up = normalVector.normalized;
        }
        else
        {
            trans.localPosition = mesh.bounds.center;
            trans.up = normalVector.normalized;
        }
    }

    public void FixBuildings(Transform trans)
    {
        Mesh meshyboi = Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 100f) ? hit.transform.GetComponent<MeshFilter>().mesh : null ;
        if(meshyboi == null)
        {
            return;
        }
        Vector3 normalVector = GetNormalVector(meshyboi);
        heightTarget = (Mathf.Exp(Mathf.PerlinNoise((trans.position.x + 500) * 0.01f, (trans.position.z + 500) * 0.01f) * 3) * 2);
        trans.up = normalVector.normalized;
        trans.Rotate(0, house.rotationAmount, 0);
    }

    Vector3 GetNormalVector()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        return GetNormalVector(mesh);
    }

    Vector3 GetNormalVector(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 normal = Vector3.Cross(vertices[1] - vertices[2], vertices[1] - vertices[0]);
        return normal;
    }

}
