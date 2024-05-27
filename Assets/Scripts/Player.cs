using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    float moveSpeed = 2.2f; // 이동 속도
    bool isJumping = false;

    Rigidbody2D rigid;
    Animator anim;
    public GameObject panel1;
    public GameObject panel2;
    bool boolpanel1 = false;
    bool boolpanel2 = false;

    public AudioSource bgmSource;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // 초기 위치 설정
        //transform.position = new Vector3(-2.5f, -2.5f, 0);

        PlayBGM();
    }

    void PlayBGM()
    {
        bgmSource.loop = true; // BGM을 반복 재생
        bgmSource.Play();
    }

    void Update()
    {
        // 오른쪽으로 이동하는 벡터 설정
        Vector3 moveDirection = Vector3.right;

        // Time.deltaTime을 곱하여 프레임 속도에 상관없이 일정한 속도로 이동하도록 함
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);


        //Ray
        Vector2 dirVec = Vector2.right;
        Debug.DrawRay(rigid.position, dirVec * 10.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 9.0f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            panel1.SetActive(true);
            if (!isJumping)
            {
                moveSpeed = 0;
                isJumping = true;
                rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                Invoke("changePanel", 1);
            }
        }

        if (boolpanel1)
        {
            moveSpeed = 3;
            panel1.SetActive(false);
            panel2.SetActive(true);
            boolpanel1 = false;
        }
        if (boolpanel2)
        {
            moveSpeed = 1.8f;
            panel1.SetActive(false);
            panel2.SetActive(false);
        }

        if (moveSpeed != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);

    }

    public void changePanel()
    {
        boolpanel1 = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 wife인 경우
        if (collision.gameObject.tag == "Enemy")
        {
            boolpanel2 = true;
        }

        if (collision.gameObject.tag == "Boss")
        {
            gameObject.SetActive(false);
            Invoke("nextStage", 2);
        }
    }

    void nextStage()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Ending");
    }
}
