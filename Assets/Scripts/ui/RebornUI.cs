using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebornUI : UIBase
{
    public Button reborn_btn_;

    // Start is called before the first frame update
    void Start()
    {
        //NetManager.AddMsgListener((short)MsgRespPbType.REBORN, OnReborn);
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

    //public void OnReborn(MsgBase msg)
    //{
    //    MsgReborn.Response resp_msg = (MsgReborn.Response)msg;
    //    Int64 global_id = resp_msg.resp.GlobalId;
    //    Int32 cur_hp = resp_msg.resp.CurHp;
    //    if (global_id == MainPlayer.GetGlobalID())
    //    {
    //        EntitySimpleInfo entity = MainPlayer.GetEntity();
    //        entity.cur_hp_ = cur_hp;
    //        SkillManager.Instance.UpdateHpUI(entity.skin_, cur_hp, entity.max_hp_);

    //        UIManager.Instance.CloseUI("Reborn");
    //        MainPlayer.SetDead(false);

    //        Animator ani = entity.skin_.transform.GetComponent<Animator>();
    //        if (ani)
    //        {
    //            ani.SetInteger("status", (int)EntityStatus.IDLE);
    //        }
    //    }
    //    else
    //    {
    //        EntitySimpleInfo entity = SceneMgr.FindEntity(global_id);
    //        if(entity != null)
    //        {
    //            entity.cur_hp_ = cur_hp;
    //            SkillManager.Instance.UpdateHpUI(entity.skin_, cur_hp, entity.max_hp_);
    //        }
    //    }

    //}
}
