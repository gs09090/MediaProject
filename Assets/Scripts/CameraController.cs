using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float moveSpeed = 2f; // ī�޶� �̵� �ӵ�
    float stopPosition = 81.3f; // ���ߴ� ��ġ

    void Update()
    {
        // ī�޶� ���������� �̵���Ŵ
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // ī�޶��� X ��ġ�� ���� �̵� �ӵ� ����
        if (transform.position.x >= stopPosition)
        {
            moveSpeed = 2.5f;
        }

        // ī�޶��� X ��ġ�� ���ߴ� ��ġ�� �����ϸ� �̵��� ����
        if (transform.position.x >= stopPosition)
        {
            transform.position = new Vector3(stopPosition, transform.position.y, transform.position.z);
            // �̵� ����
            enabled = false;
        }
    }
}
