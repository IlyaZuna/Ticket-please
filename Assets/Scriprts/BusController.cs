using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // �������� ��������
    public float turnSpeed = 300f;   // �������� ��������
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public Transform frontLeftWheelModel;
    public Transform frontRightWheelModel;
    public Transform rearLeftWheelModel;
    public Transform rearRightWheelModel;
    public Transform exitpoint;


    private Rigidbody rb;
    public float maxSteerAngle = 30f;  // ������������ ���� �������� �����
    public bool isDriver = false;

    // ����� ���������� ��� ��������� ������ � ���������
    public bool areDoorsOpen = false;  // ��������� ������ (�������/�������)
    public bool s = false;      // ��������� �� ������� �� ���������

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // �������� Rigidbody ��������
    }

    void Update()
    {
        // ���������, ������ �� ������� "1" � ��������� �� ������� �� ���������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleDoors();  // ����� ������ ������������ ��������� ������
        }

        // �������� ��������
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S ��� �������
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D ��� �������

        // ������������ ���� ��������
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle);

        // ������� ������� ������/�����
        rearLeftWheel.motorTorque = move * 300f;
        rearRightWheel.motorTorque = move * 300f;

        // ������� �����
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;

        // ��������� ������ �����
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
    }

    // ����� ��� ������������ ��������� ������...........................................................................................................................................................................................................
    void ToggleDoors()
    {
        areDoorsOpen = !areDoorsOpen;// ������ ��������� ������
        Debug.Log("����� " + (areDoorsOpen ? "�������" : "�������"));
    }
    //.....................................................................................................................................................................................................................................
    // ��������� ������� � �������� �����
    void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelModel)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelModel.position = pos;
        wheelModel.rotation = rot;
    }

     

}