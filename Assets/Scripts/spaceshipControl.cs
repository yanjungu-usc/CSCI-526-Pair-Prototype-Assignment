using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class spaceshipControl : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject centerBall;
    public float rotateSpeed = 100f;
    public float jumpForce = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void RotateAround(Vector2 center, float angularSpeed)
    {
        Vector2 toCenter = new Vector2(transform.position.x, transform.position.y) - center;

        float angle = angularSpeed * Time.deltaTime * Mathf.Deg2Rad;

        Vector2 rotated = new Vector2(
            toCenter.x * Mathf.Cos(angle) - toCenter.y * Mathf.Sin(angle),
            toCenter.x * Mathf.Sin(angle) + toCenter.y * Mathf.Cos(angle)
        );

        transform.position = center + rotated;
    }

    void Jump(Vector2 center)
    {
        Vector2 outOfCenter = new Vector2(transform.position.x, transform.position.y) - center;
        rb.AddForce(outOfCenter * jumpForce, ForceMode2D.Impulse);
        StartCoroutine(WaitAndFall(center));


    }

    IEnumerator WaitAndFall(Vector2 center)
    {
        yield return new WaitForSeconds(0.05f);
        Vector2 outOfCenter = new Vector2(transform.position.x, transform.position.y) - center;
        rb.AddForce(-outOfCenter * jumpForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateAround(centerBall.transform.position, -rotateSpeed);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateAround(centerBall.transform.position, rotateSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(centerBall.transform.position);
        }
    }
}
