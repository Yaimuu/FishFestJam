using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool debug = false;
    public float speed;
    public int damages;

    [SerializeField]
    protected float maxHealth;

    protected float health;

    protected Rigidbody2D rb;
    protected Animator animator;

    protected GameObject loot;
    [SerializeField]
    protected FloatingHealthBar healthBar;

    public float Health { 
        get => health;
        set { 
            health = value;
            
            if (health <= 0)
            {
                Die();
            }
        } 
    }

    // Start is called before the first frame update
    protected void Start()
    {
        health = maxHealth;
        Debug.Log(health);
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<FloatingHealthBar>();
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    /** Born
     * Initialization with a fancy name
     */
    protected void Born()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /** Hit
     * Is called when the enemy gets hurt
     */
    public void Hit(float dmg)
    {
        // TODO: Feedbacks (Animation + Sound effects)
        Health -= dmg;

        healthBar.UpdateHealthBar(Health, maxHealth);
    }

    /** Die
     * Is called when the enemy has no health
     */
    protected void Die()
    {
        // TODO: Feedbacks (Animation + Sound effects)
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Destroy(gameObject);
        Destroy(this);
    }

    /**
     * Get damages if a Bubble is collided and 
     * deals damage to Player if he is the collider
     */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject.tag);
            collision.gameObject.GetComponent<Player>().Hit(damages);
        }
    }
}
