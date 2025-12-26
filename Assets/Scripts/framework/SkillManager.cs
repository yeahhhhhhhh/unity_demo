using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillDef
{
    Skill1 = 1,
    Bullet = 2,
    Skill3 = 3,
    Skill4 = 4,
    Skill5 = 5,
    Skill6 = 6,
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
            return gid2skill_obj[skill_gid];
        }
        SkillBaseInfo skill = SkillConfig.GetSkillInfo(skill_id);
        if (skill == null)
        {
            return null;
        }
        GameObject skill_obj = GameObject.Instantiate(skill.skill_prefab_, position, Quaternion.Euler(direction));
        if (skill_obj != null)
        {
            skill_obj.transform.localScale = new Vector3(skill.range_x_, skill.range_y_, skill.range_z_);
            skill_obj.name = skill.skill_name_;
            gid2skill_obj[skill_gid] = skill_obj;
            Debug.Log("CreateSkill当前技能对象数量：" + gid2skill_obj.Count);
        }

        return skill_obj;
    }

    public void DestroySkill(Int64 skill_gid)
    {
        if (gid2skill_obj.ContainsKey(skill_gid))
        {
            GameObject obj = gid2skill_obj[skill_gid];
            if (!obj.IsDestroyed())
            {
                GameObject.Destroy(obj);
            }
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
        //if (IsExistSkill(global_skill_id))
        //{
        //    Debug.Log("skill obj is already exist, gid:" + global_skill_id);
        //    return;
        //}

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
        Int64 uid = resp_msg.resp.Uid;

        List<attributes.combat.FightResult> result_list = resp_msg.resp.Results;
        Debug.Log("OnUseSkill, uid:" + uid);
        for (int i = 0; i < result_list.Count; ++i)
        {
            attributes.combat.FightResult one_res = result_list[i];
            Int64 target_uid = one_res.Uid;
            Int32 damage = one_res.Damage;
            Int32 cur_hp = one_res.CurHp;
            Debug.Log("OnUseSkill, target_uid:" + target_uid);
            PlayerInfo player = SceneManager.FindPlayer(target_uid);
            if (player != null)
            {
                if (damage > 0)
                {
                    CreateBlood(player.skin_);
                }
                CreateHurtText(player.skin_, damage);
                player.fight_info_.cur_hp_ = cur_hp;
                UpdateHpUI(player.skin_, cur_hp, player.fight_info_.max_hp_);
            }
            else
            {
                Debug.Log("Player not fount, target_uid:" + target_uid);
            }
        }

        List<attributes.combat.FightResult> npc_result_list = resp_msg.resp.NpcResults;
        for (int i = 0; i < npc_result_list.Count; ++i)
        {
            attributes.combat.FightResult one_res = npc_result_list[i];
            Int64 target_npc_gid = one_res.Uid;
            Int32 damage = one_res.Damage;
            Int32 cur_hp = one_res.CurHp;
            Debug.Log("OnUseSkill, target_npc_gid:" + target_npc_gid);
            NpcInfo npc = SceneManager.FindNpc(target_npc_gid);
            if (npc != null)
            {
                if (damage > 0)
                {
                    CreateBlood(npc.skin_);
                }
                CreateHurtText(npc.skin_, damage);
                npc.cur_hp_ = cur_hp;
                UpdateHpUI(npc.skin_, cur_hp, npc.max_hp_);
            }
            else
            {
                Debug.Log("npc not found, target_npc_gid:" + target_npc_gid);
            }
        }
    }

    public void UpdateHpUI(GameObject player_obj, Int32 cur_hp, Int32 max_hp)
    {
        Debug.Log("UpdateHpUI cur hp:" + cur_hp.ToString() + " max hp:" + max_hp.ToString());
        HUDManager hud_manager = player_obj.transform.GetComponentInChildren<HUDManager>();
        if (hud_manager == null)
        {
            Debug.LogError("hud_manager is null");
            return;
        }

        hud_manager.UpdateHealth(cur_hp, max_hp);
    }

    public void CreateBlood(GameObject player_obj)
    {
        // TODO: 击中特效
        GameObject blood_prefab = ResManager.LoadPrefab("BloodPrefab");
        if (blood_prefab)
        {
            float random_float = UnityEngine.Random.Range(-0.5f, 0.5f);
            Vector3 position = player_obj.transform.position + Vector3.up * 2 + Vector3.left * random_float;
            Vector3 direction = Vector3.zero;
            GameObject skill_obj = GameObject.Instantiate(blood_prefab, position, Quaternion.Euler(direction));
            skill_obj.name = "Blood";
        }
        else
        {
            Debug.Log("blood_prefab not fount");
        }
    }

    public void CreateHurtText(GameObject player_obj, Int32 damage)
    {
        DamageTextController damage_text = player_obj.transform.GetComponentInChildren<DamageTextController>();
        damage_text.CreateDamageText(damage);
    }

    public void HandleSkills()
    {

    }
}
