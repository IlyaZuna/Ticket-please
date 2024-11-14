using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PassengerController : MonoBehaviour
{
    public List<Transform> targetPoints; // Список точек назначения
    public float moveSpeed = 2.0f; // Скорость движения пассажира
    private bool canMove = false; // Можно ли двигаться
    private int currentTargetIndex = 0; // Индекс текущей точки назначения
    private bool isStandingStill = false; // Флаг, что пассажир стоит на месте
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
        if (isStandingStill && currentTargetIndex == 1 && Input.GetKeyDown(KeyCode.Return)) // Клавиша Enter, индекс 2 (вторая точка)
        {           
            ContinueMovingToNextPoint();
        }
    }   
    public void TryToEnterBus(bool busAtStop, bool doorsOpen)
    {
       
        if (busAtStop && doorsOpen)
        {
            canMove = true; // Разрешаем движение
        }
        else
        {
            canMove = false; // Запрещаем движение
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
                Debug.Log("Пассажир достиг второй точки (индекс 1) и ждет нажатия Enter.");
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
            canMove = false; // Пассажир достиг последней точки
            Debug.Log("Пассажир достиг последней точки.");
        }
        else
        {           
            isStandingStill = false;
        }
    }
}