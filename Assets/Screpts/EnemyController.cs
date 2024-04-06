using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    public PlayerController player; 
    public float moveSpeed = 2.0f; // Швидкість руху ворога
    public float minDistance = 2.0f; // Мінімальна відстань для активації ворога
    private bool isChasing = false;
    private float _panchDistance = 2f;
    private const string chasing = "isChasing";
    private const string panch = "PanchTrigger";
    private bool isPunching;
    private bool isWinner;
    public bool isDead = false;

    private bool isFrozen = false;
    private float freezeEndTime = 0f; // Час завершення замороження
    public float freezeDuration = 5f; // Тривалість замороження
    private const string being = "Being";


    public void Freeze()
    {
        isFrozen = true;
        _animator.SetBool(chasing, false);
        freezeEndTime = Time.time + freezeDuration;
        Debug.Log("Enemy frozen"); // Додайте цей рядок для перевірки
    }

    private void Start()
    {
        StartCoroutine(MyUpdate());
    }


    private IEnumerator MyUpdate()
    {
        
        while (true)
        {
            yield return null;

            if (isFrozen)
            {
                if (Time.time >= freezeEndTime)
                {
                    isFrozen = false; // Замороження закінчено
                }
                else
                {
                    continue;
                }
            }

            if (isWinner)
            {
                continue;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (isChasing == false)
            {
                
                if (distanceToPlayer <= minDistance)
                {
                    _animator.Play("Scream");
                    yield return new WaitForSeconds(3);
                    isChasing = true;
                }
            }

            if (isChasing)
            {
                Vector3 moveDirection = (player.transform.position - transform.position).normalized;
                _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
              //  transform.position += moveDirection * moveSpeed * Time.deltaTime;
                _animator.SetBool(chasing, isChasing);
                Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = rotation;
                if (distanceToPlayer <= _panchDistance)
                {
                    moveSpeed = 0;
                    _animator.SetBool(chasing, false);
                    isChasing = false;
                    isPunching = true;

                }
            }

            if (isPunching)
            {
                _animator.SetTrigger(panch);
               // yield return new WaitForSeconds(1);
                player.DoDeath();
                isPunching = false;
                isWinner = true;
            }
        }
    }
    public void Die()
    {
        _animator.SetTrigger(being);
        isDead = true;
        Destroy(_characterController);
        Destroy(_rigidbody);
        Destroy(_capsuleCollider);
        Destroy(this);
    }
}


