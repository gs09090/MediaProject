using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private bool isActive = false;

    void Start()
    {
        Invoke("FireActive", 0f);
    }

    void Update()
    {
        
    }

    void FireActive()
    {
        isActive = !isActive;

        gameObject.SetActive(isActive);

        float nextInvokeTime = Random.Range(1f, 3f);

        Invoke("FireActive", nextInvokeTime);
    }
}
