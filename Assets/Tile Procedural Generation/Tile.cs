using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileEntity entity;
    private bool destroying;
    public float heightTarget;
    public float lerpValue;
    public TileEntity lastEntity;
    public bool keepTiles;
    public PoolType type;

    public Vector2 placement;
    void Start()
    {

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
            if (transform.position.y > heightTarget)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, heightTarget, lerpValue), transform.position.z);
                lerpValue += Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, entity.gameObject.transform.position) > entity.dissolveRange + 1)
            {
                lerpValue = 0;
                destroying = true;
                if(keepTiles)
                {
                    DestroyImmediate(this);
                }
                else
                {
                    StartCoroutine(DestroyTile());
                }
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
            CreateTiles.tilePlacement[Mathf.FloorToInt(placement.x), Mathf.FloorToInt(placement.y)] = false;
            gameObject.SetActive(false);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(DestroyTile());
    }
}
