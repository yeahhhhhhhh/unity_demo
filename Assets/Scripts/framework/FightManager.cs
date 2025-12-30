using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FightManager
{
    private static FightManager instance;
    public static FightManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FightManager();
            }
            return instance;
        }
    }

    // 私有构造函数防止外部使用new创建实例
    private FightManager()
    {
        // 初始化
    }

    public void Init()
    {
        Debug.Log("FightManager init");
        // 注册位置同步
        NetManager.AddMsgListener((short)MsgRespPbType.GET_FIGHT_INFO, OnGetFightInfo);
    }

    public void OnGetFightInfo(MsgBase msg)
    {
        MsgGetFightInfo.Response resp_msg = (MsgGetFightInfo.Response)msg;
        Int64 uid = resp_msg.resp.Uid;
        attributes.combat.FightInfo fight_info = resp_msg.resp.FightInfo;

        if (uid == MainPlayer.GetUid())
        {
            MainPlayer.SetPlayerFightInfo(fight_info);
        }

        // TODO: 更新显示
        Debug.Log("OnGetFightInfo hp:" + fight_info.CurHp);
    }
}
