using System;
using System.Collections.Generic;
using UnityEngine;

public static class SceneManager
{
    public static Int32 scene_id_ = 0;
    public static Int32 scene_gid_ = 0;
    public static bool is_in_scene_ = false;
    
    public static Dictionary<Int64, PlayerInfo> scene_players = new();

    public static void Init(Int32 scene_id, Int32 scene_gid)
    {
        foreach (var item in scene_players)
        {
            PlayerInfo info = item.Value;
            if(info.skin_ != null)
            {
                GameObject.Destroy(info.skin_);
            }
        }
        scene_players.Clear();
        scene_id_ = scene_id;
        scene_gid_ = scene_gid;

        if (scene_id == 0)
        {
            is_in_scene_ = false;
        }
        else
        {
            is_in_scene_ = true;
        }
    }

    public static void AddPlayer(PlayerInfo player)
    {
        Int64 uid = player.base_info_.uid;
        if (scene_players.ContainsKey(uid))
        {
            return;
        }

        scene_players[uid] = player;
    }

    public static void RemovePlayer(Int64 uid)
    {
        scene_players.Remove(uid);
    }

    public static PlayerInfo FindPlayer(Int64 uid)
    {
        if (scene_players.ContainsKey(uid))
        {
            return scene_players[uid];
        }

        return null;
    }
}
