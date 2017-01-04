using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateBoundingBoxes : MonoBehaviour {
    public GameObject model;
	// Use this for initialization
	void Start () {
		
	}

    private void Recalculate()
    {
        SkinnedMeshRenderer[] meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
           
        }
    }
}
