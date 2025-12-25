using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SyncActor : BaseActor
{
    public Vector3 start_pos_;
    public Vector3 last_pos_;
    public Vector3 forcast_pos_;
    public float forcast_time_;
    public Int32 direction_ = (Int32)DirectionType.UP;
    public float frame_interval_ = 0.1f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        start_pos_ = transform.position;
        last_pos_ = transform.position;
        forcast_pos_ = transform.position;
        forcast_time_ = Time.time;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ForecastUpdate();
    }

    public virtual void ForecastUpdate()
    {
        float t = (Time.time - forcast_time_) / frame_interval_;
        t = Mathf.Clamp01(t);
        transform.position = Vector3.Lerp(start_pos_, last_pos_, t);
    }

    public virtual void SyncPos(Vector3 pos, Int32 direction)
    {
        start_pos_ = transform.position;
        forcast_pos_ = pos + 2 * (pos - last_pos_);  // √ª”√µΩ
        last_pos_ = pos;
        forcast_time_ = Time.time;
        direction_ = direction;
        transform.eulerAngles = MoveManager.GetRotaionByDirection(direction);
    }


}
