using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameAttribute : MonoBehaviour {

    /*---------用来存储整个游戏的上下文---------*/

    //金币数量
    public int coin;

    //用于双倍积分
    public int multiply = 1;

    //暴露一个属性出来,使得其他脚本也可access到此脚本内容
    public static GameAttribute instance;

	public int life = 1;

	public Text Text_Coin; 

	// Use this for initialization
	void Start () {
        coin = 0;
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		Text_Coin.text = coin.ToString ();
	}

    public void AddCoin(int coinNUmber)
    {
        coin += multiply * coinNUmber;

    }
}
