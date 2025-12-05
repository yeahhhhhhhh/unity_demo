using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class CtrlActor : BaseActor
{
    public float update_pos_interval = 0.1f;
    public float update_pos_last_time = 0;
    public float update_min_dis_ = 0.2f;
    public float update_min_angles_ = 90f;
    public Vector3 old_pos_ = new Vector3(0, 0, 0);
    public Int32 direction_ = (Int32)DirectionType.UP;
    public bool is_change_dir_ = false;
    public bool is_moving_ = false;
    public bool is_change_moving_ = false;

    public float old_x_ = 0;
    public float old_y_ = 0;


    private DirectionType cur_direction_ = DirectionType.UP;
    private List<KeyCode> pressed_keys_ = new List<KeyCode>();

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
        old_pos_ = transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //MoveUpdate();
        NewMoveUpdate();
    }

    public void NewMoveUpdate()
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

    public void MoveUpdate()
    {
        // TODO: 改变朝向
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // 如果两个键一起按，则不改变朝向
        if (x != 0f && y == 0f)
        {
            if (x > 0f)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                if (direction_ != (Int32)DirectionType.RIGHT)
                {
                    direction_ = (Int32)DirectionType.RIGHT;
                    is_change_dir_ = true;
                }
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
                if (direction_ != (Int32)DirectionType.LEFT)
                {
                    direction_ = (Int32)DirectionType.LEFT;
                    is_change_dir_ = true;
                }
            }
            
        }
        if (x == 0f && y != 0f)
        {
            if (y > 0f)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                if (direction_ != (Int32)DirectionType.UP)
                {
                    direction_ = (Int32)DirectionType.UP;
                    is_change_dir_ = true;
                }
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                if (direction_ != (Int32)DirectionType.DOWN)
                {
                    direction_ = (Int32)DirectionType.DOWN;
                    is_change_dir_ = true;
                }
            }
            
        }

        if (x != 0 && y != 0)
        {
            if (x != old_x_)
            {
                if (x > 0f)
                {
                    transform.eulerAngles = new Vector3(0, 90, 0);
                    if (direction_ != (Int32)DirectionType.RIGHT)
                    {
                        direction_ = (Int32)DirectionType.RIGHT;
                        is_change_dir_ = true;
                    }
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, -90, 0);
                    if (direction_ != (Int32)DirectionType.LEFT)
                    {
                        direction_ = (Int32)DirectionType.LEFT;
                        is_change_dir_ = true;
                    }
                }
            }
            if (y != old_y_)
            {
                if (y > 0f)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    if (direction_ != (Int32)DirectionType.UP)
                    {
                        direction_ = (Int32)DirectionType.UP;
                        is_change_dir_ = true;
                    }
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    if (direction_ != (Int32)DirectionType.DOWN)
                    {
                        direction_ = (Int32)DirectionType.DOWN;
                        is_change_dir_ = true;
                    }
                }
            }
        }
        old_x_ = x;
        old_y_ = y;
        //transform.Rotate(0, x * steer_ * Time.deltaTime, 0);

        // 前后左右
        //Vector3 s = y * transform.forward * speed_ * Time.deltaTime + x * transform.right * speed_ * Time.deltaTime;
        if (x != 0 || y != 0)
        {
            transform.position += transform.forward * speed_ * Time.deltaTime;
        }

        float dis = Mathf.Abs(transform.position.x - old_pos_.x) + Mathf.Abs(transform.position.y - old_pos_.y) + Mathf.Abs(transform.position.z - old_pos_.z);

        // 同步位置
        float cur_time = Time.time;
        if ((Time.time - update_pos_last_time > update_pos_interval) && (dis > update_min_dis_ || is_change_dir_))
        {
            update_pos_last_time = cur_time;
            MsgMove msg = new();
            msg.SetSendData(transform.position, direction_);
            NetManager.Send(msg);
            old_pos_ = transform.position;
            is_change_dir_ = false;
        }
    }

    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target);
    }
}
