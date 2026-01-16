using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlActor : BaseActor
{

    private DirectionType cur_direction_ = DirectionType.UP;
    private List<KeyCode> pressed_keys_ = new();

    private Dictionary<KeyCode, DirectionType> key2direction = new Dictionary<KeyCode, DirectionType> {
        {KeyCode.W, DirectionType.UP},
        {KeyCode.S, DirectionType.DOWN},
        {KeyCode.A, DirectionType.LEFT},
        {KeyCode.D, DirectionType.RIGHT},
    };


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
        // 1. 更新按键状态：检测所有相关键的按下和抬起事件
        UpdateKeyStates();

        // 2. 计算新的方向（基于最后按下的键）
        DirectionType new_direction = CalculateCurrentDirection();

        // 3. 如果方向发生变化，则发送相应的指令
        if (new_direction != cur_direction_)
        {
            if (new_direction == DirectionType.START)
            {
                MsgNewMove msg = new();
                msg.SetSendData((Int32)cur_direction_, true);
                NetManager.Send(msg);
            }
            else
            {
                MsgNewMove msg = new();
                msg.SetSendData((Int32)new_direction, false);
                NetManager.Send(msg);
            }
            cur_direction_ = new_direction; // 更新当前方向
        }
    }

    void UpdateKeyStates()
    {
        // 检查每个定义好的键
        foreach (var key in key2direction.Keys)
        {
            // 按键按下：如果该键被按下且不在列表中，则添加到列表末尾
            if (Input.GetKeyDown(key))
            {
                if (!pressed_keys_.Contains(key))
                {
                    pressed_keys_.Add(key);
                }
            }
            // 按键抬起：如果该键被抬起且在列表中，则从列表中移除
            if (Input.GetKeyUp(key))
            {
                if (pressed_keys_.Contains(key))
                {
                    pressed_keys_.Remove(key);
                }
            }
        }
    }

    DirectionType CalculateCurrentDirection()
    {
        if (pressed_keys_.Count > 0)
        {
            // 获取列表最后一个元素（最后按下的键）
            KeyCode last_pressed_key = pressed_keys_[pressed_keys_.Count - 1];
            return key2direction[last_pressed_key];
        }
        return DirectionType.START; // 没有键按下，返回None
    }

    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target);
    }
}
