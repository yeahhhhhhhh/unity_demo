using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public static class NpcConfig
{
    public static Dictionary<Int32, NpcInfo> npc_id2data_ = new();
    public static void Init()
    {
        List<NpcInfo> npc_list = new()
        {
            new(){
                npc_id_ = 1,
                name_ = "monster_1",
                max_hp_ = 10,
            },
        };

        for (int i = 0; i < npc_list.Count; ++i)
        {
            npc_id2data_[npc_list[i].npc_id_] = npc_list[i];
        }
    }

    public static NpcInfo GetNpcInfo(Int32 npc_id)
    {
        if (npc_id2data_.ContainsKey(npc_id))
        {
            return npc_id2data_[npc_id];
        }

        return null;
    }
}
