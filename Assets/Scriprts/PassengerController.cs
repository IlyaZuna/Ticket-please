using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public float moveSpeed = 2.0f; // �������� �������� ���������
    private bool canMove = false; // ��������� �� ���������
    private Transform currentTarget; // ������� ����� ����������
    private Queue<Transform> busPointsQueue; // ������� � ��������
    private List<Transform> seatPointsList; // ������ ���� ��� �������
    private PassengerManager manager; // �������� ����������
    private bool waitingAtBusPoint = false; // ���� �������� � ��������
    private bool isSeated = false; // ����, ��� �������� ����� �����

    private Transform busPointIndex2; // ����� � �������� 2 � ������ Bus points

    public void SetQueueTarget(Transform queuePoint, Queue<Transform> busPoints, List<Transform> seatPoints, PassengerManager passengerManager)
    {
        currentTarget = queuePoint;
        busPointsQueue = busPoints;
        seatPointsList = seatPoints;
        manager = passengerManager;

        // �������� ����� ��� �������� 2 (���� ��� ����)
        int index = 2;
        if (busPointsQueue.Count > index)
        {
            busPointIndex2 = GetPointAtIndex(busPointsQueue, index);
        }
    }

    public void AllowToMove()
    {
        canMove = true; // ��������� ��������
    }

    public void DenyMovement()
    {
        canMove = false; // ��������� ��������
    }

    private void Update()
    {
        if (!canMove || currentTarget == null || isSeated)
            return;

        MoveToTarget();

        // ����� �������� ��������� ������� �����
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            Debug.Log("s"+currentTarget);
            // ���� ������� ���� � ����� � �������� 2
            if (currentTarget == busPointIndex2)
            {
                waitingAtBusPoint = true;
                Debug.Log("�������� ����������� �� ����� � �������� 2.");

                // ���� ������� Enter
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    waitingAtBusPoint = false;
                    MoveToNextBusPoint(); // ������� � ��������� �����
                }
                return;
            }

            // ���� ������� ����� �� �������� ��������� � ������ �������
            if (busPointsQueue.Contains(currentTarget))
            {
                manager.FreeBusPoint(currentTarget); // ����������� �����
                MoveToNextBusPoint(); // ������� � ��������� �����
            }
            else
            {
                manager.FreeQueuePoint(currentTarget); // ����������� ����� � ����� �������
                MoveToNextBusPoint();
            }
        }
    }

    private void MoveToTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void MoveToNextBusPoint()
    {
        if (busPointsQueue.Count > 0)
        {
            currentTarget = busPointsQueue.Dequeue(); // ����� ��������� ����� � ������� � ��������
        }
        else
        {
            ProceedToSeat(); // ���� ������ ��� �����, ���� � �����
        }
    }

    private void ProceedToSeat()
    {
        if (seatPointsList.Count > 0)
        {
            int randomSeatIndex = Random.Range(0, seatPointsList.Count);
            currentTarget = seatPointsList[randomSeatIndex];
            seatPointsList.RemoveAt(randomSeatIndex); // ������� ����� �� ���������
        }
        else
        {
            Debug.Log("��� ��������� ���� ��� ���������.");
            isSeated = true; // �������� �����, ���� ��� ����
        }
    }

    private Transform GetPointAtIndex(Queue<Transform> queue, int index)
    {
        Transform[] array = queue.ToArray();
        if (index >= 0 && index < array.Length)
        {
            return array[index];
        }
        return null;
    }
}
