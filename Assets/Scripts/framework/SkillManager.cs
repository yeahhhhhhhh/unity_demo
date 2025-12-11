using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillDef
{
    Skill1 = 1,
    Bullet = 2,
}

public class SkillManager
{
    public Dictionary<Int64, GameObject> gid2skill_obj = new();

    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillManager();
            }
            return instance;
        }
    }

    // 私有构造函数防止外部使用new创建实例
    private SkillManager()
    {
        // 初始化
    }

    public GameObject CreateSkill(Int32 skill_id, Int64 skill_gid, Vector3 position, Vector3 direction)
    {
        if (gid2skill_obj.ContainsKey(skill_gid))
        {
            Debug.Log("重复创建skill, gid:" + skill_gid);
            return null;
        }
        SkillBaseInfo skill = SkillConfig.GetSkillInfo(skill_id);
        if (skill == null)
        {
            return null;
        }
        GameObject skill_obj = GameObject.Instantiate(skill.skill_prefab_, position, Quaternion.Euler(direction));
        skill_obj.transform.localScale = new Vector3(skill.range_x_, skill.range_y_, skill.range_z_);
        skill_obj.name = skill.skill_name_;

        gid2skill_obj[skill_gid] = skill_obj;
        Debug.Log("CreateSkill当前技能对象数量：" + gid2skill_obj.Count);

        return skill_obj;
    }

    public void DestroySkill(Int64 skill_gid)
    {
        if (gid2skill_obj.ContainsKey(skill_gid))
        {
            GameObject obj = gid2skill_obj[skill_gid];
            GameObject.Destroy(obj);
            gid2skill_obj.Remove(skill_gid);
        }

        Debug.Log("DestroySkill当前技能对象数量：" + gid2skill_obj.Count);
    }

    public bool IsExistSkill(Int64 skill_gid)
    {
        return gid2skill_obj.ContainsKey(skill_gid);
    }

    public void Init()
    {
        Debug.Log("SkillManager init");
        // 注册位置同步
        NetManager.AddMsgListener((short)MsgRespPbType.USE_SKILL_RESPONSE, OnUseSkill);
        NetManager.AddMsgListener((short)MsgRespPbType.SKILL_RESPONSE_POS, OnUpdateSkillPos);
    }

    public void OnUpdateSkillPos(MsgBase msg)
    {
        MsgUseSkill.ResponsePos resp_msg = (MsgUseSkill.ResponsePos)msg;
        Int64 uid = resp_msg.resp.Uid;
        Int64 global_skill_id = resp_msg.resp.GlobalSkillId;
        if (IsExistSkill(global_skill_id))
        {
            Debug.Log("skill obj is already exist, gid:" + global_skill_id);
            return;
        }

        bool is_main_player = MainPlayer.GetUid() == uid;
        PlayerInfo player = SceneManager.FindPlayer(uid);
        if (player != null)
        {
            Int32 skill_id = resp_msg.resp.SkillId;
            Vector3 position = new()
            {
                x = resp_msg.resp.Pos.X,
                y = resp_msg.resp.Pos.Y,
                z = resp_msg.resp.Pos.Z,
            };
            Vector3 direction = MoveManager.GetRotaionByDirection(resp_msg.resp.Pos.Direction);
            if (is_main_player)
            {
                MainPlayerActor player_actor = player.skin_.GetComponent<MainPlayerActor>();
                player_actor.OnUsedSkill(skill_id, global_skill_id, position, direction);
            }
            else
            {
                SyncPlayerActor player_actor = player.skin_.GetComponent<SyncPlayerActor>();
                player_actor.OnUsedSkill(skill_id, global_skill_id, position, direction);
            }
        }
    }

    public void OnUseSkill(MsgBase msg)
    {
        MsgUseSkill.Response resp_msg = (MsgUseSkill.Response)msg;

    }

    public void HandleSkills()
    {

    }
}
