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
        //NetManager.AddMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
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
        attributes.scene.EntitySceneInfo entity_scene_info = resp_msg.resp.EntitySceneInfo;
        attributes.scene.SceneInfo scene_info = resp_msg.resp.SceneInfo;
        Debug.Log("OnEnterScene, error_code:" + resp_msg.resp.ErrorCode + " uid:" + entity_scene_info.Id);
        Int64 uid = entity_scene_info.Id;
        Int32 scene_id = scene_info.SceneId;
        Int32 scene_gid = scene_info.SceneGid;

        EntitySimpleInfo entity = new();
        entity.Copy(entity_scene_info);

        // 生成主玩家
        // 进入场景，生成模型
        SceneManager.Init(scene_id, scene_gid);
        entity = SceneManager.CreateEntity(entity);
        if (entity != null)
        {
            Debug.Log("create main player success, uid:" + uid.ToString());
            GameMain.SetMainCanvasActive(false);
            // 第一次进入获取战斗信息
            MsgGetFightInfo get_fight_info_msg = new();
            get_fight_info_msg.SetSendData(uid);
            NetManager.Send(get_fight_info_msg);

            // 设置摄像头
            CameraFollower follower = Camera.main.AddComponent<CameraFollower>();
            follower.Init(entity.skin_.transform, new Vector3()
            {
                x = 0,
                y = 12,
                z = -10
            });

            entity.skin_.name = "MainPlayer" + uid.ToString();
            // 挂上控制脚本
            entity.skin_.AddComponent<MainPlayerActor>();
        }
    }

    //public void OnGetScenePlayers(MsgBase msg)
    //{
    //    MsgGetScenePlayers.Response resp_msg = (MsgGetScenePlayers.Response)msg;
    //    Int32 scene_id = resp_msg.resp.SceneId;
    //    Int32 scene_gid = resp_msg.resp.SceneGid;
    //    Int32 player_count = resp_msg.resp.PlayerCount;
    //    Debug.Log("OnGetScenePlayers, player_count:" + player_count.ToString());

    //    List<attributes.scene.PlayerSceneInfo> player_list = resp_msg.resp.Players;
    //    for (int i = 0; i < player_list.Count; ++i)
    //    {
    //        attributes.scene.PlayerSceneInfo player_info = player_list[i];
    //        Int64 uid = player_info.Uid;
    //        String nickname = player_info.Nickname;
    //        Vector3 pos = new()
    //        {
    //            x = player_info.Position.X,
    //            y = player_info.Position.Y,
    //            z = player_info.Position.Z
    //        };
    //        Vector3 rotation = MoveManager.GetRotaionByDirection(player_info.Position.Direction);
    //        SceneManager.CreatePlayer(pos, rotation, uid, scene_id, scene_gid, nickname);
    //    }

    //    List<attributes.scene.NpcSceneInfo> npc_list = resp_msg.resp.Npcs;
    //    for (int i = 0; i < npc_list.Count; ++i)
    //    {
    //        attributes.scene.NpcSceneInfo npc_scene_info = npc_list[i];
    //        Int64 npc_gid = npc_scene_info.NpcGid;
    //        Int32 npc_id = npc_scene_info.NpcId;
    //        Vector3 pos = new()
    //        {
    //            x = npc_scene_info.Position.X,
    //            y = npc_scene_info.Position.Y,
    //            z = npc_scene_info.Position.Z,
    //        };
    //        Int32 direction = npc_scene_info.Position.Direction;
    //        Vector3 rotation = MoveManager.GetRotaionByDirection(direction);
    //        NpcInfo npc = SceneManager.CreateNpc(pos, rotation, npc_id, npc_gid);
    //        npc.cur_hp_ = npc_scene_info.CurHp;
    //        HUDManager hud_mgr = npc.skin_.transform.GetComponentInChildren<HUDManager>();
    //        if (hud_mgr != null)
    //        {
    //            hud_mgr.UpdateNickname(npc.name_);
    //            hud_mgr.UpdateHealth(npc.cur_hp_, npc.max_hp_);
    //        }
    //    }
    //}
}
