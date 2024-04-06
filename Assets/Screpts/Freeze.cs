using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    public float rayLength = 10f; // Встановіть відстань райкасту
    public LayerMask enemyLayer; // Виберіть шар для взаємодії з ворогами

    private void Update()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out hitInfo, rayLength);

        if (hasHit)
        {
            EnemyController enemy = hitInfo.collider.GetComponent<EnemyController>(); 
            if (enemy != null)
            {
                enemy.Freeze();
            }
            EnemyBoss boss = hitInfo.collider.GetComponent<EnemyBoss>();
            if (boss != null)
            {
                boss.Freeze();
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayLength);
    }
}
