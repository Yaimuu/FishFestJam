using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ActivePower : Power
{
    // Value in ms
    public int cooldown = 1000;
    public string buttonName = "";
    public bool isUsed = false;

    public ActivePower() : base() { }

    public ActivePower(GameObject player) : base(player) { }

    public override void Activate()
    {
        if (Input.GetButton(buttonName) && !isUsed)
        {
            Effect();
            _ = Cooldown();
        }
    }

    protected virtual void Effect() { }

    public async Task Cooldown()
    {
        await Task.Delay(cooldown);
        // Set your value here after the delay
        isUsed = false;
        Debug.Log($"{buttonName} cooldown is over..");
    }
}
