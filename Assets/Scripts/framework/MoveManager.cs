using attributes.scene;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DirectionType
{
    START = 0,
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
    WA = 5,  // 左上
    WD = 6,  // 右上
    SA = 7,  // 左下
    SD = 8, // 右下
    END = 9,
}
public class MoveManager
{
    private static MoveManager instance;
    public static MoveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MoveManager();
            }
            return instance;
        }
    }

    // 私有构造函数防止外部使用new创建实例
    private MoveManager()
    {
        // 初始化
    }

    public void Init()
    {
        Debug.Log("MoveManager init");
        // 注册位置同步(客户端为准)
        NetManager.AddMsgListener((short)MsgRespPbType.MOVE_UPDATE_POS, OnPlayerPosUpdate);
        // 服务端为准
        NetManager.AddMsgListener((short)MsgRespPbType.MOVE, OnPlayerMove);

        // 视野更新
        NetManager.AddMsgListener((short)MsgRespPbType.UPDATE_VIEW, OnUpdateView);
        NetManager.AddMsgListener((short)MsgRespPbType.LEAVE_VIEW, OnLeaveView);
        NetManager.AddMsgListener((short)MsgRespPbType.ENTER_VIEW, OnEnterView);

    }

    public void OnLeaveView(MsgBase msg)
    {
        MsgResponseLeaveView resp_msg = (MsgResponseLeaveView)msg;
        SceneMgr.DeleteEntity(resp_msg.resp.GlobalId);
    }

    public void OnEnterView(MsgBase msg)
    {
        MsgResponseEnterView resp_msg = (MsgResponseEnterView)msg;
        attributes.scene.EntitySceneInfo entity_info = resp_msg.resp.InViewEntity;
        EntitySimpleInfo entity = new();
        entity.Copy(entity_info);
        SceneMgr.CreateEntity(entity);
    }

    public void OnUpdateView(MsgBase msg)
    {
        MsgResponseUpdateView resp_msg = (MsgResponseUpdateView)msg;
        List <attributes.scene.EntitySceneInfo> leave_view_entity_list = resp_msg.resp.OutViewEntities;
        List<attributes.scene.EntitySceneInfo> enter_view_entity_list = resp_msg.resp.InViewEntities;

        for (int i = 0; i < leave_view_entity_list.Count; ++i)
        {
            attributes.scene.EntitySceneInfo entity_info = leave_view_entity_list[i];
            SceneMgr.DeleteEntity(entity_info.GlobalId);
        }

        for (int i = 0; i < enter_view_entity_list.Count; ++i)
        {
            attributes.scene.EntitySceneInfo entity_info = enter_view_entity_list[i];
            EntitySimpleInfo entity = new();
            entity.Copy(entity_info);
            SceneMgr.CreateEntity(entity);
        }
    }

    public void OnPlayerPosUpdate(MsgBase msg)
    {
        //MsgMove.Response resp_msg = (MsgMove.Response)msg;
        //Int64 uid = resp_msg.resp.Uid;
        //Int32 scene_id = resp_msg.resp.SceneInfo.SceneId;
        //Int32 scene_gid = resp_msg.resp.SceneInfo.SceneGid;
        //float x = resp_msg.resp.SceneInfo.Position.X;
        //float y = resp_msg.resp.SceneInfo.Position.Y;
        //float z = resp_msg.resp.SceneInfo.Position.Z;
        //Int32 direction = resp_msg.resp.SceneInfo.Position.Direction;
        //Vector3 pos = new Vector3(x, y, z);
        //// 判断是否在场景
        //if (scene_id != SceneMgr.scene_id_ || scene_gid != SceneMgr.scene_gid_)
        //{
        //    Debug.Log("OnPlayerPosUpdate scene id is error, scene id:" + scene_id.ToString());
        //    return;
        //}
        //var player_info = SceneMgr.FindEntity(uid);
        //if(player_info == null)
        //{
        //    Debug.Log("OnPlayerPosUpdate player is not in scene, uid:" + uid);
        //    return;
        //}
        //// 判断是否是主角色
        //bool is_main_player = uid == MainPlayer.GetUid();
        //if (is_main_player)
        //{
        //    Debug.Log("OnPlayerPosUpdate main player");
        //}
        //else
        //{
        //    Debug.Log("OnPlayerPosUpdate player, uid:" + uid.ToString());
        //    SyncPlayerActor actor = player_info.skin_.GetComponent<SyncPlayerActor>();
        //    if (actor != null)
        //    {
        //        actor.SyncPos(pos, direction);
        //    }
        //}
    }

    public void OnPlayerMove(MsgBase msg)
    {
        MsgNewMove.Response resp_msg = (MsgNewMove.Response)msg;
        Int64 global_id = resp_msg.resp.GlobalId;
        Vector3 pos = new(
            resp_msg.resp.Position.X, 
            resp_msg.resp.Position.Y, 
            resp_msg.resp.Position.Z);
        Int32 direction = resp_msg.resp.Position.Direction;

        EntitySimpleInfo entity = SceneMgr.FindEntity(global_id);
        if (entity == null)
        {
            Debug.Log("entity is null, global_id:" + global_id.ToString());
            return;
        }

        if (entity.type_ == (Int32)EntityTypes.PLAYER)
        {
            SyncPlayerActor actor = entity.skin_.GetComponent<SyncPlayerActor>();
            if (actor != null)
            {
                actor.SyncPos(pos, direction);
            }
        }
        else if (entity.type_ == (Int32)EntityTypes.NPC)
        {
            NpcSyncActor actor = entity.skin_.GetComponent<NpcSyncActor>();
            if (actor != null)
            {
                actor.SyncPos(pos, direction);
            }
        }
        else if (entity.type_ == (Int32)EntityTypes.FIGHT)
        {
            ActiveSkillActor actor = entity.skin_.GetComponent<ActiveSkillActor>();
            if (actor != null)
            {
                actor.SyncPos(pos);
            }
        }

    }

    public static Vector3 GetRotaionByDirection(Int32 direction)
    {
        switch (direction)
        {
            case (Int32)DirectionType.UP:
                return new Vector3(0, 0, 0);
            case (Int32)DirectionType.DOWN:
                return new Vector3(0, 180, 0);
            case (Int32)DirectionType.LEFT:
                return new Vector3(0, -90, 0);
            case (Int32)DirectionType.RIGHT:
                return new Vector3(0, 90, 0);
            default:
                return new Vector3(0, 0, 0);
        }
    }
}
