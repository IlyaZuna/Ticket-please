using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusInteraction : MonoBehaviour
{
    public GameObject player;       // ��������, �������� ���������� � �������
    public GameObject bus;          // �������, ������� ����� ��������������
    public GameObject busCamera;    // ������ ��������
    public GameObject playerCamera; // ������ ���������

    private bool isNearBus = false;
    private bool isInBus = false;
    public Transform childObject;
    public Transform parentObject;

    private void Start()
    {
        // ��������, ��� ��� ������� ���������� ��������� � ��� ������ ���������
        bus.GetComponent<BusController>().enabled = false;
        busCamera.SetActive(false);

        // �������� ��������� � ��� ������
        player.SetActive(true);
        playerCamera.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� � ���� �������� ����� ��������
        if (other.gameObject == player)
        {
            isNearBus = true;
            Debug.Log("Press F to enter the bus.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ����� �������� ������� �� ���� ��������
        if (other.gameObject == player)
        {
            isNearBus = false;
        }
    }

    private void Update()
    {
        // ���� ����� � ��������� � ������ F, � �������� �� � ��������
        if (isNearBus && Input.GetKeyDown(KeyCode.F) && !isInBus)
        {
            EnterBus();
        }
        // ���� �������� ��� � �������� � ������ F, ����� �����
        else if (isInBus && Input.GetKeyDown(KeyCode.F))
        {
            ExitBus();
        }
    }

    private void EnterBus()
    {
        // ��������� ��������� � ��� ������
        childObject.SetParent(parentObject);
        player.SetActive(false);
        playerCamera.SetActive(false);

        // �������� ������� � ������ ��������
        bus.GetComponent<BusController>().enabled = true;
        busCamera.SetActive(true);

        isInBus = true;
        Debug.Log("Press F to exit the bus.");
    }

    private void ExitBus()
    {
        // �������� ��������� � ���      
        childObject.SetParent(null);
        player.SetActive(true);
        playerCamera.SetActive(true);

        // ��������� ������� � ������ ��������
        bus.GetComponent<BusController>().enabled = false;
        busCamera.SetActive(false);

        isInBus = false;
        Debug.Log("Press F to enter the bus.");
    }

    
}
