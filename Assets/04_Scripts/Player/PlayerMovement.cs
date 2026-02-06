using UnityEngine;

/// <summary>
/// 俯视角2D角色移动脚本（WASD控制）
/// 依赖Rigidbody2D和Collider2D，挂载在玩家对象上
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] // 强制要求挂载Rigidbody2D，缺少会自动添加
public class PlayerMovement2D : MonoBehaviour
{
    [Header("移动基础参数")]
    [Tooltip("移动速度，默认500")]
    public float moveSpeed = 1500f;

    private Rigidbody2D rb;
    private Vector2 moveInput; // 存储WASD的输入向量

    void Awake()
    {
        // 获取自身的Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();
        // 初始化刚体关键参数
        InitRigidbodySetting();
    }

    void Update()
    {
        // 每一帧获取玩家的键盘输入
        GetPlayerInput();
    }

    void FixedUpdate()
    {
        // FixedUpdate是物理帧，专门处理刚体相关操作（移动、加力等）
        if (rb != null)
        {
            MovePlayer();
        }
    }

    /// <summary>
    /// 初始化刚体参数（适配俯视角，和之前的设置保持一致）
    /// </summary>
    private void InitRigidbodySetting()
    {
        rb.gravityScale = 0f; // 强制无重力，俯视角必备
        rb.freezeRotation = true; // 强制冻结旋转，避免角色乱转
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete; // 基础碰撞检测
    }

    /// <summary>
    /// 获取WASD的输入值，转换为2D向量
    /// </summary>
    private void GetPlayerInput()
    {
        // GetAxisRaw返回-1/0/1，无平滑过渡，俯视角操作更跟手；GetAxis有平滑过渡，偏休闲
        float inputX = Input.GetAxisRaw("Horizontal"); // 水平轴：A=-1，D=1
        float inputY = Input.GetAxisRaw("Vertical"); // 垂直轴：S=-1，W=1

        moveInput = new Vector2(inputX, inputY);
        // 归一化：防止斜向移动时速度翻倍（斜向向量模长>1，归一化后保持1，各方向速度一致）
        moveInput = moveInput.normalized;
    }

    /// <summary>
    /// 给刚体加力，实现玩家移动（物理驱动）
    /// </summary>
    private void MovePlayer()
    {
        // AddForce：给刚体添加力，ForceMode2D.Force是持续力，配合Linear Drag实现“按键移动，松键减速”
        rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}