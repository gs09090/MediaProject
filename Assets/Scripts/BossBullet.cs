using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    private BossPlayerMove playerMove;
    private BossPlayerAttack playerAttack;
    private SpriteRenderer spriteRenderer;
    private GameObject playerObject;

    private int direction;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        playerObject = GameObject.FindWithTag("Player");


        if (playerObject != null)
        {
            playerMove = playerObject.GetComponent<BossPlayerMove>();
            playerAttack = playerObject.GetComponent<BossPlayerAttack>();
        }

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        
        if (playerAttack.skill != 1)
            spriteRenderer.flipX = playerMove.direction == -1;
        else
            spriteRenderer.flipX = playerMove.direction == 1;

        if (playerAttack.skill != 2)
            rb2d.velocity = transform.right * playerMove.direction * speed;
        

        //만약 Boss일 경우
        if (gameObject.CompareTag("Boss"))
        {
            float dir = playerObject.transform.position.x - transform.position.x;
            dir = (dir < 0) ? 1 : -1;
            spriteRenderer.flipX = dir == -1;
            rb2d.velocity = transform.right * dir * speed;
        }
    }

    private void FixedUpdate()
    {
        if (playerAttack.skill == 2 && gameObject.CompareTag("playerBullet"))
        {
            if (playerMove.direction == 1)
                //transform.Translate(new Vector2(1,1));
                transform.position = new Vector3(playerObject.transform.position.x + 2.5f, playerObject.transform.position.y, playerObject.transform.position.z);
            else
                transform.position = new Vector3(playerObject.transform.position.x - 2.5f, playerObject.transform.position.y, playerObject.transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "playerBullet")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }

}
