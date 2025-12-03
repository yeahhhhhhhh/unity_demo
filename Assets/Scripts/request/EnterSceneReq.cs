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

        // 生成主玩家
        Int64 uid = resp_msg.resp.Uid;
        if (uid == MainPlayer.GetUid())
        {
            // 进入场景，生成模型
            Int32 scene_id = resp_msg.resp.SceneInfo.SceneId;
            Int32 scene_gid = resp_msg.resp.SceneInfo.SceneGid;
            if (SceneManager.scene_id_ != scene_id || SceneManager.scene_gid_ != scene_gid)
            {
                SceneManager.Init(scene_id, scene_gid);
            }

            PlayerInfo player = SceneManager.FindPlayer(uid);
            if (player == null)
            {
                GameObject prefab = ResManager.LoadPrefab("PlayerPrefab");
                if (prefab != null)
                {
                    Vector3 pos = new()
                    {
                        x = resp_msg.resp.SceneInfo.Position.X,
                        y = resp_msg.resp.SceneInfo.Position.Y,
                        z = resp_msg.resp.SceneInfo.Position.Z
                    };
                    GameObject instance = Instantiate(prefab, pos, Quaternion.identity);
                    instance.name = "MainPlayer";
                    instance.SetActive(true);
                    MainPlayer.player_skin_ = instance;
                    GameMain.SetMainCanvasActive(false);
                    MainPlayer.player_.scene_info_.pos_ = pos;

                    // 第一次进入获取场景玩家
                    NetManager.AddMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
                    MsgGetScenePlayers get_scene_players_msg = new();
                    NetManager.Send(get_scene_players_msg);

                    SceneManager.AddPlayer(MainPlayer.player_);
                }

                //GameObject main_player = new GameObject("MainPlayer");
                //MainPlayerActor actor = main_player.AddComponent<MainPlayerActor>();
                //actor.Init("PlayerPrefab");
            }
            else
            {
                Debug.Log("main player already exist, uid:" + uid.ToString());
            }
        }
        else
        {
            Debug.Log("玩家进入场景,uid:" + uid.ToString());
        }
    }

    public void OnGetScenePlayers(MsgBase msg)
    {
        MsgGetScenePlayers.Response resp_msg = (MsgGetScenePlayers.Response)msg;
        Int32 scene_id = resp_msg.resp.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneGid;
        Int32 player_count = resp_msg.resp.PlayerCount;

        Debug.Log("OnGetScenePlayers, player_count:" + player_count.ToString());

        NetManager.RemoveMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
    }
}
