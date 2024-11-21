using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Скорость движения пассажира
    private bool canMove = false; // Разрешено ли двигаться
    private Transform currentTarget; // Текущая точка назначения
    private Queue<Transform> busPointsQueue; // Очередь к водителю
    private List<Transform> seatPointsList; // Список мест для посадки
    private PassengerManager manager; // Менеджер пассажиров
    private bool waitingAtBusPoint = false; // Флаг ожидания у водителя
    private bool isSeated = false; // Флаг, что пассажир занял место

    private Transform busPointIndex2; // Точка с индексом 2 в списке Bus points

    public void SetQueueTarget(Transform queuePoint, Queue<Transform> busPoints, List<Transform> seatPoints, PassengerManager passengerManager)
    {
        currentTarget = queuePoint;
        busPointsQueue = busPoints;
        seatPointsList = seatPoints;
        manager = passengerManager;

        // Получаем точку под индексом 2 (если она есть)
        int index = 2;
        if (busPointsQueue.Count > index)
        {
            busPointIndex2 = GetPointAtIndex(busPointsQueue, index);
        }
    }

    public void AllowToMove()
    {
        canMove = true; // Разрешаем движение
    }

    public void DenyMovement()
    {
        canMove = false; // Запрещаем движение
    }

    private void Update()
    {
        if (!canMove || currentTarget == null || isSeated)
            return;

        MoveToTarget();

        // Когда пассажир достигнет текущей точки
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            Debug.Log("s"+currentTarget);
            // Если текущая цель — точка с индексом 2
            if (currentTarget == busPointIndex2)
            {
                waitingAtBusPoint = true;
                Debug.Log("Пассажир остановился на точке с индексом 2.");

                // Ждем нажатия Enter
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    waitingAtBusPoint = false;
                    MoveToNextBusPoint(); // Переход к следующей точке
                }
                return;
            }

            // Если текущая точка не является последней в списке очереди
            if (busPointsQueue.Contains(currentTarget))
            {
                manager.FreeBusPoint(currentTarget); // Освобождаем точку
                MoveToNextBusPoint(); // Переход к следующей точке
            }
            else
            {
                manager.FreeQueuePoint(currentTarget); // Освобождаем точку в общей очереди
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
            currentTarget = busPointsQueue.Dequeue(); // Берем следующую точку в очереди к водителю
        }
        else
        {
            ProceedToSeat(); // Если больше нет точек, идем к месту
        }
    }

    private void ProceedToSeat()
    {
        if (seatPointsList.Count > 0)
        {
            int randomSeatIndex = Random.Range(0, seatPointsList.Count);
            currentTarget = seatPointsList[randomSeatIndex];
            seatPointsList.RemoveAt(randomSeatIndex); // Убираем место из доступных
        }
        else
        {
            Debug.Log("Нет доступных мест для пассажира.");
            isSeated = true; // Пассажир сядет, если нет мест
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
