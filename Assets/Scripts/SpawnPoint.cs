using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public float spawnFrequency = 2f;
    public GameObject enemy;
    public float spawnTimeout = 0;
    public Transform player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimeout += Time.deltaTime;
        if (spawnTimeout >= spawnFrequency)
        {
            GameObject spawnedInstance = Instantiate(enemy, transform.position, Quaternion.identity) as GameObject;
            spawnedInstance.GetComponent<SkeletonController>().target = player;
            spawnTimeout = 0;
        }
        
    }
}
