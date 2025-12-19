using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class MainPlayerActor : CtrlActor
{
    public Transform body_;
    public Transform mouth_;
    public Transform fire_point_;

    public GameObject bullet_prefab_;
    public int max_bullet_count_ = 5;
    public int cur_bullet_count_ = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        Debug.Log("MainPlayerActor start");
        base.Start();
        body_ = transform.Find("Body");
        mouth_ = transform.Find("Mouth"); 
        fire_point_ = transform.Find("FirePoint");
        bullet_prefab_ = ResManager.LoadPrefab("BulletPrefab");
        if (bullet_prefab_ == null)
        {
            Debug.Log("bullet_prefab_ is null");
        }
        if (body_ == null)
        {
            Debug.Log("body is null");
        }
        if (mouth_ == null)
        {
            Debug.Log("mouth is null");
        }
        if (fire_point_ == null)
        {
            Debug.Log("fire_point is null");
        }
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Fire();

    }

    public void Fire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cur_bullet_count_ >= max_bullet_count_)
            {
                Debug.Log("已发射最大数量bullet:" + max_bullet_count_.ToString());
                return;
            }
            Debug.Log("fire!!!!");
            SendSkillMsg(SkillDef.Bullet);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendSkillMsg(SkillDef.Skill1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SendSkillMsg(SkillDef.Skill3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SendSkillMsg(SkillDef.Skill4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SendSkillMsg(SkillDef.Skill5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SendSkillMsg(SkillDef.Skill6);
        }
    }

    public void SendSkillMsg(SkillDef skill_id)
    {
        Debug.Log("use skill:" + skill_id.ToString());
        // 使用技能请求
        MsgUseSkill msg = new();
        msg.SetSendData((Int32)skill_id);
        NetManager.Send(msg);
    }

    public override bool OnUsedSkill(Int32 skill_id, Int64 skill_gid, Vector3 position, Vector3 direction)
    {
        return base.OnUsedSkill(skill_id, skill_gid, position, direction);
    }

    public void NotifiedBulletCreate()
    {
        cur_bullet_count_++;

    }
    public void NotifiedBulletDestroy()
    {
        cur_bullet_count_--;
    }
}
