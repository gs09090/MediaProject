using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float moveSpeed = 2f; // 카메라 이동 속도
    float stopPosition = 81.3f; // 멈추는 위치

    void Update()
    {
        // 카메라를 오른쪽으로 이동시킴
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 카메라의 X 위치에 따라 이동 속도 변경
        if (transform.position.x >= stopPosition)
        {
            moveSpeed = 2.5f;
        }

        // 카메라의 X 위치가 멈추는 위치에 도달하면 이동을 멈춤
        if (transform.position.x >= stopPosition)
        {
            transform.position = new Vector3(stopPosition, transform.position.y, transform.position.z);
            // 이동 멈춤
            enabled = false;
        }
    }
}
