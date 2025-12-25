using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcInfo
{
    public Int32 npc_id_;
    public String name_;
    public Int64 npc_gid_;
    public Int32 max_hp_;
    public Int32 cur_hp_;

    public GameObject skin_;

    public void CopyNpcConfig(Int32 npc_id)
    {
        NpcInfo npc = NpcConfig.GetNpcInfo(npc_id);
        if (npc != null)
        {
            npc_id_ = npc.npc_id_;
            name_ = npc.name_;
            max_hp_ = npc.max_hp_;
        }
        else
        {
            Debug.LogError("no npc_id config, " + npc_id);
        }
    }

    public void CopyNpcSyncData(attributes.scene.NpcSceneInfo npc)
    {
        npc_gid_ = npc.NpcGid;
        cur_hp_ = npc.CurHp;
    }
}
