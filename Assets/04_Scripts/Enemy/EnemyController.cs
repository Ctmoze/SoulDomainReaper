using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 3f;
    public float stopDistance = 1f;  // 距离玩家多远时停止
    public float acceleration = 20f;      // 加速度

    [Header("组件引用")]
    private Rigidbody2D rb;
    private Transform playerTransform;
    private List<SpriteRenderer> allRenderers = new List<SpriteRenderer>();

    private Vector2 currentVelocity;
    private bool isFacingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponentsInChildren<SpriteRenderer>(true, allRenderers);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        //保险
        rb.gravityScale = 0;                  // 关闭重力
        rb.freezeRotation = true;             // 冻结旋转，防止翻滚
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 防止高速穿墙
    }

    void FixedUpdate()
    {
        if (playerTransform != null) return;
        Vector2 direction = (Vector2)playerTransform.position - (Vector2)transform.position;
        float distance = direction.magnitude;
        if (distance > stopDistance)
        {
            direction.Normalize();
            currentVelocity = Vector2.Lerp(currentVelocity, direction * moveSpeed, acceleration * Time.fixedDeltaTime);
            rb.velocity = currentVelocity;
            HandleFacing(direction);
        }
        else
        {
            rb.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;

            // AttackPlayer(); 
        }
    }
    void HandleFacing(Vector2 direction)
    {
        if (direction.x == 0) return;
        bool targetFlip = direction.x < 0;
        if (targetFlip != isFacingLeft)
        {
            isFacingLeft = targetFlip;
            foreach (SpriteRenderer sr in allRenderers)
            {
                if (sr != null)
                {
                    sr.flipX = isFacingLeft;
                }
            }
        }
    }
}
