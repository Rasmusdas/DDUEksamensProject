using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPivot : MonoBehaviour
{

    public void FixPivotPoint()
    {
        GetComponent<MeshFilter>().mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh.RecalculateTangents();
        GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }

}
