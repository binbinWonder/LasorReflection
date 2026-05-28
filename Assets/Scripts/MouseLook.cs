using UnityEngine;

public class MouseLook : MonoBehaviour
{   
    private float mouseX, mouseY; 
    [Header("视角参数")]
    public float mouseSensitivity = 100f; // 鼠标灵敏度
    public Transform playerBody;          // 玩家身体的Transform（用于左右旋转）

    private float xRotation = 0f;         // 记录垂直方向的旋转角度

    void Start()
    {
        // 游戏开始时锁定并隐藏鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 获取鼠标移动的增量
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
       
        // 1. 垂直旋转（上下看）：控制摄像机本身的 X 轴旋转
        xRotation -= mouseY;
        // 限制上下看的角度，防止脖子折断（限制在 -70度 到 70度之间）
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // 2. 水平旋转（左右看）：控制玩家身体（胶囊体）的 Y 轴旋转
        playerBody.Rotate(Vector3.up * mouseX);
        
    }
}