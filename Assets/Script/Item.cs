using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    
    public float rotateSpeed = 1;
    //添加吃金币的效果
    public GameObject hitEffect;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public virtual void HitItem()
    {
        //GameAttribute.instance.coin++;
        //实列化吃金币特效， 特效位置--金币位置， 特效旋转--无
        GameObject effect = Instantiate(hitEffect);
        effect.transform.parent = PlayerController.instance.gameObject.transform;
        effect.transform.localPosition = new Vector3(0, 0.5f, 0);

        //这里gameObject指的是当前脚本挂载的主角--金币
        Destroy(gameObject);

    }

    //other 是碰撞的this物体的主角， 这里this是金币
    //其中要记的在主角中加个tag： 这里叫Player
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //GameAttribute.instance.coin++;
            //实列化吃金币特效， 特效位置--金币位置， 特效旋转--无
            /* GameObject effect = Instantiate(hitEffect);
             effect.transform.parent = PlayerController.instance.gameObject.transform;
             effect.transform.localPosition = new Vector3(0, 0.5f, 0);

             //这里gameObject指的是当前脚本挂载的主角--金币
             Destroy(gameObject);*/

            HitItem();
        }
    }
}
