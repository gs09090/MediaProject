using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePeatingBackground : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private float groundHorizontalLength;

    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizontalLength = groundCollider.size.x;
    }

    void Update()
    {
        //if(transform.position.x < -groundHorizontalLength)
        //{
        //    RepositionBackground();
        //}

        if (transform.position.x < -groundHorizontalLength)
        {
            transform.position = new Vector2(groundHorizontalLength * 1.0f, transform.position.y);
        }
    }

    private void RepositionBackground()
    {
        Vector2 groundOffset = new Vector2(groundHorizontalLength * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffset;
        transform.position = (Vector2)transform.position + new Vector2(groundHorizontalLength * 2f, 0);
    }
}
