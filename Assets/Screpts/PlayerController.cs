using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public CharacterController controller;
    public AlliController ally;
    public float walkSpeed = 2.0f; // Швидкість ходьби
    public float runSpeed = 1f; // Швидкість бігу
    public float sneakSpeed = 0.1f;
    public string deathTrigger = "isTripping"; // Додайте тригер смерті
    public bool isDead = false;
    public Transform flashlight;
    public LayerMask enemyLayer; // Шар, на якому розміщені вороги
    public bool isRunning = false;
    private bool isSneaking = false;
    private float rotationSpeed = 8f;
    internal bool isWalking;
    public Vector3 startPos;
    public Vector3 idlePos;
    public Quaternion startRotation;
    public Quaternion idleRotation;
    
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        Debug.DrawRay(flashlight.position, flashlight.forward * 10f, Color.green);

        // Встановлюємо анімацію руху і бігу в стан "вимкнено" за замовчуванням
        _animator.SetBool("isRun", false);
        _animator.SetBool("isMove", false);
        _animator.SetBool("isSneak", false);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        var moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        float moveSneakSpeed = Input.GetKey(KeyCode.RightControl) ? sneakSpeed : walkSpeed;

        if (moveDirection != Vector3.zero)
        {
            _animator.SetBool("isMove", true);

            isRunning = moveSpeed == runSpeed;
            _animator.SetBool("isRun", isRunning);

            isSneaking = moveSneakSpeed == sneakSpeed;
            _animator.SetBool("isSneak", isSneaking);

            var targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            moveDirection = moveDirection.normalized * moveSpeed;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    public void DoDeath()
    {
        StartCoroutine(DeathAnimationDelay(0.2f));
        isDead = true;
        controller.enabled = false;
    }

    IEnumerator DeathAnimationDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetTrigger("isTripping");
    }
    private void Awake()
    {
        startPos = flashlight.transform.localPosition;
        startRotation = flashlight.transform.localRotation;
        idlePos = new Vector3(-0.0007f, 0.102f, -0.008f);
        idleRotation = Quaternion.Euler(new Vector3(112, 41, 170));
        
    }
}
