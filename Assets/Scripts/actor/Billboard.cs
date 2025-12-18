using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // 缓存主摄像机以提高性能
        mainCamera = Camera.main;
    }

    void LateUpdate() // 在LateUpdate中执行，确保在摄像机移动后更新朝向
    {
        if (mainCamera != null)
        {
            // 让血条的面板始终朝向摄像机
            transform.rotation = mainCamera.transform.rotation;
            // 或者使用下面这行代码，可以实现更精确的“看向”效果
            // transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}