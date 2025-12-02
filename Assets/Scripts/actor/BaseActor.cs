using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    private GameObject skin_;
    // 转向速度
    public float steer_ = 20;
    // 移动速度
    public float speed_ = 3f;

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Init(string skinPath)
    {
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin_ = (GameObject)Instantiate(skinRes);
        skin_.transform.parent = this.transform;
        skin_.transform.localPosition = Vector3.zero;
        skin_.transform.localEulerAngles = Vector3.zero;
    }
}
