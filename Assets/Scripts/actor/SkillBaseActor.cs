using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseActor : MonoBehaviour
{
    public SkillDynamicInfo skill_info_ = new();

    public virtual void Init(Int32 skill_id, Int64 skill_gid, GameObject owner)
    {
        SkillBaseInfo skill = SkillConfig.GetSkillInfo(skill_id);
        skill_info_.skill_gid_ = skill_gid;
        skill_info_.owner_ = owner;
        skill_info_.CopyBaseInfo(skill);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        skill_info_.cur_life_time_ += Time.deltaTime;
        if (skill_info_.cur_life_time_ >= skill_info_.base_info_.max_life_time_)
        {
            Debug.Log("Destroy skill:" + skill_info_.base_info_.skill_id_);
            SkillManager.Instance.DestroySkill(skill_info_.skill_gid_);
            Destroy(gameObject);
        }
    }
}
