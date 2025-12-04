using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillDef
{
    Bullet = 1,
}

public class SkillManager
{
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

    public void Init()
    {
        Debug.Log("SkillManager init");
        // 注册位置同步
        NetManager.AddMsgListener((short)MsgRespPbType.USE_SKILL_RESPONSE, OnUseSkill);
    }

    public void OnUseSkill(MsgBase msg)
    {
        MsgUseSkill.Response resp_msg = (MsgUseSkill.Response)msg;
        Int64 uid = resp_msg.resp.Uid;

        // 暂不处理
        if (MainPlayer.GetUid() == uid)
        {
            return;
        }

        PlayerInfo player = SceneManager.FindPlayer(uid);
        if (player != null)
        {
            Int32 skill_id = resp_msg.resp.SkillId;
            SyncPlayerActor player_actor = player.skin_.GetComponent<SyncPlayerActor>();
            if (player_actor != null)
            {
                player_actor.Fire();
            }
            else
            {
                Debug.Log("player_actor is null");
            }
        }
    }

    public void HandleSkills()
    {

    }
}
