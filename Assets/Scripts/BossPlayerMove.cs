using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPlayerMove : MonoBehaviour
{
    public BossGameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioDie;
    public AudioClip audioFinish;


    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    BoxCollider2D boxCollider;
    AudioSource audioSource;

    private BossPlayerAttack playerAttack;

    public int direction = -1;

    private SpriteRenderer spriteSetting;

    public bool bossDie = false;
    public GameObject bossfinish;

    public static int EndingIdx;

    bool isJumping = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerAttack = this.GetComponent<BossPlayerAttack>();
        spriteSetting = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaySound("ATTACK");
        }

            //Jump
            if (Input.GetButton("Jump") && !isJumping)//&& !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJumping = true;
            //anim.SetBool("isJumping", true); //애니메이션
            PlaySound("JUMP");
        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {   //normalized: ???? ???? 1?? ????
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
            if (spriteRenderer.flipX)
            {
                direction = 1;
            }
            else
                direction = -1;
        }
        
        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        //여기서 구슬 효과 코드 짜야 합니다~!~!~!~~!
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameManager.ItemUse(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameManager.ItemUse(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameManager.ItemUse(3);
        }

    }
    void FixedUpdate()
    {
        //Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed)//Right max speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left max speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            //Debug.Log(rayHit.collider);
            if (rayHit.collider != null)
            {
                //Debug.Log(rayHit.distance);
                if (rayHit.distance < 3.0f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss" || collision.gameObject.tag == "bossBullet")
        {
            OnDamaged(collision.transform.position);
        }
        if (collision.gameObject.tag == "Platform")
            isJumping = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Time.timeScale = 0;
            gameManager.NextStage();
        }
    }

    public void BossFinish()
    {
        if (bossDie == true)
        {
            bossfinish.gameObject.SetActive(true);
        }
    }


    void OnDamaged(Vector2 targetPos)
    {
        //Health Down
        gameManager.HealthDown();
        // Change Layer (Immortal Active)
        gameObject.layer = 10;
        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        //Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);
        PlaySound("DAMAGED");
    }

    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        boxCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        PlaySound("DIE");
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
    
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
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }
}
