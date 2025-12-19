using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        String nickname = resp_msg.resp.Nickname;
        Vector3 pos = new()
        {
            x = resp_msg.resp.SceneInfo.Position.X,
            y = resp_msg.resp.SceneInfo.Position.Y,
            z = resp_msg.resp.SceneInfo.Position.Z
        };
        Vector3 rotation = MoveManager.GetRotaionByDirection(resp_msg.resp.SceneInfo.Position.Direction);
        Int64 uid = resp_msg.resp.Uid;

        // 生成主玩家
        if (uid == MainPlayer.GetUid())
        {
            // 进入场景，生成模型
            if (SceneManager.scene_id_ != scene_id || SceneManager.scene_gid_ != scene_gid)
            {
                SceneManager.Init(scene_id, scene_gid);
            }

            PlayerInfo new_player = SceneManager.CreatePlayer(pos, rotation, uid, scene_id, scene_gid, nickname);
            if (new_player != null)
            {
                Debug.Log("create main player success, uid:" + uid.ToString());
                GameMain.SetMainCanvasActive(false);
                // 第一次进入获取场景玩家
                MsgGetScenePlayers get_scene_players_msg = new();
                NetManager.Send(get_scene_players_msg);
                // 第一次进入获取战斗信息
                MsgGetFightInfo get_fight_info_msg = new();
                get_fight_info_msg.SetSendData(uid);
                NetManager.Send(get_fight_info_msg);

                // 设置摄像头
                CameraFollower follower = Camera.main.AddComponent<CameraFollower>();
                follower.Init(new_player.skin_.transform, new Vector3()
                {
                    x = 0,
                    y = 12,
                    z = -10
                });
            }
        }
        else
        {
            Debug.Log("玩家进入场景,uid:" + uid.ToString());
            SceneManager.CreatePlayer(pos, rotation, uid, scene_id, scene_gid, nickname);
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
            String nickname = player_info.Nickname;
            Vector3 pos = new()
            {
                x = player_info.Position.X,
                y = player_info.Position.Y,
                z = player_info.Position.Z
            };
            Vector3 rotation = MoveManager.GetRotaionByDirection(player_info.Position.Direction);
            SceneManager.CreatePlayer(pos, rotation, uid, scene_id, scene_gid, nickname);
        }
    }
}
