using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public TopDownGameManager manager;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public Vector2 targetPosition; // 목표 위치
    public float moveSpeed = 2f; // 이동 속도
    public int nextFlip;
    public bool isActive;
    public GameObject mole;

    // 이동 범위 설정
    private float minX = -59f;
    private float maxX = -48f;
    private float minY = -15f;
    private float maxY = 17f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Think(); // 초기 Think 호출
    }

    void FixedUpdate()
    {
        // 목표 위치로 이동
        Vector2 newPosition = Vector2.MoveTowards(rigid.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rigid.MovePosition(newPosition);

        if (manager.isAction)
        {
            CancelInvoke("Think"); // Think 호출 취소
            mole.SetActive(true); // 두더지 활성화
            rigid.velocity = Vector2.zero; // 속도를 0으로 설정하여 움직임을 멈춤
        }
    }

    void Think()
    {
        // 목표 위치를 범위 내에서 설정
        targetPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        // 스프라이트 뒤집기 설정
        nextFlip = Random.Range(0, 2);
        spriteRenderer.flipX = nextFlip == 0;

        // 다음 호출 시간 설정
        float nextTime = Random.Range(1f, 3f); // 더 의미 있는 시간 간격으로 조정

        if (isActive)
        {
            mole.SetActive(true); // 두더지 활성화
            isActive = false;
        }
        else
        {
            mole.SetActive(false); // 두더지 비활성화
            isActive = true;
        }

        // 재귀 호출
        Invoke("Think", nextTime);
    }
}
