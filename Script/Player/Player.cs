using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class Player : SingletonMonoBehavior<Player>
{
    public List<GameObject> playerCarryItemList;
    Rigidbody rb;
    Animator animator;
    private float moveSpeed;
    private bool isIdle;
    private bool isWalking;
    private bool isRunning;
    private bool inputDisable;
    public bool InputDisable
    {
        get
        {
            return inputDisable;
        }
        set
        {
            inputDisable = value;
        }
    }
    private float horizontal;
    private float vertical;
    private Vector3 moveDirection;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        EventHolder.onPlantCropEvent += DisablePlayerInput;
        EventHolder.onWaterCropEvent += DisablePlayerInput;
        EventHolder.onHarvestCropEvent += DisablePlayerInput;
        EventHolder.onDestoryCropEvent += DisablePlayerInput;
    }
    void OnDisable()
    {
        EventHolder.onPlantCropEvent -= DisablePlayerInput;
        EventHolder.onWaterCropEvent -= DisablePlayerInput;
        EventHolder.onHarvestCropEvent -= DisablePlayerInput;
        EventHolder.onDestoryCropEvent -= DisablePlayerInput;
    }
    private void Update()
    {
        if (!inputDisable)
        {
            PlayerRunningInput();
            PlayerWalkingInput();
        }
        SetAnimation();
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    private void PlayerRunningInput()
    {
        // 获取输入
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        // 计算移动方向（相对于世界坐标）
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        if (moveDirection.magnitude > 0)
        {
            moveSpeed = Setting.playerRunningSpeed;
            isRunning = true;
            isWalking = false;
            isIdle = false;
        }
        else
        {
            isIdle = true;
            isWalking = false;
            isRunning = false;
        }
    }
    private void PlayerWalkingInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
            moveSpeed = Setting.playerWalkingSpeed;
        }
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            moveSpeed = Setting.playerRunningSpeed;
        }
    }
    private void SetAnimation()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            animator.SetFloat("speed", moveDirection.magnitude);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
    }
    private void PlayerMove()
    {
        // 如果有移动输入
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(-moveDirection.x, -moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Setting.rotateSpeed * Time.fixedDeltaTime);

            // 使用目标方向计算移动
            Vector3 targetForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 moveVector = targetForward * moveSpeed * Time.fixedDeltaTime;
            moveVector.y = rb.velocity.y * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveVector);
        }
    }

    private void DisablePlayerInput()
    {
        StartCoroutine(DisableInput());
    }
    private void DisablePlayerInput(int v1)
    {
        StartCoroutine(DisableInput());
    }
    private void DisablePlayerInput(int v1, int v2)
    {
        StartCoroutine(DisableInput());
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
    public void ShowPlayerCarryItem(ItemType itemType)
    {
        ClearAllCarryItem();
        Debug.Log((int)itemType);
        playerCarryItemList[(int)itemType].SetActive(true);
    }
    public void ClearAllCarryItem()
    {
        foreach (GameObject tool in playerCarryItemList)
        {
            tool.SetActive(false);
        }
    }

    IEnumerator DisableInput()
    {
        inputDisable = true;
        yield return Setting.playerPlantCropSecond;
        inputDisable = false;
    }
}