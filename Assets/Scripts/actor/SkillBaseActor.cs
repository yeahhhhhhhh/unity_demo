using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseActor : MonoBehaviour
{
    public SkillDynamicInfo skill_info_ = new();

    public virtual void Init(Int32 skill_id, Int64 skill_gid, GameObject owner, Vector3 position, Vector3 direction)
    {
        SkillBaseInfo skill = SkillConfig.GetSkillInfo(skill_id);
        skill_info_.skill_gid_ = skill_gid;
        skill_info_.owner_ = owner;
        skill_info_.pos_ = position;
        skill_info_.direction_ = direction;
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
        if (skill_info_.base_info_.speed_ > 0)
        {
            transform.Translate(skill_info_.base_info_.speed_ * Time.deltaTime * transform.forward, Space.World);
        }
    }
    public void SyncPos(Vector3 pos)
    {
        // 修正位置
        float error_dis = Mathf.Abs(transform.position.x - pos.x) + Mathf.Abs(transform.position.z - pos.z) + Mathf.Abs(transform.position.y - pos.y);
        Debug.Log("SyncPos 修正pos, cur pos:" + transform.position + " 修正pos:" + pos);
        if (error_dis > 1f)
        {
            //Debug.Log("SyncPos 修正pos, cur pos:" + transform.position + " 修正pos:" + pos);
            transform.position = pos;
        }
    }
}
