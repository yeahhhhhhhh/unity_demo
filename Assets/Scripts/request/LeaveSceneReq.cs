using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSceneReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener((short)MsgRespPbType.LEAVE_SCENE, OnPlayerLeaveScene);
        //NetManager.RemoveMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLeaveSceneClick()
    {
        Debug.Log("OnLeaveSceneClick");
        MsgLeaveScene msg = new();
        NetManager.Send(msg);
    }

    public void OnPlayerLeaveScene(MsgBase msg)
    {
        MsgLeaveScene.Response resp_msg = (MsgLeaveScene.Response)msg;
        Int32 scene_id = resp_msg.resp.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneGid;
        Int64 uid = resp_msg.resp.Uid;
        string nickname = resp_msg.resp.Nickname;

        // TODO: 消息提示
        Debug.Log("player leave scene, uid:" + uid.ToString() + " nickname:" + nickname + " scene id:" + scene_id);

        // 是否是和主角同一个场景
        if (MainPlayer.player_.scene_info_.scene_gid_ != scene_gid)
        {
            return;
        }

        // 判断是否是主角
        bool is_main_player = MainPlayer.GetUid() == uid;
        if (is_main_player)
        {
            // 销毁场景，弹出窗口
            SceneManager.Init(0, 0);
            // 重置玩家场景数据
            MainPlayer.player_.scene_info_ = new();
            GameMain.SetMainCanvasActive(true);
        }
        else
        {
            // 销毁其他玩家
            PlayerInfo info = SceneManager.FindPlayer(uid);
            if (info != null)
            {
                GameObject.Destroy(info.skin_);
                SceneManager.RemovePlayer(uid);
            }
        }
    }
}
