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
    public AudioSource typingSound; // 타이핑 사운드
    Dictionary<int, string[]> talkData;

    int countNum = 0; // 대화 문구의 인덱스
    float typingSpeed = 0.1f; // 타자 속도 (초 단위)
    bool isTyping = false; // 타이핑 중인지 여부
    bool isComplete = false; // 문장이 완전히 출력되었는지 여부

    Coroutine typingCoroutine; // 현재 타이핑 코루틴

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

        // 첫 대화 시작
        //ShowText();
        Invoke("TextStart", 2);

        PlayBGM();
    }
    void PlayBGM()
    {
        bgmSource.loop = true; // BGM을 반복 재생
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
        talkData.Add(0, new string[] { "옛날 옛적에 아들만 내리 셋을 둔 어느 부부가 있었다." });
        talkData.Add(1, new string[] { "부부는 딸을 갖고 싶다는 간절한 마음에 여우들이 출몰하는 여웃골 근처 절에서 치성을 드렸다." });
        talkData.Add(2, new string[] { "마침내 귀하디 귀한 막내딸을 얻게 되었다." });
        talkData.Add(3, new string[] { "막내 딸은 부모님과 오빠들의 사랑을 독차지하며 집안의 금지옥엽으로 자랐다." });
        talkData.Add(4, new string[] { "그러던 어느날 그믐만 되면 집에서 키우던 소나 말이 아무런 이유도 없이 죽어나갔다." });
        talkData.Add(5, new string[] { "아버지는 장남에게 원인을 밝혀내라 하고, 장남은 밤을 새서 외양간을 지켰다." });

        talkData.Add(6, new string[] { "밤을 새던 장남은 누이가 밖으로 나와 꼬리가 아홉 개 달린 여우로 변신해 외양간에서 소의 간을 빼먹는 장면을 목격했다." });
        talkData.Add(7, new string[] { "다음날 장남은 그대로 아버지에게 고했다." });
        talkData.Add(8, new string[] { "하지만 아버지는 이를 믿지 않고 다른 아들에게도 감시를 명했다." });
        talkData.Add(9, new string[] { "그러나 이들 역시 누이가 여우로 변신해 가축의 간을 먹는 것을 목격했다." });
        talkData.Add(10, new string[] { "아버지는 아들들이 단체로 어린 누이를 모함한다면서 그들을 내쫓았다" });

        talkData.Add(11, new string[] { "이후 장남은 다른 집에 장가를 가 가정을 꾸렸다." });
        talkData.Add(12, new string[] { "세월이 흘러 가족들이 걱정된 장남은 다시 집에 돌아가 보기로 했다." });
        talkData.Add(13, new string[] { "하지만 누이가 여우라는 사실이 마음에 걸려 발걸음이 쉬이 떨어지질 않았다." });
        talkData.Add(14, new string[] { "제일 가던 부짓잡인 본가는 이미 몰락한지 오래라는 소문을 들었다.." });
        talkData.Add(15, new string[] { "아내는 위험을 대비하여 삼색 호리병을 가지고 가라고 조언했다." });
    }

    void ShowText()
    {
        // 현재 타이핑 코루틴이 있다면 중지
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

        // 현재 대화 데이터 확인
        string textToShow = talkData[countNum][0];
        Text.text = "";

        // 타이핑 효과
        for (int i = 0; i < textToShow.Length; i++)
        {
            Text.text += textToShow[i];
            if (!char.IsWhiteSpace(textToShow[i])) // 현재 문자가 공백이 아닐 때만 소리 재생
            {
                typingSound.Play();
            }
            yield return new WaitForSeconds(typingSpeed);

            // 스페이스가 눌리면 타이핑 효과 중단하고 전체 문장 출력
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
                // 타이핑 중에 스페이스를 누르면 전체 문장 출력
                Text.text = talkData[countNum][0];
                StopCoroutine(typingCoroutine);
                isTyping = false;
                isComplete = true;

                Cursor.SetActive(true);
            }
            else if (isComplete && talkSet.activeSelf)
            {
                // 문장이 완전히 출력된 상태에서 스페이스를 누르면 다음 대화로 이동
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
                    Debug.Log("대화 종료");
                    talkSet.SetActive(false); // 모든 대화가 끝났을 때 talkSet을 비활성화
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
        float duration = 2.0f; // 흔들리는 시간
        float elapsed = 0.0f;
        float magnitude = 0.1f; // 흔들림의 강도

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
        float rollSpeed = 2f; // 굴러가는 속도
        float rotationSpeed = 360f; // 회전 속도 (도/초)

        while (player.transform.position.x < 10)
        {
            rigid.velocity = new Vector2(rollSpeed, rigid.velocity.y);
            player.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // z축 회전
            yield return null;
        }

        // 굴러가는 효과가 끝나면 속도 초기화
        rigid.velocity = Vector2.zero;
    }

}
