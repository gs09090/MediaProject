using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossGameManager gameManager;
    public AudioClip audioDamaged;
    public AudioClip audioAttack;
    public AudioClip audioDie;

    AudioSource audioSource;

    public float speed;
    public int health;
    public Sprite[] sprites;
    public Sprite[] healthBar;

    public float maxShotDeley;
    public float curShotDeley;

    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    //SpriteRenderer healthSpriteRenderer;

    public int nextMove;

    public int patternIndex;

    public GameObject bulletObj;
    public GameObject playerObject;
    public Transform playerPos;
    private BossPlayerMove playerMove;
    public int startHealth;

    public Transform ShotPosition;

    bool scaleUp = false;

    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");


        if (playerObject != null)
        {
            playerMove = playerObject.GetComponent<BossPlayerMove>();
        }

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //healthSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health <= 20)
        {
            spriteRenderer.sprite = sprites[1];
            //transform.localScale *= 1.5f; 계속 커지는.. 폭팔하는 줄
            if(scaleUp == false)
            {
                transform.localScale *= 1.5f;
                boxCollider.transform.localScale = transform.localScale;
                scaleUp = true;
            }
        }

    }


    void OnEnable()
    {
        health = 40;
        startHealth = 40;
        Invoke("Stop", 2);
    }

    void Stop()
    {
        if (!gameObject.activeSelf) return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();

        //InvokeRepeating("Think", 2, 2);
        Invoke("Think", 2);
    }

    void Think()
    {
        if (health == 0)
            return;
        else if (health > 30)
            patternIndex = 0;
        else if (health > 20)
            patternIndex = 1;
        else if (health > 10)
            patternIndex = 2;
        else
            patternIndex = 3;


        switch (patternIndex)
        {
            case 0:
                UDLR();
                break;
            case 1:
                targetShot();
                break;
            case 2:
                randomRotShot();
                break;
            case 3:
                circleShot();
                break;
        }
    }


    void UDLR()
    {
        if (health <= 0)
            return;

        Debug.Log("1");

        GameObject bulletU = Instantiate(bulletObj);
        GameObject bulletD = Instantiate(bulletObj);
        GameObject bulletL = Instantiate(bulletObj);
        GameObject bulletR = Instantiate(bulletObj);

        bulletU.transform.position = ShotPosition.position;
        bulletD.transform.position = ShotPosition.position;
        bulletL.transform.position = ShotPosition.position;
        bulletR.transform.position = ShotPosition.position;

        Quaternion rotU = Quaternion.Euler(0, 0, 0);
        Quaternion rotD = Quaternion.Euler(0, 0, 90);
        Quaternion rotL = Quaternion.Euler(0, 0, 180);
        Quaternion rotR = Quaternion.Euler(0, 0, 270);

        bulletU.transform.rotation = rotU;
        bulletD.transform.rotation = rotD;
        bulletL.transform.rotation = rotL;
        bulletR.transform.rotation = rotR;

        Destroy(bulletU, 2f);
        Destroy(bulletD, 2f);
        Destroy(bulletR, 2f);
        Destroy(bulletL, 2f);

        PlaySound("ATTACK");

        Invoke("Think", 3);
    }

    void targetShot()
    {
        if (health <= 0)
            return;

        Debug.Log("2");

        GameObject bullet = Instantiate(bulletObj);

        Vector3 direction = playerPos.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.transform.position = ShotPosition.position;

        //bullet.transform.rotation = ShotPosition.rotation;

        Destroy(bullet, 3f);

        PlaySound("ATTACK");

        Invoke("Think", 3);
    }
    void randomRotShot()
    {
        if (health <= 0)
            return;

        Debug.Log("3");

        GameObject bulletL = Instantiate(bulletObj);
        GameObject bulletC = Instantiate(bulletObj);
        GameObject bulletR = Instantiate(bulletObj);

        bulletL.transform.position = ShotPosition.position;
        bulletC.transform.position = ShotPosition.position;
        bulletR.transform.position = ShotPosition.position;

        float ran = Random.Range(0, 360);

        Quaternion rotL = Quaternion.Euler(0, 0, ran - 10);
        Quaternion rotC = Quaternion.Euler(0, 0, ran);
        Quaternion rotR = Quaternion.Euler(0, 0, ran + 10);

        bulletL.transform.rotation = rotL;
        bulletC.transform.rotation = rotC;
        bulletR.transform.rotation = rotR;

        Destroy(bulletL, 3f);
        Destroy(bulletC, 3f);
        Destroy(bulletR, 3f);

        PlaySound("ATTACK");

        Invoke("Think", 3);
    }
    void circleShot()
    {
        if (health <= 0)
            return;

        Debug.Log("4");

        for (int i = 0; i < 360; i += 30)
        {
            GameObject temp = Instantiate(bulletObj);
            Destroy(temp, 2f);
            temp.transform.position = ShotPosition.position;
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        PlaySound("ATTACK");

        Invoke("Think", 3);
    }


    public void OnDamaged(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        Invoke("ReturnSprite", 0.1f);
        PlaySound("DAMAGED");
        gameManager.BossHealth();
        
        if (health <= 0)
        {
            playerObject.GetComponent<BossPlayerMove>().bossDie = true;
            //엔딩으로 가는 부분 구현
            //playerObject.GetComponent<BossPlayerMove>().BossFinish();
            PlaySound("DIE");
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerBullet")
        {
            OnDamaged(1);
            print("보스아파1");
        }
    }
    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerBullet")
        {
            OnDamaged(1);
            print("보스아파2");
        }
    }*/

    void PlaySound(string action)
    {
        switch (action)
        {
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
        }
        audioSource.Play();
    }
}
