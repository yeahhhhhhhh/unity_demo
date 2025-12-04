using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SyncActor : BaseActor
{
    public Vector3 last_pos_;
    public Vector3 forcast_pos_;
    public float forcast_time_;

    public void Init()
    {
        last_pos_ = transform.position;
        forcast_pos_ = transform.position;
        forcast_time_ = Time.time;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ForecastUpdate();
    }

    public void ForecastUpdate()
    {
        float t = (Time.time - forcast_time_) / 0.1f;
        t = Mathf.Clamp(t, 0f, 1f);
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, last_pos_, t);
        transform.position = pos;
    }

    public void SyncPos(Vector3 pos)
    {
        forcast_pos_ = pos + 2 * (pos - last_pos_);
        last_pos_ = pos;
        forcast_time_ = Time.time;
    }


}
