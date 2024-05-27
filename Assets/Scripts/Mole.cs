using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public TopDownGameManager manager;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public Vector2 targetPosition; // ��ǥ ��ġ
    public float moveSpeed = 2f; // �̵� �ӵ�
    public int nextFlip;
    public bool isActive;
    public GameObject mole;

    // �̵� ���� ����
    private float minX = -59f;
    private float maxX = -48f;
    private float minY = -15f;
    private float maxY = 17f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Think(); // �ʱ� Think ȣ��
    }

    void FixedUpdate()
    {
        // ��ǥ ��ġ�� �̵�
        Vector2 newPosition = Vector2.MoveTowards(rigid.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rigid.MovePosition(newPosition);

        if (manager.isAction)
        {
            CancelInvoke("Think"); // Think ȣ�� ���
            mole.SetActive(true); // �δ��� Ȱ��ȭ
            rigid.velocity = Vector2.zero; // �ӵ��� 0���� �����Ͽ� �������� ����
        }
    }

    void Think()
    {
        // ��ǥ ��ġ�� ���� ������ ����
        targetPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        // ��������Ʈ ������ ����
        nextFlip = Random.Range(0, 2);
        spriteRenderer.flipX = nextFlip == 0;

        // ���� ȣ�� �ð� ����
        float nextTime = Random.Range(1f, 3f); // �� �ǹ� �ִ� �ð� �������� ����

        if (isActive)
        {
            mole.SetActive(true); // �δ��� Ȱ��ȭ
            isActive = false;
        }
        else
        {
            mole.SetActive(false); // �δ��� ��Ȱ��ȭ
            isActive = true;
        }

        // ��� ȣ��
        Invoke("Think", nextTime);
    }
}
