using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;        // �������� ������������
    public float mouseSensitivity = 2f; // ���������������� ����
    public Transform cameraRig;         // ������, � �������� ��������� ������
    public float cameraDistance = 4f;   // ���������� ������ �� ���������
    public float cameraHeight = 2f;     // ������ ������ ������������ ���������

    private CharacterController controller;

    private float yaw = 0f;    // ���� �������� ������ �� ��� Y

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // ��������� � �������� ������
        Cursor.visible = false; // ���������, ��� ������ �����
    }

    void Update()
    {
        RotateCharacter();
        MoveCharacter();
        UpdateCameraPosition();
    }

    private void RotateCharacter()
    {
        // �������� ���� � ���� ��� �������� �� ��� Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        // ��������� ���� �������� ������ (yaw)
        yaw += mouseX;

        // ������������ ��������� �� ��� Y
        transform.Rotate(Vector3.up, mouseX);
    }

    private void MoveCharacter()
    {
        // �������� ���� � ����������
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ��������� ���� � ������ ��������
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void UpdateCameraPosition()
    {
        // ��������� ������� ������ ������������ ���������
        Vector3 targetPosition = transform.position - cameraRig.forward * cameraDistance + Vector3.up * cameraHeight;

        // ������� ������
        cameraRig.position = targetPosition;

        // ������ ������ �������� �� ���������
        cameraRig.LookAt(transform.position + Vector3.up * cameraHeight);
    }
}