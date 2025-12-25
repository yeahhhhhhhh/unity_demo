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
        // 注册位置同步
        NetManager.AddMsgListener((short)MsgRespPbType.MOVE_UPDATE_POS, OnPlayerPosUpdate);
        NetManager.AddMsgListener((short)MsgRespPbType.MOVE, OnPlayerMove);

        // 视野更新
        NetManager.AddMsgListener((short)MsgRespPbType.UPDATE_VIEW, OnUpdateView);
        NetManager.AddMsgListener((short)MsgRespPbType.LEAVE_VIEW, OnLeaveView);
        NetManager.AddMsgListener((short)MsgRespPbType.ENTER_VIEW, OnEnterView);
        //NetManager.RemoveMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);

        // NPC视野更新
        NetManager.AddMsgListener((short)MsgRespPbType.UPDATE_NPC_VIEW, OnUpdateNpcView);
    }

    public void OnUpdateNpcView(MsgBase msg)
    {
        MsgResponseNpcView resp_msg = (MsgResponseNpcView)msg;
        List<attributes.scene.NpcSceneInfo> leave_npcs = resp_msg.resp.LeaveNpcs;
        List<attributes.scene.NpcSceneInfo> enter_npcs = resp_msg.resp.EnterNpcs;
        List<attributes.scene.NpcSceneInfo> update_npcs = resp_msg.resp.UpdateNpcs;

        for (int i = 0; i < leave_npcs.Count; ++i)
        {
            Int64 npc_gid = leave_npcs[i].NpcGid;
            SceneManager.DeleteNpc(npc_gid);
        }

        for (int i = 0; i < enter_npcs.Count; ++i)
        {
            attributes.scene.NpcSceneInfo npc_scene_info = enter_npcs[i];
            Int64 npc_gid = npc_scene_info.NpcGid;
            Int32 npc_id = npc_scene_info.NpcId;
            Vector3 pos = new()
            {
                x = npc_scene_info.Position.X,
                y = npc_scene_info.Position.Y,
                z = npc_scene_info.Position.Z,
            };
            Int32 direction = npc_scene_info.Position.Direction;
            Vector3 rotation = MoveManager.GetRotaionByDirection(direction);
            NpcInfo npc = SceneManager.CreateNpc(pos, rotation, npc_id, npc_gid);
            npc.cur_hp_ = npc_scene_info.CurHp;
        }

        for (int i = 0; i < update_npcs.Count; ++i)
        {
            attributes.scene.NpcSceneInfo npc_scene_info = update_npcs[i];
            Int64 npc_gid = npc_scene_info.NpcGid;

            NpcInfo npc = SceneManager.FindNpc(npc_gid);
            if (npc != null)
            {
                Int32 npc_id = npc_scene_info.NpcId;
                Vector3 pos = new()
                {
                    x = npc_scene_info.Position.X,
                    y = npc_scene_info.Position.Y,
                    z = npc_scene_info.Position.Z,
                };
                Int32 direction = npc_scene_info.Position.Direction;
                npc.cur_hp_ = npc_scene_info.CurHp;
                NpcSyncActor sync_comp = npc.skin_.transform.GetComponent<NpcSyncActor>();
                if (sync_comp != null)
                {
                    sync_comp.SyncPos(pos, direction);
                }
            }
        }
    }

    public void OnLeaveView(MsgBase msg)
    {
        MsgResponseLeaveView resp_msg = (MsgResponseLeaveView)msg;
        Int64 uid = resp_msg.resp.Uid;
        SceneManager.DeletePlayer(uid);
    }

    public void OnEnterView(MsgBase msg)
    {
        MsgResponseEnterView resp_msg = (MsgResponseEnterView)msg;
        Int64 uid = resp_msg.resp.Uid;
        String nickname = resp_msg.resp.Nickname;
        attributes.scene.SceneInfo scene_info = resp_msg.resp.SceneInfo;
        Int32 scene_id = scene_info.SceneId;
        Int32 scene_gid = scene_info.SceneGid;
        Vector3 pos = new()
        {
            x = scene_info.Position.X,
            y = scene_info.Position.Y,
            z = scene_info.Position.Z
        };
        Vector3 rotation = MoveManager.GetRotaionByDirection(scene_info.Position.Direction);
        SceneManager.CreatePlayer(pos, rotation, uid, scene_id, scene_gid, nickname);
    }

    public void OnUpdateView(MsgBase msg)
    {
        MsgResponseUpdateView resp_msg = (MsgResponseUpdateView)msg;
        Int64 uid = resp_msg.resp.Uid;
        Int32 scene_id = resp_msg.resp.SceneInfo.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneInfo.SceneGid;
        if (scene_id != SceneManager.scene_id_ || scene_gid != SceneManager.scene_gid_)
        {
            Debug.Log("OnUpdateView, scene id or scene gid is not same, scene id:" + scene_id + " scene id:" + SceneManager.scene_id_);
            return;
        }
        List <attributes.scene.PlayerSceneInfo> leave_view_player_list = resp_msg.resp.LeavePlayers;
        List<attributes.scene.PlayerSceneInfo> enter_view_player_list = resp_msg.resp.EnterPlayers;

        for (int i = 0; i < leave_view_player_list.Count; ++i)
        {
            attributes.scene.PlayerSceneInfo player_info = leave_view_player_list[i];
            Int64 leave_uid = player_info.Uid;
            SceneManager.DeletePlayer(leave_uid);
        }

        
        for (int i = 0; i < enter_view_player_list.Count; ++i)
        {
            attributes.scene.PlayerSceneInfo player_info = enter_view_player_list[i];
            Int64 enter_uid = player_info.Uid;
            String nickname = player_info.Nickname;
            Vector3 pos = new()
            {
                x = player_info.Position.X,
                y = player_info.Position.Y,
                z = player_info.Position.Z
            };
            Vector3 rotation = MoveManager.GetRotaionByDirection(player_info.Position.Direction);
            SceneManager.CreatePlayer(pos, rotation, enter_uid, scene_id, scene_gid, nickname);
        }
    }

    public void OnPlayerPosUpdate(MsgBase msg)
    {
        MsgMove.Response resp_msg = (MsgMove.Response)msg;
        Int64 uid = resp_msg.resp.Uid;
        Int32 scene_id = resp_msg.resp.SceneInfo.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneInfo.SceneGid;
        float x = resp_msg.resp.SceneInfo.Position.X;
        float y = resp_msg.resp.SceneInfo.Position.Y;
        float z = resp_msg.resp.SceneInfo.Position.Z;
        Int32 direction = resp_msg.resp.SceneInfo.Position.Direction;
        Vector3 pos = new Vector3(x, y, z);
        // 判断是否在场景
        if (scene_id != SceneManager.scene_id_ || scene_gid != SceneManager.scene_gid_)
        {
            Debug.Log("OnPlayerPosUpdate scene id is error, scene id:" + scene_id.ToString());
            return;
        }
        var player_info = SceneManager.FindPlayer(uid);
        if(player_info == null)
        {
            Debug.Log("OnPlayerPosUpdate player is not in scene, uid:" + uid);
            return;
        }
        // 判断是否是主角色
        bool is_main_player = uid == MainPlayer.GetUid();
        if (is_main_player)
        {
            Debug.Log("OnPlayerPosUpdate main player");
        }
        else
        {
            Debug.Log("OnPlayerPosUpdate player, uid:" + uid.ToString());
            SyncPlayerActor actor = player_info.skin_.GetComponent<SyncPlayerActor>();
            if (actor != null)
            {
                actor.SyncPos(pos, direction);
            }
        }
    }

    public void OnPlayerMove(MsgBase msg)
    {
        MsgNewMove.Response resp_msg = (MsgNewMove.Response)msg;
        Int64 uid = resp_msg.resp.Uid;
        Vector3 pos = new(
            resp_msg.resp.SceneInfo.Position.X, 
            resp_msg.resp.SceneInfo.Position.Y, 
            resp_msg.resp.SceneInfo.Position.Z);
        Int32 direction = resp_msg.resp.SceneInfo.Position.Direction;

        PlayerInfo player = SceneManager.FindPlayer(uid);
        if (player == null)
        {
            Debug.Log("OnPlayerMove player is null, uid:" + uid.ToString());
            return;
        }

        SyncPlayerActor actor = player.skin_.GetComponent<SyncPlayerActor>();
        if (actor != null)
        {
            actor.SyncPos(pos, direction);
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
