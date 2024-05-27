using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wife : MonoBehaviour
{
    float moveSpeed = 0f; // 이동 속도
    Animator anim;
    SpriteRenderer wifeSprite;

    void Start()
    {
        anim = GetComponent<Animator>();
        wifeSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 오른쪽으로 이동하는 벡터 설정
        Vector3 moveDirection = Vector3.right;

        // Time.deltaTime을 곱하여 프레임 속도에 상관없이 일정한 속도로 이동하도록 함
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 Player인 경우
        if (collision.gameObject.tag == "Player")
        {
            print("충돌");
            // 와이프의 스프라이트를 반대 방향으로 뒤집음
            if (wifeSprite != null)
            {
                wifeSprite.flipX = true;
            }

            // 오른쪽으로 이동하는 속도를 3으로 설정
            moveSpeed = 1.75f;
        }

        if (collision.gameObject.tag == "Boss")
        {
            gameObject.SetActive(false);
        }
    }
}
