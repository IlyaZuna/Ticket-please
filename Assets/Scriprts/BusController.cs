using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // �������� ��������
    public float turnSpeed = 30f;   // �������� ��������

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftWheelModel;
    public Transform frontRightWheelModel;
    public Transform rearLeftWheelModel;
    public Transform rearRightWheelModel;

    private Rigidbody rb;

    public float maxSteerAngle = 30f;  // ������������ ���� �������� �����
    public bool isDriver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // �������� Rigidbody ��������
    }

    void Update()
    {
        // ���� ��� �������� ������ � �����
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S ��� �������
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D ��� �������

        // ������������ ���� ��������
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle); // ������������ ���� �������� �����

        // ������� ������� ������/�����
        rearLeftWheel.motorTorque = move * 200f;
        rearRightWheel.motorTorque = move * 200f;

        // ������� �����
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;

        // ��������� ������ �����
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
    }

    // ��������� ������� � �������� �����
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
        isDriver = true; // �������� ����������
        enabled = true; // �������� ���� ������
    }

    public void DisableDriving()
    {
        isDriver = false; // ��������� ����������
        enabled = false; // ��������� ���� ������
    }
}
