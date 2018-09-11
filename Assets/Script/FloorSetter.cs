using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSetter : MonoBehaviour {

    public GameObject floorOnRunning;
    public GameObject floorForward;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.z > floorOnRunning.transform.position.z +32)
        {
            floorOnRunning.transform.position = new Vector3(0, 0,
                                                            floorForward.transform.position.z + 100);
            GameObject temp = floorOnRunning;
            floorOnRunning = floorForward;
            floorForward = temp;
        }
	}
}
