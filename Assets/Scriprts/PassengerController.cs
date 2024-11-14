using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PassengerController : MonoBehaviour
{
    public List<Transform> targetPoints; // ������ ����� ����������
    public float moveSpeed = 2.0f; // �������� �������� ���������
    private bool canMove = false; // ����� �� ���������
    private int currentTargetIndex = 0; // ������ ������� ����� ����������
    private bool isStandingStill = false; // ����, ��� �������� ����� �� �����
    public BusStopTrigger busStopTrigger;
    public BusController busController;

    void Update()
    {
        bool busAtStop = busStopTrigger.isAtBusStop;
        bool doorsOpen = busController.areDoorsOpen;        
        TryToEnterBus(busAtStop, doorsOpen);        
        if (canMove && !isStandingStill)
        {
            MoveToNextPoint();
        }        
        if (isStandingStill && currentTargetIndex == 1 && Input.GetKeyDown(KeyCode.Return)) // ������� Enter, ������ 2 (������ �����)
        {           
            ContinueMovingToNextPoint();
        }
    }   
    public void TryToEnterBus(bool busAtStop, bool doorsOpen)
    {
       
        if (busAtStop && doorsOpen)
        {
            canMove = true; // ��������� ��������
        }
        else
        {
            canMove = false; // ��������� ��������
        }
    }   
    private void MoveToNextPoint()
    {       
        if (targetPoints.Count == 0)
        {
            return;
        }       
        Transform target = targetPoints[currentTargetIndex];       
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;        
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {           
            if (currentTargetIndex == 1)
            {
                isStandingStill = true;
                Debug.Log("�������� ������ ������ ����� (������ 1) � ���� ������� Enter.");
            }
            else
            {               
                ContinueMovingToNextPoint();
            }
        }
    }   
    private void ContinueMovingToNextPoint()
    {      
        currentTargetIndex++;        
        if (currentTargetIndex >= targetPoints.Count)
        {
            canMove = false; // �������� ������ ��������� �����
            Debug.Log("�������� ������ ��������� �����.");
        }
        else
        {           
            isStandingStill = false;
        }
    }
}