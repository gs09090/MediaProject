using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMole : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        PlaySound();
        // 클릭 시 오브젝트 파괴
        Destroy(gameObject);
        // 점수 획득
        MoleScore.score += 1;

        // 목표 달성 시 스테이지 이동
        if (MoleScore.score == 100)
            MoleSpawner.nextStage();
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
