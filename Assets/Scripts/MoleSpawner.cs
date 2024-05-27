using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MoleSpawner : MonoBehaviour
{
    public GameObject mole;
    public Transform[] spawnPoints;

    int rand;

    //public int point;

    public TextMeshProUGUI UIPoint;
    public GameObject UIRestartBtn;

    public AudioSource bgmSource;

    void Awake()
    {
        SpawnMole();
    }

    void Start()
    {
        PlayBGM();
    }
    void PlayBGM()
    {
        bgmSource.loop = true; // BGM을 반복 재생
        bgmSource.Play();
    }

    void Update()
    {
        //UIPoint.text = point.ToString();
        UIPoint.text = "잡은 두더지 수 : " + MoleScore.score.ToString();
        if (MoleScore.score == 10)
            nextStage();
    }

    void SpawnMole()
    {
        rand = Random.Range(0, 9);
        
        switch (rand)
        {
            case 0:
                GameObject moleInstance0 = Instantiate(mole, spawnPoints[0].position, Quaternion.identity);
                Destroy(moleInstance0, 2f);
                break;
            case 1:
                GameObject moleInstance1 = Instantiate(mole, spawnPoints[1].position, Quaternion.identity);
                Destroy(moleInstance1, 2f);
                break;
            case 2:
                GameObject moleInstance2 = Instantiate(mole, spawnPoints[2].position, Quaternion.identity);
                Destroy(moleInstance2, 2f);
                break;
            case 3:
                GameObject moleInstance3 = Instantiate(mole, spawnPoints[3].position, Quaternion.identity);
                Destroy(moleInstance3, 2f);
                break;
            case 4:
                GameObject moleInstance4 = Instantiate(mole, spawnPoints[4].position, Quaternion.identity);
                Destroy(moleInstance4, 2f);
                break;
            case 5:
                GameObject moleInstance5 = Instantiate(mole, spawnPoints[5].position, Quaternion.identity);
                Destroy(moleInstance5, 2f);
                break;
            case 6:
                GameObject moleInstance6 = Instantiate(mole, spawnPoints[6].position, Quaternion.identity);
                Destroy(moleInstance6, 2f);
                break;
            case 7:
                GameObject moleInstance7 = Instantiate(mole, spawnPoints[7].position, Quaternion.identity);
                Destroy(moleInstance7, 2f);
                break;
            case 8:
                GameObject moleInstance8 = Instantiate(mole, spawnPoints[8].position, Quaternion.identity);
                Destroy(moleInstance8, 2f);
                break;
        }

        Invoke("SpawnMole", 4f);

    }

    public static void nextStage()
    {
        SceneManager.LoadScene("Stage2");
    }
}
