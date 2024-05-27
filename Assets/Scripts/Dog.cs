using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public TopDownGameManager manager;
    public QuestManager questManager;
    public Rigidbody2D target;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    //GameObject scanObject;
    public int nextMove;
    public bool isColliding;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 5);
    }


    void FixedUpdate()
    {

        if (manager.isAction)
        {
            nextMove = 0;
            rigid.velocity = Vector2.zero; // rigidbody의 속도를 0으로 설정하여 움직임을 멈춤
            anim.SetBool("isWalking", false); // 걷는 애니메이션을 false로 설정
        }

        if (questManager.isMoving)
        {
            CancelInvoke("Think");
            spriteRenderer.flipX = false;
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * 2 * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
            anim.SetBool("isWalking", true);

            float distanceToTarget = Vector2.Distance(rigid.position, target.position);
            if (distanceToTarget < 0.1f) // 도착을 판단하는 임계값으로 0.1f를 사용합니다. 필요에 따라 조절하세요.
            {
                // 이동을 멈추고 isWalking을 false로 설정합니다.
                rigid.velocity = Vector2.zero;
                spriteRenderer.flipX = true;
                anim.SetBool("isWalking", false);
                questManager.isMoving = false; // 목적지 도달 시 이동을 멈추려면 questManager.isMoving을 false로 설정합니다.
            }
        }

        if (questManager.reThink)
        {
            questManager.reThink = false;
            Think();
        }

        //Move
        // 1: up, 2:down, 3:left, 4:right
        if (nextMove == 0)
            rigid.velocity = new Vector2(0, 0);
        else if (nextMove == 1)
            rigid.velocity = new Vector2(0, 1);
        else if (nextMove == 2)
            rigid.velocity = new Vector2(0, -1);
        else if (nextMove == 3)
            rigid.velocity = new Vector2(-1, 0);
        else if (nextMove == 4)
            rigid.velocity = new Vector2(1, 0);

        /*
        //Ray 나중에 영역 설정하면 이거 하기
        Debug.DrawRay(rigid.position, dirVec * 1.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 1.0f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject;
        else
            scanObject = null;

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.forward, 1, LayerMask.GetMask("Object"));
        if (rayhit.collider == null)
            rigid.velocity = new Vector2(0, 1);*/
    }

    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(0, 5);

        //Sprite Animation
        if(nextMove == 0)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        //Flip Sprite
        if (nextMove == 3)
            spriteRenderer.flipX = true;
        else if (nextMove == 4)
            spriteRenderer.flipX = false;

        //Recursive
        Invoke("Think", 5);
    }
}
