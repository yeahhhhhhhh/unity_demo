using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollower : MonoBehaviour
{
    public Transform target_; // 指定要跟随的目标
    public Vector3 offset_ = new();   // 存储初始偏移

    public void Init(Transform target, Vector3 offset)
    {
        target_ = target;
        offset_ = offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 计算并存储摄像机与目标之间的初始偏移量
        //if (target_ != null && !target_.IsDestroyed())
        //{
        //    offset_ = transform.position - target_.position;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // 每帧更新摄像机位置，保持初始偏移
        if (target_ != null && !target_.IsDestroyed())
        {
            transform.position = target_.position + offset_;
        }
    }
}
