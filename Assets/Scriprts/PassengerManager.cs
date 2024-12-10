using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    public Transform spawnPoint; // Точка появления пассажиров
    public List<Transform> queuePoints; // Очередь к автобусу
    public List<Transform> busPoints; // Очередь к водителю
    public List<Transform> seatPoints; // Точки для посадки в автобусе
    public GameObject passengerPrefab; // Префаб пассажира
    public BusController busController; // Ссылка на контроллер автобуса
    public BusStopTrigger busStopTrigger; // Ссылка на триггер остановки
    public float spawnInterval = 2f; // Интервал появления пассажиров
    public int maxQueueSize = 10; // Максимальная длина очереди

    private Queue<Transform> availableQueuePoints = new Queue<Transform>(); // Доступные точки в очереди к автобусу
    private Queue<Transform> availableBusPoints = new Queue<Transform>(); // Доступные точки в очереди к водителю
    private List<Transform> availableSeatPoints = new List<Transform>(); // Доступные места в автобусе
    private List<GameObject> passengers = new List<GameObject>(); // Список всех пассажиров

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

        // Периодический спавн пассажиров
        InvokeRepeating(nameof(SpawnPassenger), spawnInterval, spawnInterval);
    }

    private void Update()
    {
        // Проверяем условия: двери открыты и автобус на остановке
        if (busController.areDoorsOpen && busStopTrigger.isAtBusStop)
        {
            // Разрешаем движение всем пассажирам
            foreach (var passenger in passengers)
            {
                var passengerController = passenger.GetComponent<PassengerController>();
                if (passengerController != null)
                {
                    passengerController.AllowToMove(); // Вызываем метод для разрешения движения
                }
            }
        }
        else
        {
            // Условия не выполнены — запрещаем движение
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
            Debug.Log("Максимальное количество пассажиров достигнуто или очередь заполнена.");
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

