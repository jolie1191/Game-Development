using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject target;//相机要跟随的目标：小人
    public float height;//相机和人之间的相对高度
    public float distance;//。。。相对距离
    Vector3 pos;

	// Use this for initialization
	void Start () {
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //让所有组件update完成之后，在执行以下function：确保相机和人位置信息不会错乱
    void LateUpdate()
    {
        /*this.transform.position = new Vector3(target.transform.position.x,
                                              target.transform.position.y + height,
                                              target.transform.position.z - distance);*/

        //实现相机跟随缓冲效果
        //pos.x = Mathf.Lerp(pos.x, target.transform.position.x, Time.deltaTime);
        pos.x = target.transform.position.x;
        pos.y = Mathf.Lerp(pos.y, target.transform.position.y + height, Time.deltaTime);
        //pos.z = Mathf.Lerp(pos.z, target.transform.position.z - distance, Time.deltaTime);
        pos.z = target.transform.position.z - distance;
        transform.position = pos;
    }
}
