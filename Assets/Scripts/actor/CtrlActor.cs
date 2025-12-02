using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlActor : BaseActor
{
    // Start is called before the first frame update
    new void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        MoveUpdate();
    }

    public void MoveUpdate()
    {
        // TODO: 改变朝向
        float x = Input.GetAxis("Horizontal");
        //transform.Rotate(0, x * steer_ * Time.deltaTime, 0);
        // 前后左右
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed_ * Time.deltaTime + x * transform.right * speed_ * Time.deltaTime;
        transform.transform.position += s;
    }

    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target);
    }
}
