using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterSceneReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener((short)MsgRespPbType.ENTER_DEFAULT_SCENE_RESPONSE, OnEnterDefaultScene);
        NetManager.AddMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
        //NetManager.RemoveMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterSceneClick()
    {
        Debug.Log("click enter scene");
        MsgEnterScene enter_scene_msg = new();
        NetManager.Send(enter_scene_msg);
    }

    public void OnEnterDefaultScene(MsgBase msg)
    {
        MsgEnterScene.Response resp_msg = (MsgEnterScene.Response)msg;
        Debug.Log("OnEnterScene, error_code:" + resp_msg.resp.ErrorCode + " scene info:" + resp_msg.resp.SceneInfo.SceneId);
        Int32 scene_id = resp_msg.resp.SceneInfo.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneInfo.SceneGid;
        Vector3 pos = new()
        {
            x = resp_msg.resp.SceneInfo.Position.X,
            y = resp_msg.resp.SceneInfo.Position.Y,
            z = resp_msg.resp.SceneInfo.Position.Z
        };
        Int64 uid = resp_msg.resp.Uid;

        // 生成主玩家
        if (uid == MainPlayer.GetUid())
        {
            // 进入场景，生成模型
            if (SceneManager.scene_id_ != scene_id || SceneManager.scene_gid_ != scene_gid)
            {
                SceneManager.Init(scene_id, scene_gid);
            }

            var new_player = CreatePlayer(pos, uid, scene_id, scene_gid);
            if (new_player != null)
            {
                Debug.Log("create main player success, uid:" + uid.ToString());
                GameMain.SetMainCanvasActive(false);
                // 第一次进入获取场景玩家
                MsgGetScenePlayers get_scene_players_msg = new();
                NetManager.Send(get_scene_players_msg);
            }
        }
        else
        {
            Debug.Log("玩家进入场景,uid:" + uid.ToString());
            CreatePlayer(pos, uid, scene_id, scene_gid);
        }
    }

    public void OnGetScenePlayers(MsgBase msg)
    {
        MsgGetScenePlayers.Response resp_msg = (MsgGetScenePlayers.Response)msg;
        Int32 scene_id = resp_msg.resp.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneGid;
        Int32 player_count = resp_msg.resp.PlayerCount;
        Debug.Log("OnGetScenePlayers, player_count:" + player_count.ToString());

        List<attributes.scene.PlayerSceneInfo> player_list = resp_msg.resp.Players;
        for (int i = 0; i < player_list.Count; ++i)
        {
            attributes.scene.PlayerSceneInfo player_info = player_list[i];
            Int64 uid = player_info.Uid;
            Vector3 pos = new()
            {
                x = player_info.Position.X,
                y = player_info.Position.Y,
                z = player_info.Position.Z
            };
            CreatePlayer(pos, uid, scene_id, scene_gid);
        }
    }

    public PlayerInfo CreatePlayer(Vector3 pos, Int64 uid, Int32 scene_id, Int32 scene_gid)
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
        GameObject instance = Instantiate(prefab, pos, Quaternion.identity);
        PlayerInfo player;
        if (is_main_player)
        {
            instance.name = "MainPlayer" + uid.ToString();
            player = MainPlayer.player_;
            // 挂上控制脚本
            var actor = instance.AddComponent<MainPlayerActor>();
        }
        else
        {
            instance.name = "OtherPlayer" + uid.ToString();
            player = new();
            // 挂上同步脚本
            var actor = instance.AddComponent<SyncPlayerActor>();
            actor.Init();
        }

        //instance.transform.position = pos;
        instance.SetActive(true);

        player.base_info_.uid = uid;
        player.scene_info_.pos_ = pos;
        player.scene_info_.scene_id_ = scene_id;
        player.scene_info_.scene_gid_ = scene_gid;
        player.skin_ = instance;
        SceneManager.AddPlayer(player);

        return player;
    }
}
