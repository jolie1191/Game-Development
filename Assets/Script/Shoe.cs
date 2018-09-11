using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : Item {

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            PlayerController.instance.UseShoe();
        }
    }
}
