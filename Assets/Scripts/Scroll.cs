using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public string text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Allows the Player to get the Scroll and keeps keeps it
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //TODO: Animation

            collision.GetComponent<Player>().AddScroll(this);

            Destroy(gameObject);
        }
    }
}
