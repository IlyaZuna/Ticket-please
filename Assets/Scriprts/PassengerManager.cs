using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    public Transform spawnPoint; // ����� ��������� ����������
    public List<Transform> queuePoints; // ������� � ��������
    public List<Transform> busPoints; // ������� � ��������
    public List<Transform> seatPoints; // ����� ��� ������� � ��������
    public GameObject passengerPrefab; // ������ ���������
    public BusController busController; // ������ �� ���������� ��������
    public BusStopTrigger busStopTrigger; // ������ �� ������� ���������
    public float spawnInterval = 2f; // �������� ��������� ����������
    public int maxQueueSize = 10; // ������������ ����� �������

    private Queue<Transform> availableQueuePoints = new Queue<Transform>(); // ��������� ����� � ������� � ��������
    private Queue<Transform> availableBusPoints = new Queue<Transform>(); // ��������� ����� � ������� � ��������
    private List<Transform> availableSeatPoints = new List<Transform>(); // ��������� ����� � ��������
    private List<GameObject> passengers = new List<GameObject>(); // ������ ���� ����������

    private void Start()
    {
        foreach (var point in queuePoints)
        {
            availableQueuePoints.Enqueue(point);
        }
        foreach (var point in busPoints)
        {
            availableBusPoints.Enqueue(point);
        }
        availableSeatPoints.AddRange(seatPoints);

        // ������������� ����� ����������
        InvokeRepeating(nameof(SpawnPassenger), spawnInterval, spawnInterval);
    }

    private void Update()
    {
        // ��������� �������: ����� ������� � ������� �� ���������
        if (busController.areDoorsOpen && busStopTrigger.isAtBusStop)
        {
            // ��������� �������� ���� ����������
            foreach (var passenger in passengers)
            {
                var passengerController = passenger.GetComponent<PassengerController>();
                if (passengerController != null)
                {
                    passengerController.AllowToMove(); // �������� ����� ��� ���������� ��������
                }
            }
        }
        else
        {
            // ������� �� ��������� � ��������� ��������
            foreach (var passenger in passengers)
            {
                var passengerController = passenger.GetComponent<PassengerController>();
                if (passengerController != null)
                {
                    passengerController.DenyMovement(); 
                }
            }
        }
    }


    private void SpawnPassenger()
    {
        if (passengers.Count >= maxQueueSize || availableQueuePoints.Count == 0)
        {
            Debug.Log("������������ ���������� ���������� ���������� ��� ������� ���������.");
            return;
        }

        GameObject passenger = Instantiate(passengerPrefab, spawnPoint.position, Quaternion.identity);
        passengers.Add(passenger);

        PassengerController passengerController = passenger.GetComponent<PassengerController>();
        if (passengerController != null)
        {
            Transform queuePoint = availableQueuePoints.Dequeue();
            passengerController.SetQueueTarget(queuePoint, availableBusPoints, availableSeatPoints, this);
        }
    }

    public void FreeQueuePoint(Transform point)
    {
        availableQueuePoints.Enqueue(point);
    }

    public void FreeBusPoint(Transform point)
    {
        availableBusPoints.Enqueue(point);
    }

    public void FreeSeatPoint(Transform point)
    {
        availableSeatPoints.Add(point);
    }
}

