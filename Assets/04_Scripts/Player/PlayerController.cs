using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // 强制要求挂载Rigidbody2D，缺少会自动添加
public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 15f;        // 移动速度
    public float acceleration = 50f;    // 加速度
    public float deceleration = 50f;    // 减速度

    [Header("组件引用")]
    private Rigidbody2D rb;
    private List<SpriteRenderer> playerRenderers = new List<SpriteRenderer>();// 用于翻转朝向

    private Vector2 currentVelocity;    // 当前实际速度向量
    private Vector2 inputDirection;     // 玩家输入的方向
    private bool isFacingLeft = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponentsInChildren<SpriteRenderer>(true, playerRenderers);

        //  保险物理设置
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector2(horizontal, vertical).normalized;
        if (horizontal != 0)
        {
            ChangeFacing();
        }
    }

    void FixedUpdate()
    {
        if (inputDirection != Vector2.zero)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, inputDirection * moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            if (currentVelocity.magnitude < 0.01f) currentVelocity = Vector2.zero;
        }

        rb.velocity = currentVelocity;
    }

    void ChangeFacing()
    {
        bool flip = inputDirection.x < 0;
        if (isFacingLeft != flip)
        {
            foreach (SpriteRenderer renderer in playerRenderers)
            {
                renderer.flipX = flip;
            }
            isFacingLeft = flip;
        }
    }
}