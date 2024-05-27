using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class BossGameManager : MonoBehaviour
{
    //private int totalKey;
    public int totalKey;

    public int keyNumber;

    public int stageIndex;
    public int health = 40;
    public BossPlayerMove player;
    public Boss boss;

    public GameObject[] Stages;

    public Image healthBarImage; // UI 이미지 컴포넌트
    public Sprite[] healthBars;
    public Image UIfox;
    public Sprite[] foxs;
    public Image[] UIhealth;
    public Image[] UIItem;
    public Text UIPoint;
    public GameObject UIRestartBtn;
    private BossPlayerMove playerMove;
    public GameObject playerObject;

    public GameObject fire;
    public GameObject spike;
    public GameObject water;

    SpriteRenderer spriteRenderer;

    int endingCount = 3;
    public static int ending;

    bool Door = false;
    public GameObject door;
    public AudioSource bgmSource;

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerMove = playerObject.GetComponent<BossPlayerMove>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayBGM();
    }

    void PlayBGM()
    {
        bgmSource.loop = true; // BGM을 반복 재생
        bgmSource.Play();
    }

    void Update()
    {
        if (Door)
        {
            door.SetActive(true);
        }
    }

    public void NextStage()
    {
        if (playerMove.bossDie)
            SceneManager.LoadScene("Ending");
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            player.OnDie();
            UIRestartBtn.SetActive(true);
        }
    }

    public void HealthUp()
    {
        if (health < 3)
        {
            health++;
            UIhealth[health - 1].color = new Color(1, 1, 1, 1);
        }

    }

    public void ItemUse(int a)
    {
        UIItem[a - 1].color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

        if (a == 1)
        {
            InvokeRepeating("Fire", 0.1f, 0.2f);
            Invoke("StopFire", 2);
            Ending(1);

        }       

        if(a == 2)
        {
            InvokeRepeating("Spike", 0.1f, 0.2f);
            Invoke("StopSpike", 2);
            Ending(2);
        }

        if (a == 3)
        {
            InvokeRepeating("Water", 0.1f, 0.2f);
            Invoke("StopWater", 2);
            Ending(3);
        }
    }

    public void Ending(int num)
    {
        endingCount--;
        if (endingCount == 0)
            ending = num;
    }

    public void nextStage()
    {
        if(ending == 1)
            SceneManager.LoadScene("Stage6"); //불
        else if(ending == 2)
            SceneManager.LoadScene("Stage7"); //가시
        else
            SceneManager.LoadScene("Stage8"); //물
    }

    public void FoxHealth()
    {
        healthBarImage.color = new Color(0, 0, 0, 0);
        UIfox.color = new Color(0, 0, 0, 0);
        Door = true;
    }

    public void BossHealth()
    {

        if (boss.health < 1)
        {
            healthBarImage.sprite = healthBars[20];
            Invoke("FoxHealth", 2);
        }
        else if (boss.health < 2)
            healthBarImage.sprite = healthBars[19];
        else if (boss.health < 4)
            healthBarImage.sprite = healthBars[18];
        else if (boss.health < 6)
            healthBarImage.sprite = healthBars[17];
        else if (boss.health < 8)
            healthBarImage.sprite = healthBars[16];
        else if (boss.health < 10)
            healthBarImage.sprite = healthBars[15];
        else if (boss.health < 12)
            healthBarImage.sprite = healthBars[14];
        else if (boss.health < 14)
            healthBarImage.sprite = healthBars[13];
        else if (boss.health < 16)
            healthBarImage.sprite = healthBars[12];
        else if (boss.health < 18)
            healthBarImage.sprite = healthBars[11];
        else if (boss.health < 20)
            healthBarImage.sprite = healthBars[10];
        else if (boss.health < 22)
        {
            healthBarImage.sprite = healthBars[9];
            if (boss.health == 20)
                UIfox.sprite = foxs[1];
        }
        else if (boss.health < 24)
            healthBarImage.sprite = healthBars[8];
        else if (boss.health < 26)
            healthBarImage.sprite = healthBars[7];
        else if (boss.health < 28)
            healthBarImage.sprite = healthBars[6];
        else if (boss.health < 30)
            healthBarImage.sprite = healthBars[5];
        else if (boss.health < 32)
            healthBarImage.sprite = healthBars[4];
        else if (boss.health < 34)
            healthBarImage.sprite = healthBars[3];
        else if (boss.health < 36)
            healthBarImage.sprite = healthBars[2];
        else if (boss.health < 38)
            healthBarImage.sprite = healthBars[1];
        else if (boss.health <= 40)
            healthBarImage.sprite = healthBars[0];
    }

    public void Fire()
    {
        fire.SetActive(true);
        fire.transform.localScale *= 1.5f;
        //fire.boxCollider.transform.localScale = transform.localScale; 
        boss.OnDamaged(1);
        //보스할때온데미지드손봐야함. 보스 레벨 알려주는 유아이와 죽는 소리 플러스하기
    }

    public void StopFire()
    {
        CancelInvoke("Fire");
        fire.SetActive(false);
    }

    public void Spike()
    {
        spike.SetActive(true);
        float randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
        float randomY = UnityEngine.Random.Range(-1.0f, 1.0f);
        spike.transform.position = new Vector3(randomX, randomY, 0);
        boss.OnDamaged(1);
    }

    public void StopSpike()
    {
        CancelInvoke("Spike");
        spike.SetActive(false);
    }

    public void Water()
    {
        water.SetActive(true);
        water.transform.position += new Vector3(0f, -1f, 0f);
        if (water.transform.position == new Vector3(0f, 1.5f, 0f) || water.transform.position == new Vector3(0f, 0.5f, 0f))
            boss.OnDamaged(5);
    }

    public void StopWater()
    {
        CancelInvoke("Water");
        water.SetActive(false);
    }


    public void BossRetry()
    {
        SceneManager.LoadScene("Stage5");
    }
}

