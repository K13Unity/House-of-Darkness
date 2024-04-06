using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightIKController : MonoBehaviour
{
    public PlayerController player;
    public Transform rightHandTarget; // Посилання на ціль для правої руки
    public Transform flashlight; // Посилання на ліхтарик

    void LateUpdate()
    {
        // Встановлення позиції цілі для правої руки в залежності від орієнтації ліхтарика
        SetIKTargetPosition(flashlight.rotation.eulerAngles);
    }

    void SetIKTargetPosition(Vector3 targetRotation)
    {
        // Встановлення позиції цілі для правої руки
        rightHandTarget.rotation = Quaternion.Euler(targetRotation);

        flashlight.rotation = Quaternion.Euler(112f, 41f, 170f);

        // Опціонально можна також встановити позицію для ліхтарика в руці
        // flashlight.position = rightHandTarget.position;
    }
}
