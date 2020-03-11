using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Transform player;
    private bool destroying;
    public float heightTarget;
    public float lerpValue;

    void Start()
    {
        player = PlayerMovement.playerReference.transform;
    }

    void Update()
    {
        if(!destroying)
        {
            if (transform.position.y > heightTarget)
            {
                transform.position = new Vector3(transform.position.x,Mathf.Lerp(transform.position.y,heightTarget,lerpValue),transform.position.z);
                lerpValue += 0.1f;
            }
            if (Vector3.Distance(transform.position, player.position) > 10)
            {
                lerpValue = 0;
                destroying = true;
                StartCoroutine(DestroyTile());
            }
        }
        
    }

    IEnumerator DestroyTile()
    {
        if(transform.position.y < 3)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 3+heightTarget, lerpValue), transform.position.z);
            lerpValue += 0.1f;
        }
        else
        {
            CreateTiles.generatedTiles.Remove(gameObject);
            DestroyImmediate(gameObject);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(DestroyTile());
    }
}
