using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public EnemyController enemyController;
    public string tegEnemy = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tegEnemy))
        {
            EnemyController enemy =other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
