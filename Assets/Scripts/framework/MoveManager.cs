using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //NetManager.RemoveMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
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
                actor.SyncPos(pos);
            }
        }
    }
}
