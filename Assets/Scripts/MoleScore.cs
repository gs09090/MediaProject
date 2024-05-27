using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoleScore : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static int score;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "Score : " + score.ToString();
    }
}
