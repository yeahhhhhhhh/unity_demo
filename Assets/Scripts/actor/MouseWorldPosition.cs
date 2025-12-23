using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    [SerializeField] private LayerMask mouseColliderLayerMask = -1;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            // 创建从摄像机到鼠标位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 发射射线检测碰撞
            if (Physics.Raycast(ray, out hit, float.MaxValue, mouseColliderLayerMask))
            {
                Vector3 worldPosition = hit.point;
                Debug.Log("鼠标点击世界坐标: " + worldPosition);
                Debug.Log("点击的物体: " + hit.collider.gameObject.name);

                // 在这里添加你的点击处理逻辑
                OnPositionClicked(worldPosition, hit.collider.gameObject);
            }
        }
    }

    private void OnPositionClicked(Vector3 position, GameObject clickedObject)
    {
        // 示例：在点击位置生成特效或移动物体
        Debug.Log($"在位置 {position} 点击了物体 {clickedObject.name}");
        if (string.Equals(clickedObject.name, "Floor"))
        {
            if (position.x > 0f && position.z > 0f)
            {
                MsgPathFind msg = new();
                msg.SetSendData((Int32)position.x, (Int32)position.z);
                NetManager.Send(msg);

                // 生成特效
                GameObject prefab_obj = ResManager.LoadPrefab("Prefabs/Slash effects/Snow slash");
                if (prefab_obj != null)
                {
                    GameObject click_obj = GameObject.Instantiate(prefab_obj, position, Quaternion.Euler(Vector3.zero));
                    Destroy(click_obj, 1f);
                }
            }
        }
    }
}