using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class used to show the health of entities other than the player
 */
public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if(healthBar == null)
        {
            healthBar = GetComponent<Slider>();
        }
        if(camera == null)
        {
            camera = Camera.main;
        }
        if(target == null)
        {
            target = transform.GetComponentInParent<Enemy>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = camera.transform.rotation;
        //transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
    }
}
