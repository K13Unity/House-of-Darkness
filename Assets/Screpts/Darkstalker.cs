using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkstalker : MonoBehaviour
{
    public EnemyController enemyController;
    public string enemyTag = "Enemy"; 

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(enemyTag))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
