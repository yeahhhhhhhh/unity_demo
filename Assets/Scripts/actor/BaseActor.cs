using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    private GameObject skin_;
    // 转向速度
    public float steer_ = 20;
    // 移动速度
    public float speed_ = 3f;

    // Start is called before the first frame update
    public virtual void Start()
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
        skin_.SetActive(true);
    }

    public virtual bool OnUsedSkill(Int32 skill_id, Int64 skill_gid, Vector3 position, Vector3 direction)
    {
        SkillBaseInfo skill = SkillConfig.GetSkillInfo(skill_id);
        if (skill == null)
        {
            return false;
        }

        GameObject skill_obj = SkillManager.Instance.CreateSkill(skill_id, skill_gid, position, direction);
        if (skill_obj == null)
        {
            return false;
        }

        ActiveSkillActor actor = skill_obj.GetComponent<ActiveSkillActor>();
        if (actor == null)
        {
            actor = skill_obj.AddComponent<ActiveSkillActor>();
            actor.Init(skill.skill_id_, skill_gid, gameObject, position, direction);
        }
        else
        {
            actor.SyncPos(position);
        }

        return true;
    }
}
