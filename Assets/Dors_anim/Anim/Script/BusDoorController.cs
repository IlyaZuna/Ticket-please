using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorController : MonoBehaviour {
    // ������ �� ��������� Animator
    private Animator animator;

    // ��� �������� ��� �������� ������
    private const string OpenDoorsTrigger = "OpenDoors";

    void Start() {
        // �������� ��������� Animator �� �������
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator �� ������! �������� ��������� Animator � �������.");
        }
    }

    void Update() {
        // ��������� ������� ������� K
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (animator != null)
            {
                // ���������� ������� ��� ��������
                animator.SetTrigger(OpenDoorsTrigger);
            }
        }
    }
}
