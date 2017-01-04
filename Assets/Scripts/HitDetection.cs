using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour {
    //private Animator myAnimator;
    private SkeletonController myController;
    public string TriggerName;
	// Use this for initialization
	void Start () {
        myController = GetComponentInParent<SkeletonController>();
        if (TriggerName == null) Debug.LogError("No TriggerName assigned for hit trigger script at: " + gameObject.name);
	}
	
    void OnTriggerEnter(Collider other)
    {
        //if (!other.gameObject.CompareTag(this.gameObject.tag))
        
            myController.HitZoneEnter(TriggerName, other.gameObject.name);
        
        
    }

    void OnTriggerExit(Collider other)
    {
        //if (!other.gameObject.CompareTag(this.gameObject.tag))

        myController.HitZoneExit(TriggerName, other.gameObject.name);
    }
}
