using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    float moveSpeed = 2.2f; // �̵� �ӵ�
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

        // �ʱ� ��ġ ����
        //transform.position = new Vector3(-2.5f, -2.5f, 0);

        PlayBGM();
    }

    void PlayBGM()
    {
        bgmSource.loop = true; // BGM�� �ݺ� ���
        bgmSource.Play();
    }

    void Update()
    {
        // ���������� �̵��ϴ� ���� ����
        Vector3 moveDirection = Vector3.right;

        // Time.deltaTime�� ���Ͽ� ������ �ӵ��� ������� ������ �ӵ��� �̵��ϵ��� ��
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
        // �浹�� ������Ʈ�� wife�� ���
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
