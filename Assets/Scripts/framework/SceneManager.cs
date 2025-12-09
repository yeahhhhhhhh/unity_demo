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

    public static PlayerInfo CreatePlayer(Vector3 pos, Vector3 rotation, Int64 uid, Int32 scene_id, Int32 scene_gid)
    {
        if (SceneManager.scene_id_ != scene_id || SceneManager.scene_gid_ != scene_gid)
        {
            Debug.Log("CreatePlayer error, not same scene, scene id:" + scene_id.ToString());
            return null;
        }

        PlayerInfo already_player = SceneManager.FindPlayer(uid);
        if (already_player != null)
        {
            Debug.Log("already_player, uid:" + uid);
            return null;
        }

        GameObject prefab = ResManager.LoadPrefab("PlayerPrefab");
        if (prefab == null)
        {
            Debug.Log("PlayerPrefab is null");
            return null;
        }

        bool is_main_player = MainPlayer.GetUid() == uid;

        Debug.Log("create player, uid:" + uid.ToString() + ", x:" + pos.x + " y:" + pos.y + " z:" + pos.z);
        GameObject instance = UnityEngine.GameObject.Instantiate(prefab, pos, Quaternion.Euler(rotation));
        //instance.transform.eulerAngles = rotation;
        //instance.transform.position = pos;
        PlayerInfo player;
        if (is_main_player)
        {
            instance.name = "MainPlayer" + uid.ToString();
            player = MainPlayer.player_;
            // 挂上控制脚本
            instance.AddComponent<MainPlayerActor>();
        }
        else
        {
            instance.name = "OtherPlayer" + uid.ToString();
            player = new();
        }
        // 挂上同步脚本
        instance.AddComponent<SyncPlayerActor>();
        instance.SetActive(true);

        player.base_info_.uid = uid;
        player.scene_info_.pos_ = pos;
        player.scene_info_.scene_id_ = scene_id;
        player.scene_info_.scene_gid_ = scene_gid;
        player.skin_ = instance;
        SceneManager.AddPlayer(player);

        return player;
    }

    public static void DeletePlayer(Int64 uid)
    {
        PlayerInfo leave_player = SceneManager.FindPlayer(uid);
        if (leave_player != null)
        {
            SceneManager.RemovePlayer(uid);
            GameObject.Destroy(leave_player.skin_);
        }
    }

}
