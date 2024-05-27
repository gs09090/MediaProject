using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wife : MonoBehaviour
{
    float moveSpeed = 0f; // �̵� �ӵ�
    Animator anim;
    SpriteRenderer wifeSprite;

    void Start()
    {
        anim = GetComponent<Animator>();
        wifeSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ���������� �̵��ϴ� ���� ����
        Vector3 moveDirection = Vector3.right;

        // Time.deltaTime�� ���Ͽ� ������ �ӵ��� ������� ������ �ӵ��� �̵��ϵ��� ��
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� Player�� ���
        if (collision.gameObject.tag == "Player")
        {
            print("�浹");
            // �������� ��������Ʈ�� �ݴ� �������� ������
            if (wifeSprite != null)
            {
                wifeSprite.flipX = true;
            }

            // ���������� �̵��ϴ� �ӵ��� 3���� ����
            moveSpeed = 1.75f;
        }

        if (collision.gameObject.tag == "Boss")
        {
            gameObject.SetActive(false);
        }
    }
}
