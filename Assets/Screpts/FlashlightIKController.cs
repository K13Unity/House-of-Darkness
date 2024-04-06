using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightIKController : MonoBehaviour
{
    public PlayerController player;
    public Transform rightHandTarget; // ��������� �� ���� ��� ����� ����
    public Transform flashlight; // ��������� �� �������

    void LateUpdate()
    {
        // ������������ ������� ��� ��� ����� ���� � ��������� �� �������� ��������
        SetIKTargetPosition(flashlight.rotation.eulerAngles);
    }

    void SetIKTargetPosition(Vector3 targetRotation)
    {
        // ������������ ������� ��� ��� ����� ����
        rightHandTarget.rotation = Quaternion.Euler(targetRotation);

        flashlight.rotation = Quaternion.Euler(112f, 41f, 170f);

        // ����������� ����� ����� ���������� ������� ��� �������� � ����
        // flashlight.position = rightHandTarget.position;
    }
}
