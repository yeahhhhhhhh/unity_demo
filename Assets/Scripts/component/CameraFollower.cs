using Unity.VisualScripting;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target_; // 指定要跟随的目标
    public Vector3 offset_;   // 存储初始偏移
    public Vector3 origin_offset_ = new(0, 8, -7);
    public Vector3 max_offset_ = new(7, 14, -3);
    public Vector3 min_offset_ = new(-7, 4, -14);

    [Header("移动速度设置")]
    public float dragSpeed = 40f;      // 右键拖拽移动速度
    public float scrollSpeed = 15f;    // 滚轮移动速度

    private Vector3 _dragOrigin;       // 记录拖拽起始点

    public void Init(Transform target)
    {
        target_ = target;
        offset_ = origin_offset_;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        HandleRightClickDrag();
        HandleMouseScroll();

        // 每帧更新摄像机位置，保持初始偏移
        if (target_ != null && !target_.IsDestroyed())
        {
            transform.position = target_.position + offset_;
        }
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

        offset_ += move;
        offset_.x = Mathf.Max(offset_.x, min_offset_.x);
        offset_.x = Mathf.Min(offset_.x, max_offset_.x);
        offset_.z = Mathf.Max(offset_.z, min_offset_.z);
        offset_.z = Mathf.Min(offset_.z, max_offset_.z);

        // 更新拖拽起点，实现连续拖拽
        _dragOrigin = currentPos;
    }

    void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        // 滚轮控制Y轴移动（上下）
        Vector3 verticalMove = new(0, -scroll * scrollSpeed, 0);
        offset_ += verticalMove;
        offset_.y = Mathf.Max(offset_.y, min_offset_.y);
        offset_.y = Mathf.Min(offset_.y, max_offset_.y);
    } 
}
