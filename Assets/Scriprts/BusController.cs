using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // Скорость движения
    public float turnSpeed = 30f;   // Скорость поворота

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftWheelModel;
    public Transform frontRightWheelModel;
    public Transform rearLeftWheelModel;
    public Transform rearRightWheelModel;

    private Rigidbody rb;

    public float maxSteerAngle = 30f;  // Максимальный угол поворота колес
    public bool isDriver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Получаем Rigidbody автобуса
    }

    void Update()
    {
        // Ввод для движения вперед и назад
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S или стрелки
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D или стрелки

        // Ограничиваем угол поворота
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle); // Ограничиваем угол поворота колес

        // Двигаем автобус вперед/назад
        rearLeftWheel.motorTorque = move * 200f;
        rearRightWheel.motorTorque = move * 200f;

        // Поворот колес
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;

        // Обновляем модели колес
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
    }

    // Обновляем позиции и вращения колес
    void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelModel)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelModel.position = pos;
        wheelModel.rotation = rot;
    }
    public void EnableDriving()
    {
        isDriver = true; // Включаем управление
        enabled = true; // Включаем этот скрипт
    }

    public void DisableDriving()
    {
        isDriver = false; // Отключаем управление
        enabled = false; // Отключаем этот скрипт
    }
}
