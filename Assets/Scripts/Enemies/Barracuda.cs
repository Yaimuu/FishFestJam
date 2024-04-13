using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Barracuda : Enemy
{
    protected Transform target = null;

    [SerializeField]
    // Lower the value is, more stable the movements are
    protected float noiseScale = 0.1f;
    protected Vector2 noiseOffset;
    [SerializeField]
    protected float rotationSpeed;
    [SerializeField]
    protected List<Ray> rays;
    public int nbRays = 5;
    public float fieldOfView = 60;
    public bool isWandering = true;

    public Transform Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Start()
    {
        Born();
        GenerateRays();
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
        Move();
    }

    private void FixedUpdate()
    {
        UpdateRays();
        SearchObstacles();
        Chase();
    }

    protected void Move()
    {
        rb.velocity = transform.rotation * Vector2.right * speed;
    }

    /**
     * Barracuda's bahaviour when there is no target to chase
     */
    protected void Wander()
    {
        if(!isWandering)
            return;

        // Generate Perlin noise values
        float xNoise = Mathf.PerlinNoise(Time.time * noiseScale + noiseOffset.x, 0f);
        float yNoise = Mathf.PerlinNoise(0f, Time.time * noiseScale + noiseOffset.y);

        // Convert noise values to movement direction
        Vector2 noiseDirection = new Vector2(xNoise * 2f - 1f, yNoise * 2f - 1f).normalized;
        Vector2 velocity = noiseDirection * speed;

        if(debug)
            Debug.DrawLine(transform.position, transform.position + new Vector3(velocity.x, velocity.y, 0), UnityEngine.Color.red);

        // Apply movement
        //transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;

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

    protected void GenerateRays()
    {
        rays = new List<Ray>();
        
        float step = fieldOfView / nbRays;
        // Start from the leftmost angle of the field of view
        float angle = -fieldOfView / 2f;
        
        for (int i = 1; i <= nbRays; i++)
        {
            Vector3 dir = Quaternion.Euler(0, 0, angle) * transform.right;
            rays.Add(new Ray(transform.position, dir));
            angle += step;
        }
    }

    protected void UpdateRays()
    {
        float step = fieldOfView / nbRays;
        // Start from the leftmost angle of the field of view
        float angle = -fieldOfView / 2f;
        for (int i = 1; i <= nbRays; i++)
        {
            Vector3 dir = Quaternion.Euler(0, 0, angle) * transform.right;
            rays[i - 1] = new Ray(transform.position, dir);
            angle += step;

            if (debug)
                Debug.DrawLine(transform.position + rays[i - 1].direction, transform.position + rays[i - 1].direction * 10, UnityEngine.Color.blue);
        }
    }

    void Reach(Vector3 target)
    {
        float angle = Vector3.Angle(transform.position, target);
        
        Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);

        // Move the object towards the new position
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);
    }

    void SearchObstacles()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        float bestDist = 2.5f;
        Vector3 bestDir = Vector3.zero;
        Vector3 safeDir = Vector3.zero;
        int nbObstacles = 1;
        foreach (Ray ray in rays)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, ray.direction, 20f);
            
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider);
                if (hit.distance > bestDist)
                {
                    bestDist = hit.distance;
                    bestDir = ray.direction;
                }
                nbObstacles++;
            }
            else
            {
                safeDir = ray.direction;
            }
        }
        GetComponent<PolygonCollider2D>().enabled = true;

        float obstacleProportion = (float)nbObstacles / (float)rays.Count * 100;

        if (debug)
        {
            Debug.DrawRay(transform.position, bestDir * 15, UnityEngine.Color.yellow);
            Debug.Log(obstacleProportion);
        }

        if (obstacleProportion <= 20)
        {
            isWandering = true;
            return;
        }

        if(obstacleProportion >= 80)
        {
            bestDir = safeDir;
        }

        isWandering = false;

        Reach(bestDir);
    }

    /**
     * Reach the target
     */
    protected void Chase()
    {
        if(target == null) 
            return;


    }
}
