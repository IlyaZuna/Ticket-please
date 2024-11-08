using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // ���������������� ����
    public Transform playerBody;           // ������ �� ������� (������)

    private float xRotation = 0f;          // ������ ������� �� ��� X (����� � ����)
    private float yRotation = 180f;        // ������������� ��������� ������� �� ��� Y �� 180 ��������

    void Start()
    {
        // ��������� ������ � ������ ������
        Cursor.lockState = CursorLockMode.Locked;

        // ������������� ��������� ������� ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Update()
    {
        // �������� �������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ����������� ��� Y ��� ����������� �������� ������ ����� � ����
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);  // ����������� ���� �������� ������ ����� � ����

        // ������� �� ��� Y (����� � ������)
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, 120f, 260f);  // ����������� ���� �������� ������ ����� � ������

        // ��������� �������� � ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
