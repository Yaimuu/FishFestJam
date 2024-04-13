using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float lifespan = 0.1f;
    public float damages = 0.1f;
    
    public float damping = 10f;
    public float minVelocity = 0.1f;

    public Vector2 sizeBounds = new Vector2(0.1f, 0.5f);
    public Vector2 frequencyBounds = new Vector2(20, 25);
    public Vector2 amplitudeBounds = new Vector2(0.5f, 2f);

    private Vector2 dir = Vector2.one;
    private float acc = 1f;
    private float velocity = 0f;

    private Vector2 initialPos = Vector2.zero;
    private float waveFrequence;
    private float waveAmplitude;

    public Vector2 Dir { get => dir; set => dir = value; }
    public float Acc { get => acc; set => acc = value; }
    public float Velocity { get => velocity; set => velocity = value; }

    // Start is called before the first frame update
    void Start()
    {
        float randSize = Random.Range(sizeBounds.x, sizeBounds.y);
        transform.localScale = new Vector3(randSize, randSize, transform.localScale.z);

        StartCoroutine(DelayExplode());

        initialPos = transform.localPosition;

        waveFrequence = Random.Range(frequencyBounds.x, frequencyBounds.y);
        waveAmplitude = Random.Range(amplitudeBounds.x, amplitudeBounds.y);
    }

    // Update is called once per frame
    void Update()
    {  
        // Calculates the following velocity value in function of acceleration
        Velocity = Velocity + Acc * Time.deltaTime;
        // Reducing the velocity over time to get better bubble launch effect
        Velocity -= damping * Time.deltaTime;
        // Avoiding the velocity to reach 0
        Velocity = Mathf.Max(Velocity, minVelocity);
        
        Acc = Velocity / Time.time;
        Move();
    }

    /** Move
     * Moves the bubble along the up axis while giving it a wave movement effect
     */
    void Move()
    {
        // Applies the velocity to the position
        transform.position += new Vector3(Dir.x, Dir.y, 0) * Velocity * Time.deltaTime;

        // Compute wave motion
        float waveOffsetX = Mathf.Sin(Time.time * waveFrequence) * waveAmplitude;
        float waveOffsetY = Mathf.Cos(Time.time * waveFrequence) * waveAmplitude;

        transform.position += new Vector3(waveOffsetX, waveOffsetY, 0) * Time.deltaTime;
    }

    /** DelayExplode
     * The bubble explodes after the lifespan expires
     */
    IEnumerator DelayExplode()
    {
        yield return new WaitForSeconds(lifespan);
        Explode();
    }

    /**
     * Destroys the bubble gameobject and the script attached to it
     */
    void Explode()
    {
        // TODO: Explosion Animation

        Destroy(gameObject);
        Destroy(this);
    }

    /**
     * On trigger enter the bubble is destroyed
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Bubble")
            return;

        // TODO: Inflict damage to ennemies
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.Hit(damages);
        }

        Explode();
    }
}
