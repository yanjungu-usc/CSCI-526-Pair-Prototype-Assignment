using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityField : MonoBehaviour
{

    public float gravitationalForce = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = new Vector2(transform.position.x, transform.position.y) - new Vector2(collision.transform.position.x, collision.transform.position.y);
                rb.AddForce(direction.normalized * gravitationalForce);
            }
        }
    }
}
