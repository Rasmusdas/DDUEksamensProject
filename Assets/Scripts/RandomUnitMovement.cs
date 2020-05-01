using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For testing multiple TileEntities on the map
public class RandomUnitMovement : MonoBehaviour
{
    public float movement;
    private void Start()
    {
        StartCoroutine(Move(100));
    }

    IEnumerator Move(int step)
    {
        if(step <= 0)
        {
            transform.Rotate(0, Random.Range(0, 180), 0);
            StartCoroutine(Move(100));
        }
        else
        {
            yield return new WaitForEndOfFrame();
            transform.Translate(0,0,movement*Time.deltaTime);
            StartCoroutine(Move(step - 1));
        }
    }
}
