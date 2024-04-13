using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public GameObject bubblePrefab;

    public float shootingForce = 10f;
    // Nb Bubble per seconds
    public float shootingRate = 10f;

    [SerializeField] 
    private GameObject healthPanel;
    [SerializeField]
    private int health = 3;

    private bool isShooting = false;
    private bool isHurt = false;
    
    private List<Image> hearts;

    private Rigidbody2D rb;
    private Animator animator;
    private List<Power> powers = new List<Power>();

    private List<Scroll> scrolls = new List<Scroll>();
    [SerializeField]
    private GameObject scrollUI;

    //private int Health { 
    //    get => health; 

    //    set
    //    {
    //        health = value > hearts.Count ? hearts.Count : value;
    //        health = health < 0 ? 0 : health;

    //        //if (healthPanel != null)
    //        //    Hit();
    //        Debug.Log(health);
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        powers.Add(new Dash(gameObject));

        if (healthPanel != null)
        {
            hearts = new List<Image>(healthPanel.GetComponentsInChildren<Image>());
            hearts.Remove(healthPanel.GetComponent<Image>());

            UpdateHealth();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
        ShootBubbles();
    }

    private void FixedUpdate()
    {
        ActivatePowers();
    }

    /** ShootBubbles
     * Instances new bubble in the player location and sets its direction, acceleration and velocity
     */
    private void ShootBubbles()
    {
        if(Input.GetButton("Fire1") && !isShooting)
        {

            if (bubblePrefab == null)
                return;

            isShooting = true;

            GameObject newBubble = Instantiate(bubblePrefab);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            newBubble.transform.position = transform.position + transform.right * (spriteRenderer.bounds.size.x/2);

            Bubble newBubbleScript = newBubble.GetComponent<Bubble>();
            newBubbleScript.Dir = transform.right;
            newBubbleScript.Acc = shootingForce;
            newBubbleScript.Velocity = shootingForce + rb.velocity.magnitude;

            StartCoroutine(ShootingDelay());
        }
    }

    /**
     * Sets the delay between two shoots according to the shootingRate
     */
    IEnumerator ShootingDelay()
    {
        yield return new WaitForSeconds(1 / shootingRate);
        isShooting = false;
    }

    /** UpdateHealth
     * 
     */
    private void UpdateHealth()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if(i < health)
            {
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].color = Color.black;
                Debug.Log(hearts[i]);
            }
        }
    }

    /** ActivatePowers
     * Activate each power the character has
     */
    private void ActivatePowers()
    {
        foreach (Power power in powers)
        {
            power.Activate();
        }
    }

    /** Move function
     * Controls characters movements
     */
    private void Move()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Set player velocity based on input
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;
        Vector2 velocity = movementDirection * moveSpeed;
        rb.velocity = velocity;

        // Compute rotation angle based on velocity direction
        if (velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            
            // Flip character horizontally if angle is in [-90, 90]
            if (angle >= -90f && angle <= 90f)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
            }

            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /**
     * Adds a Scroll item to the inventory
     */
    public void AddScroll(Scroll scroll)
    {
        if(scrolls.Contains(scroll))
            return;

        scrolls.Add(scroll);
        scrollUI.GetComponentInChildren<TextMeshProUGUI>().text = scrolls.Count.ToString();
    }

    /**
     * Triggered when the player gets hurt
     */
    public void Hit(int damages)
    {
        if (isHurt)
            return;

        health -= damages;
        health = health > hearts.Count ? hearts.Count : health;
        health = health < 0 ? 0 : health;

        // TODO: Hit animation
        isHurt = true;

        UpdateHealth();

        StartCoroutine(HurtingDelay());
    }

    IEnumerator HurtingDelay()
    {
        yield return new WaitForSeconds(1);
        isHurt = false;
    }
}
