using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeaveSceneReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener((short)MsgRespPbType.LEAVE_SCENE, OnPlayerLeaveScene);
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
        // TODO: 消息提示
        Debug.Log("main player leave scene");

        // 销毁场景，弹出窗口
        SceneMgr.Init(0, 0);
        // 重置玩家场景数据
        MainPlayer.player_.scene_info_ = new();
        GameMain.SetMainCanvasActive(true);
        CameraFollower actor = Camera.main.GetComponent<CameraFollower>();
        if (actor != null)
        {
            Destroy(actor);
        }
    }
}
