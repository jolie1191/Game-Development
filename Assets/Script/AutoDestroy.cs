using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    //设定默认销毁时间
    public float destroyTime = 0.7f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
