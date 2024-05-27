using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public float maxSpeed; //가속을 무한으로 받지 않기 위해서
    public float jumpPower;
    public float rayDistance = 1;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    //각각 액션마다 클립을 바꾸고 재생하는 함수를 생성하여 활용
    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }

    private void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && !gameManager.talkSet.activeSelf)
        { //&& 부분 추가하여 무한점프 막기
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
        }

        //Stop Speed(키보드에서 손을 떼면 속력 급박하게 줄기)
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            //normalized : 벡터 크기를 1로 만든 상태 (단위벡터)
            //키를 땔 때의 속도를 0.5f로 지정해 놓은 상태
        }

        //Direction Sprite(방향전환)
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        if(!gameManager.talkSet.activeSelf)
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y <= 0)
        {
            //Debugging ray
            Debug.DrawRay(rigid.position, Vector3.down * rayDistance*1.5f, new Color(1, 0, 0));
            //Ray Collider Check
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, rayDistance*1.5f, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                //Debug.Log(rayHit.collider.name);
                //Player의 가운데에서 빔이 쏘기 시작하고 Player의 크기는 1이므로 절반인 0.5f
                float adjustedDistance = transform.localScale.y * 0.5f; // 플레이어 크기에 따른 거리 조정
                if (rayHit.distance < adjustedDistance+0.45)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Attack
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y) //몬스터보다 위에 있음 + 아래로 낙하 중 = 밟음
            {
                OnAttack(collision.transform);
            }
            else //Damaged
                OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            PlaySound("ITEM");
            //Point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;

            gameManager.stagePoint += 100;

            //Deactive Item
            collision.gameObject.SetActive(false);


            bool isBig = collision.gameObject.name.Contains("Big");
            bool isSmall = collision.gameObject.name.Contains("Small");
            bool islifeItem = collision.gameObject.name.Contains("lifeItem");

            if (isBig)
            {
                collision.gameObject.SetActive(false);
                StartCoroutine(Big());
            }
            else if (isSmall)
            {
                collision.gameObject.SetActive(false);
                StartCoroutine(Small());
            }
            else if(islifeItem)
            {
                collision.gameObject.SetActive(false);
                gameManager.HealthUp();
            }
        }
        else if (collision.gameObject.tag == "bossBullet")
        {
            PlaySound("DAMAGED");
            gameManager.PlayerReposition();
        }
        else if (collision.gameObject.tag == "Finish")
        {
            PlaySound("FINISH");
            //Next Stage
            gameManager.NextStage();
        }
    }

    IEnumerator Big()
    {
        // 오브젝트 크기를 크게 설정
        transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        rayDistance = 1.5f;
        float originSpeed = maxSpeed;
        maxSpeed *= 0.75f;
        yield return new WaitForSeconds(5.0f);

        // 3번 깜빡임 효과 적용
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.color = new Color(1, 1, 1, 1.0f);
            yield return new WaitForSeconds(1.3f);
        }

        rayDistance = 1;
        maxSpeed = originSpeed;

        // 오브젝트 크기를 원래대로 되돌림
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator Small()
    {
        // 오브젝트 크기를 작게 설정
        transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        float originSpeed = maxSpeed;
        maxSpeed *= 1.5f;
        yield return new WaitForSeconds(5.0f);

        // 3번 깜빡임 효과 적용
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.color = new Color(1, 1, 1, 1.0f);
            yield return new WaitForSeconds(1.3f);
        }

        maxSpeed = originSpeed;

        // 오브젝트 크기를 원래대로 되돌림
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.3f);
    }


    //OnAttack()함수에 몬스터의 죽음관련 함수를 호출
    void OnAttack(Transform enemy)
    {
        PlaySound("ATTACK");

        //Point
        gameManager.stagePoint += 100;

        //Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        PlaySound("DAMAGED");

        //Heaith Down
        gameManager.HealthDown();

        //Change Layer (Importal Active)
        gameObject.layer = 11;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 10, ForceMode2D.Impulse);

        //Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);

    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        PlaySound("DIE");
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        capsuleCollider.enabled = false;
        //boxCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
