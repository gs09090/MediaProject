using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour
{
    public int columnPoolSize = 5;
    public GameObject columnPrefab;
    public float spawnRate = 4f;
    public float columnMin = -1f;
    public float columnMax = 3.5f;

    private GameObject[] colums;
    private Vector2 objectPoolPosition = new Vector2(-15f, 0f);
    private float timeSinceLastSpawned;
    private float spawnXPosition = 10f;
    private int currentColumn = 0;

    public AudioSource bgmSource;
    public AudioSource effectSource;

    void Start()
    {
        colums = new GameObject[columnPoolSize];
        for (int i = 0; i < columnPoolSize; i++)
        {
            colums[i] = (GameObject)Instantiate(columnPrefab, objectPoolPosition, Quaternion.identity);
        }
        PlayBGM();
    }
    void PlayBGM()
    {
        bgmSource.loop = true; // BGM을 반복 재생
        bgmSource.Play();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            effectSource.Play();

        timeSinceLastSpawned += Time.deltaTime;

        if (GameControl.instance.gameOver == false && timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0;
            float spawnYPosition = Random.Range(columnMin, columnMax);
            colums[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);
            currentColumn++;
            if (currentColumn >= columnPoolSize)
            {
                currentColumn = 0;
            }
        }
    }
}
