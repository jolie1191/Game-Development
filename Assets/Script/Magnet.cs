using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Item {

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            PlayerController.instance.UseMagnet();
        }
    }
}
