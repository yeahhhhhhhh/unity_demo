using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseInfo
{
    public Int32 skill_id_;
    public String skill_name_;
    public GameObject skill_prefab_;
    public float max_life_time_ = 0;
    public float radius_ = 1;
    public float range_x_ = 1;
    public float range_y_ = 1;
    public float range_z_ = 1;
}

public class SkillDynamicInfo
{
    public SkillBaseInfo base_info_ = new();
    public Int64 skill_gid_;
    public Vector3 pos_;
    public Vector3 direction_;
    public GameObject owner_;
    public float cur_life_time_ = 0;

    public void CopyBaseInfo(SkillBaseInfo base_info)
    {
        base_info_.skill_id_ = base_info.skill_id_;
        base_info_.skill_name_ = base_info.skill_name_;
        base_info_.skill_prefab_ = base_info.skill_prefab_;
        base_info_.max_life_time_ = base_info.max_life_time_;
        base_info_.radius_ = base_info.radius_;
        base_info_.range_x_ = base_info.range_x_;
        base_info_.range_z_ = base_info.range_z_;
    }
}