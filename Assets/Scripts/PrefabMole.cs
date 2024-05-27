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
        // Ŭ�� �� ������Ʈ �ı�
        Destroy(gameObject);
        // ���� ȹ��
        MoleScore.score += 1;

        // ��ǥ �޼� �� �������� �̵�
        if (MoleScore.score == 100)
            MoleSpawner.nextStage();
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
