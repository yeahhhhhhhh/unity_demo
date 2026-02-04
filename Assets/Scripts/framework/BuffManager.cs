using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    private static BuffManager instance;
    public static BuffManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuffManager();
            }
            return instance;
        }
    }

    // 私有构造函数防止外部使用new创建实例
    private BuffManager()
    {
        // 初始化
    }

    public void Init()
    {
        Debug.Log("SkillManager init");
        // 注册位置同步
        NetManager.AddMsgListener((short)MsgRespPbType.BUFF_UPDATE, OnBuffUpdate);
    }

    public void OnBuffUpdate(MsgBase msg)
    {
        MsgResponseBuff resp_msg = (MsgResponseBuff)msg;
        Debug.Log("buff_id:" + resp_msg.resp.BuffId + " is add:" + resp_msg.resp.IsAdd);
    }
}
