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
    public float maxSpeed; //������ �������� ���� �ʱ� ���ؼ�
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

    //���� �׼Ǹ��� Ŭ���� �ٲٰ� ����ϴ� �Լ��� �����Ͽ� Ȱ��
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
        { //&& �κ� �߰��Ͽ� �������� ����
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
        }

        //Stop Speed(Ű���忡�� ���� ���� �ӷ� �޹��ϰ� �ٱ�)
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            //normalized : ���� ũ�⸦ 1�� ���� ���� (��������)
            //Ű�� �� ���� �ӵ��� 0.5f�� ������ ���� ����
        }

        //Direction Sprite(������ȯ)
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
                //Player�� ������� ���� ��� �����ϰ� Player�� ũ��� 1�̹Ƿ� ������ 0.5f
                float adjustedDistance = transform.localScale.y * 0.5f; // �÷��̾� ũ�⿡ ���� �Ÿ� ����
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
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y) //���ͺ��� ���� ���� + �Ʒ��� ���� �� = ����
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
        // ������Ʈ ũ�⸦ ũ�� ����
        transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        rayDistance = 1.5f;
        float originSpeed = maxSpeed;
        maxSpeed *= 0.75f;
        yield return new WaitForSeconds(5.0f);

        // 3�� ������ ȿ�� ����
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.color = new Color(1, 1, 1, 1.0f);
            yield return new WaitForSeconds(1.3f);
        }

        rayDistance = 1;
        maxSpeed = originSpeed;

        // ������Ʈ ũ�⸦ ������� �ǵ���
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator Small()
    {
        // ������Ʈ ũ�⸦ �۰� ����
        transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        float originSpeed = maxSpeed;
        maxSpeed *= 1.5f;
        yield return new WaitForSeconds(5.0f);

        // 3�� ������ ȿ�� ����
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.color = new Color(1, 1, 1, 1.0f);
            yield return new WaitForSeconds(1.3f);
        }

        maxSpeed = originSpeed;

        // ������Ʈ ũ�⸦ ������� �ǵ���
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.3f);
    }


    //OnAttack()�Լ��� ������ �������� �Լ��� ȣ��
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
