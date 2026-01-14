using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    [Header("移动速度设置")]
    public float dragSpeed = 40f;      // 右键拖拽移动速度
    public float scrollSpeed = 15f;    // 滚轮移动速度

    private Vector3 _dragOrigin;       // 记录拖拽起始点

    void Update()
    {
        // 1. 鼠标右键拖拽 - 水平移动 (XZ平面)
        HandleRightClickDrag();

        // 2. 鼠标滚轮 - 垂直移动 (Y轴)
        HandleMouseScroll();
    }

    void HandleRightClickDrag()
    {
        // 右键按下时，记录初始位置
        if (Input.GetMouseButtonDown(1)) // 1代表鼠标右键
        {
            _dragOrigin = Input.mousePosition;
            return;
        }

        // 按住右键拖拽时，计算移动
        if (!Input.GetMouseButton(1)) return;

        // 计算鼠标偏移量，转换为世界位移
        Vector3 currentPos = Input.mousePosition;
        Vector3 difference = Camera.main.ScreenToViewportPoint(_dragOrigin - currentPos);

        // 只在XZ平面移动，Y轴保持不变
        Vector3 move = new Vector3(difference.x, 0, difference.y) * dragSpeed;

        // 应用移动（使用世界坐标系）
        transform.Translate(move, Space.World);

        // 更新拖拽起点，实现连续拖拽
        _dragOrigin = currentPos;
    }

    void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        // 滚轮控制Y轴移动（上下）
        Vector3 verticalMove = new Vector3(0, -scroll * scrollSpeed, 0);
        transform.Translate(verticalMove, Space.World);
    }
}