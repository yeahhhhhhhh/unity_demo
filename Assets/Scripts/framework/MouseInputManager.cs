using UnityEngine;
using UnityEngine.Events;

public class MouseInputManager : MonoBehaviour
{
    [System.Serializable]
    public class MouseClickEvent : UnityEvent<Vector3, GameObject> { }

    [Header("配置")]
    public LayerMask interactableLayers = -1;
    public float maxRayDistance = 100f;

    [Header("事件")]
    public MouseClickEvent onWorldClicked;
    public MouseClickEvent onObjectClicked;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            ProcessLeftClick();
        }

        if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            ProcessRightClick();
        }

        // 实时获取鼠标位置（用于悬停效果等）
        if (Input.GetMouseButton(0)) // 左键拖拽
        {
            Vector3 currentWorldPos = GetCurrentMouseWorldPosition();
            OnMouseDrag(currentWorldPos);
        }
    }

    private void ProcessLeftClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayers))
        {
            // 点击到物体
            onObjectClicked?.Invoke(hit.point, hit.collider.gameObject);
            Debug.Log($"点击物体: {hit.collider.name} 位置: {hit.point}");
        }
        else
        {
            // 点击空白区域
            Vector3 worldPos = GetCurrentMouseWorldPosition();
            onWorldClicked?.Invoke(worldPos, null);
            Debug.Log($"点击空白处: {worldPos}");
        }
    }

    private void ProcessRightClick()
    {
        Vector3 worldPos = GetCurrentMouseWorldPosition();
        Debug.Log($"右键点击位置: {worldPos}");
        // 添加右键功能逻辑
    }

    public Vector3 GetCurrentMouseWorldPosition(float distanceFromCamera = 10f)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceFromCamera;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDrag(Vector3 position)
    {
        // 拖拽逻辑
        // Debug.Log($"鼠标拖拽位置: {position}");
    }

    // 公开方法：获取鼠标指向的世界坐标（带地面检测）
    public bool GetMouseGroundPosition(out Vector3 groundPosition)
    {
        groundPosition = Vector3.zero;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 假设地面在"Ground"层
        int groundLayer = LayerMask.GetMask("Ground");

        if (Physics.Raycast(ray, out hit, maxRayDistance, groundLayer))
        {
            groundPosition = hit.point;
            return true;
        }

        return false;
    }
}