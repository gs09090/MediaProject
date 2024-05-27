using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject talkSet;
    public TMP_Text Text;
    public GameObject Cursor;
    public AudioSource typingSound; // Ÿ���� ����
    Dictionary<int, string[]> talkData;

    int countNum = 0; // ��ȭ ������ �ε���
    float typingSpeed = 0.1f; // Ÿ�� �ӵ� (�� ����)
    bool isTyping = false; // Ÿ���� ������ ����
    bool isComplete = false; // ������ ������ ��µǾ����� ����

    Coroutine typingCoroutine; // ���� Ÿ���� �ڷ�ƾ

    public GameObject house;
    public GameObject player;
    public GameObject fox1;
    public GameObject fox2;

    public int move = 0;
    int moveSpeed = 2;
    int foxSpeed = 2;
    bool isJumping = false;
    bool isBounding = false;

    SpriteRenderer sprite;
    Rigidbody2D rigid;

    public AudioSource bgmSource;

    void Start()
    {
        rigid = player.GetComponent<Rigidbody2D>();
        sprite = player.GetComponent<SpriteRenderer>();

        talkData = new Dictionary<int, string[]>();
        GenerateData();

        // ù ��ȭ ����
        //ShowText();
        Invoke("TextStart", 2);

        PlayBGM();
    }
    void PlayBGM()
    {
        bgmSource.loop = true; // BGM�� �ݺ� ���
        bgmSource.Play();
    }


    void TextStart()
    {
        talkSet.SetActive(true);
        ShowText();
        //countNum++;
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "���� ������ �Ƶ鸸 ���� ���� �� ��� �κΰ� �־���." });
        talkData.Add(1, new string[] { "�κδ� ���� ���� �ʹٴ� ������ ������ ������� ����ϴ� ������ ��ó ������ ġ���� ��ȴ�." });
        talkData.Add(2, new string[] { "��ħ�� ���ϵ� ���� �������� ��� �Ǿ���." });
        talkData.Add(3, new string[] { "���� ���� �θ�԰� �������� ����� �������ϸ� ������ ������������ �ڶ���." });
        talkData.Add(4, new string[] { "�׷��� ����� �׹ʸ� �Ǹ� ������ Ű��� �ҳ� ���� �ƹ��� ������ ���� �׾����." });
        talkData.Add(5, new string[] { "�ƹ����� �峲���� ������ �������� �ϰ�, �峲�� ���� ���� �ܾ簣�� ���״�." });

        talkData.Add(6, new string[] { "���� ���� �峲�� ���̰� ������ ���� ������ ��ȩ �� �޸� ����� ������ �ܾ簣���� ���� ���� ���Դ� ����� ����ߴ�." });
        talkData.Add(7, new string[] { "������ �峲�� �״�� �ƹ������� ���ߴ�." });
        talkData.Add(8, new string[] { "������ �ƹ����� �̸� ���� �ʰ� �ٸ� �Ƶ鿡�Ե� ���ø� ���ߴ�." });
        talkData.Add(9, new string[] { "�׷��� �̵� ���� ���̰� ����� ������ ������ ���� �Դ� ���� ����ߴ�." });
        talkData.Add(10, new string[] { "�ƹ����� �Ƶ���� ��ü�� � ���̸� �����Ѵٸ鼭 �׵��� ���ѾҴ�" });

        talkData.Add(11, new string[] { "���� �峲�� �ٸ� ���� �尡�� �� ������ �ٷȴ�." });
        talkData.Add(12, new string[] { "������ �귯 �������� ������ �峲�� �ٽ� ���� ���ư� ����� �ߴ�." });
        talkData.Add(13, new string[] { "������ ���̰� ������ ����� ������ �ɷ� �߰����� ���� �������� �ʾҴ�." });
        talkData.Add(14, new string[] { "���� ���� �������� ������ �̹� �������� ������� �ҹ��� �����.." });
        talkData.Add(15, new string[] { "�Ƴ��� ������ ����Ͽ� ��� ȣ������ ������ ����� �����ߴ�." });
    }

    void ShowText()
    {
        // ���� Ÿ���� �ڷ�ƾ�� �ִٸ� ����
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        isComplete = false;

        Cursor.SetActive(false);

        // ���� ��ȭ ������ Ȯ��
        string textToShow = talkData[countNum][0];
        Text.text = "";

        // Ÿ���� ȿ��
        for (int i = 0; i < textToShow.Length; i++)
        {
            Text.text += textToShow[i];
            if (!char.IsWhiteSpace(textToShow[i])) // ���� ���ڰ� ������ �ƴ� ���� �Ҹ� ���
            {
                typingSound.Play();
            }
            yield return new WaitForSeconds(typingSpeed);

            // �����̽��� ������ Ÿ���� ȿ�� �ߴ��ϰ� ��ü ���� ���
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Text.text = textToShow;
                break;
            }
        }

        isTyping = false;
        isComplete = true;

        Cursor.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Ÿ���� �߿� �����̽��� ������ ��ü ���� ���
                Text.text = talkData[countNum][0];
                StopCoroutine(typingCoroutine);
                isTyping = false;
                isComplete = true;

                Cursor.SetActive(true);
            }
            else if (isComplete && talkSet.activeSelf)
            {
                // ������ ������ ��µ� ���¿��� �����̽��� ������ ���� ��ȭ�� �̵�
                countNum++;
                if (talkData.ContainsKey(countNum))
                {
                    print(countNum);
                    if(countNum == 6)
                    {
                        talkSet.SetActive(false);
                        move = 1;
                        Invoke("nextMove", 5);
                        Invoke("nextMove", 10);
                        Invoke("nextMove", 13);
                        return;
                    }
                    else if (countNum == 11)
                    {
                        talkSet.SetActive(false);
                        move = 5;
                        Invoke("nextMove", 5);
                        return;
                    }

                    ShowText();
                }
                else
                {
                    Debug.Log("��ȭ ����");
                    talkSet.SetActive(false); // ��� ��ȭ�� ������ �� talkSet�� ��Ȱ��ȭ
                    Invoke("nextMove", 3);
                }
            }
        }


        if (move == 1)
        {
            Move1player();
            Move1fox();
        }
        if (move == 2)
        {
            Move2fox();
        }
        if(move == 3)
        {
            Move2player();
            Movehouse();
        }
        if (move == 4)
        {
            player.SetActive(false);
            fox2.SetActive(false);
            talkSet.SetActive(true);
            ShowText();
            move = 0;
        }
        if (move == 5)
        {
            player.SetActive(true);
            Move3player();
            move = 6;
        }
        if(move == 7)
        {
            talkSet.SetActive(true);
            ShowText();
            move = 8;
        }
        if (move == 9)
        {
            SceneManager.LoadScene("Stage2");
        }
    }

    void nextMove()
    {
        move++;
    }


    void Move1player()
    {
        // Player
        Vector3 moveDirection = Vector3.left;
        if (player.transform.position.x > 7.0f)
        {
            player.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

    }

    void Move1fox()
    {
        // Fox
        Vector3 rightDirection = Vector3.right;

        if (fox1.transform.position.x < -0.5f)
        {
            fox1.transform.Translate(rightDirection * foxSpeed * Time.deltaTime);
        }
    }

    void Move2fox()
    {
        fox1.SetActive(false);
        fox2.SetActive(true);

        foxSpeed = 1;

        Vector3 leftDirection = Vector3.left;

        if (fox2.transform.position.x > -5f)
        {
            fox2.transform.Translate(leftDirection * foxSpeed * Time.deltaTime);
        }
        else
        {
            foxSpeed = 0;
        }
    }

    void Movehouse()
    {
        StartCoroutine(ShakeHouse());
    }

    IEnumerator ShakeHouse()
    {
        float duration = 2.0f; // ��鸮�� �ð�
        float elapsed = 0.0f;
        float magnitude = 0.1f; // ��鸲�� ����

        Vector3 originalPosition = house.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            house.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            yield return null;
        }

        house.transform.position = originalPosition;
    }

    void Move2player()
    {
        if (!isJumping)
        {
            moveSpeed = 0;
            isJumping = true;
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }


    void Move3player()
    {
        if (!isBounding)
        {
            player.transform.position = new Vector2(-5.5f, -0.5f);
            sprite.flipX = true;
            isBounding = true;
            rigid.AddForce(new Vector2(3,5).normalized * 10, ForceMode2D.Impulse);
            Invoke("StartRollPlayer", 2);
        }
    }

    void StartRollPlayer()
    {
        StartCoroutine(RollPlayer());
    }

    IEnumerator RollPlayer()
    {
        float rollSpeed = 2f; // �������� �ӵ�
        float rotationSpeed = 360f; // ȸ�� �ӵ� (��/��)

        while (player.transform.position.x < 10)
        {
            rigid.velocity = new Vector2(rollSpeed, rigid.velocity.y);
            player.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // z�� ȸ��
            yield return null;
        }

        // �������� ȿ���� ������ �ӵ� �ʱ�ȭ
        rigid.velocity = Vector2.zero;
    }

}
