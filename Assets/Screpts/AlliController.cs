using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlliController : MonoBehaviour
{
    public PlayerController player;
    public float followDistance = 5.0f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 8f;
    public float running = 4.0f;
    public float currentSpeed;
    public Animator animator;
    public Transform throwPoint; // Визначте цю точку, де напарник кине камінь.
    public GameObject stonePrefab; // Попередньо створений префаб каменя.
    public GameObject cubeObject;
    public Transform cubeTransform;

    // private bool isThrowing = false;
    public string throwingTrigger = "isThrowing";
    private bool isRunning = false;
    private bool isIdle = false;
    private bool isWalking = false;

   
    
    void Start()
    {
        animator = GetComponent<Animator>();
        SetIdle();
    }

    private void SetIdle()
    {
        StopRunning();
        StopWalking();
        currentSpeed = 0;
        animator.SetBool("isIdle", true);
        isIdle = true;
    }

    void Idle()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > followDistance)
        {
            float playerSpeed = player.controller.velocity.magnitude;
            if (playerSpeed > 2.8f)
            {
                SetRunning();
                return;
            }
            if (playerSpeed > 0.1f && playerSpeed < 2.8f)
            {
                SetWalking();
                return;
            }

        }
    }

    void SetRunning()
    {

        StopWalking();
        animator.SetBool("isRunning", true);
        currentSpeed = running;
        isRunning = true;
    }

    private void Running()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= followDistance)
        {
            SetIdle();
            return;
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.localRotation = rotation;
        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
    }

    void SetWalking()
    {
        StopIdle();
        StopRunning();
        animator.SetBool("isWalking", true);
        currentSpeed = moveSpeed;
        isWalking = true;
    }

    private void Walking()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= followDistance)
        {
            SetIdle();
            return;
        }
        if (distanceToPlayer > followDistance)
        {
            float playerSpeed = player.controller.velocity.magnitude;
            if (playerSpeed > 2.8f)
            {
                SetRunning();
                return;
            }
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.localRotation = rotation;
        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
    }
    private void StopRunning()
    {
        animator.SetBool("isRunning", false);
        isRunning = false;
    }
    private void StopWalking()
    {
        animator.SetBool("isWalking", false);
        isWalking = false;
    }
    private void StopIdle()
    {
        animator.SetBool("isIdle", false);
        isIdle = false;
    }

    void Update()
    {
        if (isIdle == true)
        {
            Idle();
        }
        if (isRunning == true)
        {
            Running();
        }
        if (isWalking == true)
        {
            Walking();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!animator.GetBool("isThrowing"))
            {
                StartThrowAnimation();
            }
        }
    }


    public void StartThrowAnimation()
    {
        animator.SetTrigger("isThrowing");
        StartCoroutine(ThrowStoneWithDelay());
    }

    private IEnumerator ThrowStoneWithDelay()
    {
        yield return new WaitForSeconds(1.5f);

        Vector3 targetDirection;
        GameObject stone = CreateStone(out targetDirection);
        SetCharacterRotation(targetDirection);
        ThrowStone(stone, targetDirection);
        EnableGravity(stone);
        SetStoneVelocity(stone, targetDirection);
        SetIdle();
    }
    private void SetCharacterRotation(Vector3 targetDirection) // повертає персонажа в напрямку кидка каменя
    {
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = lookRotation;
    }

    private GameObject CreateStone(out Vector3 targetDirection) // створює камінь і визначає напрямок кидка (ціль кидка)
    {
        Vector3 targetPosition = cubeTransform.position;
        targetDirection = (targetPosition - throwPoint.position).normalized;
        return Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
    }

    private void ThrowStone(GameObject stone, Vector3 targetDirection) //обчислює і додає силу до каменя для кидка у напрямку цілі.
    {
        float throwForce = 70.0f;
        stone.GetComponent<Rigidbody>().AddForce(targetDirection * throwForce, ForceMode.Impulse);
    }

    private void EnableGravity(GameObject stone) //включає гравітацію для каменя
    {
        stone.GetComponent<Rigidbody>().useGravity = true;
    }

    private void SetStoneVelocity(GameObject stone, Vector3 targetDirection) //встановлює початкову швидкість для каменя
    {
        float throwSpeed = 5.0f;
        stone.GetComponent<Rigidbody>().velocity = targetDirection * throwSpeed;
    }

}