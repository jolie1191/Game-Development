using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            //Debug.Log(other.name);
            StartCoroutine(HitCoin(other.gameObject));

        }
    }

    //利用携程，制造出一种人在吃金币的过程：金币位置无限向人的位置趋近
    IEnumerator HitCoin(GameObject coin)
    {
        bool isLoop = true;
        while(isLoop)
        {
            coin.transform.position = Vector3.Lerp(coin.transform.position,
                                              PlayerController.instance.gameObject.transform.position,
                                              Time.deltaTime * 5);
            if(Vector3.Distance(coin.transform.position, PlayerController.instance.gameObject.transform.position) < 0.5f)
            {
                coin.GetComponent<Coin>().HitItem();
                isLoop = false;
            }
            yield return null;
        }
    }
}


