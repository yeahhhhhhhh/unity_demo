using System;
using System.Collections.Generic;

public static class SceneManager
{
    public static Int32 scene_id_ = 0;
    public static Int32 scene_gid_ = 0;
    
    public static Dictionary<Int64, PlayerInfo> scene_players = new();

    public static void Init(Int32 scene_id, Int32 scene_gid)
    {
        scene_players.Clear();
        scene_id_ = scene_id;
        scene_gid_ = scene_gid;
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
