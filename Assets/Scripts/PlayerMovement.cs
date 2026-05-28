using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 6f;        // 移动速度
    public float jumpForce = 8f;        // 跳跃力度
    public float gravity = -9.81f;      // 重力加速度

    private CharacterController controller;
    private Vector3 velocity;           // 用于存储垂直方向的速度（重力与跳跃）
    private bool isGrounded;            // 是否在地面上

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. 地面检测：CharacterController自带了一个非常好用的isGrounded属性
        isGrounded = controller.isGrounded;

        // 如果在地面上，且垂直速度向下，将其重置为接近0（防止一直向下累积速度）
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 一个很小的负数，保证角色紧贴地面
        }

        // 2. 获取移动输入 (WASD)
        float x = Input.GetAxis("Horizontal"); // A/D 或 左右箭头
        float z = Input.GetAxis("Vertical");   // W/S 或 上下箭头

        // 3. 计算移动方向（基于角色自身的朝向）
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 4. 跳跃逻辑
        if (Input.GetButtonDown("Jump") && isGrounded) // 默认“Jump”对应空格键
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // 根据跳跃高度反推初始速度
        }

        // 5. 应用重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}