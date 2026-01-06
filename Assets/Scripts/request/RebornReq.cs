using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RebornReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener((short)MsgRespPbType.REBORN, OnReborn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickReborn()
    {
        if (MainPlayer.IsDead())
        {
            MsgReborn msg = new();
            msg.SetSendData(1);
            NetManager.Send(msg);
        }
    }

    public void OnReborn(MsgBase msg)
    {
        MsgReborn.Response resp_msg = (MsgReborn.Response)msg;
        Int32 cur_hp = resp_msg.resp.CurHp;
        EntitySimpleInfo entity = MainPlayer.GetEntity();
        entity.cur_hp_ = cur_hp;
        SkillManager.Instance.UpdateHpUI(entity.skin_, cur_hp, entity.max_hp_);

        GameMain.SetRebornBtnActive(false);
        MainPlayer.SetDead(false);
    }
}
