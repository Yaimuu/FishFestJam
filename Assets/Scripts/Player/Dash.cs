using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Dash : ActivePower
{
    public float dashSpeed;

    private void Initialize()
    {
        cooldown = 1000;
        buttonName = "Dash";
        dashSpeed = 500f;
    }

    public Dash() : base()
    {
        Initialize();
    }

    public Dash(GameObject player) : base(player) {
        Initialize();
    }

    /**
     * Dash effect
     */
    protected override void Effect()
    {
        Debug.Log(buttonName);
        isUsed = true;

        //Up vector is forward in 2D
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Player doesn't have a Rigidbody2D component!");
            return;
        }

        Vector2 forward = player.transform.right;
        Debug.Log(rb.velocity);
        //rb.velocity += forward * dashSpeed;
        rb.AddForce(forward * dashSpeed, ForceMode2D.Impulse);
        //player.transform.position += player.transform.right * dashSpeed;
    }
}
