using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Power
{
    public GameObject player;
    public PowerType powerType;

    public Power() {
        this.player = GameObject.Find("Player");
    }

    public Power(GameObject player)
    {
        this.player = player;
    }

    public abstract void Activate();
}
