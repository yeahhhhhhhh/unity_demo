using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlActor : BaseActor
{
    public float update_pos_interval = 0.1f;
    public float update_pos_last_time = 0;
    public float update_min_dis_ = 0.2f;
    public Vector3 old_pos_ = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
        transform.position += s;

        float dis = Mathf.Abs(transform.position.x - old_pos_.x) + Mathf.Abs(transform.position.y - old_pos_.y) + Mathf.Abs(transform.position.z - old_pos_.z);

        // 同步位置
        float cur_time = Time.time;
        if (Time.time - update_pos_last_time > update_pos_interval && dis > update_min_dis_)
        {
            update_pos_last_time = cur_time;
            MsgMove msg = new();
            msg.SetSendData(transform.position);
            NetManager.Send(msg);
            old_pos_ = transform.position;
        }
    }

    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target);
    }
}
