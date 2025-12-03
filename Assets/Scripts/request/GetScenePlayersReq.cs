using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetScenePlayersReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener((short)MsgRespPbType.GET_SCENE_PLAYERS, OnGetScenePlayers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGetScenePlayersClick()
    {
        Debug.Log("click request get scene players");
        MsgGetScenePlayers msg = new();
        NetManager.Send(msg);
    }

    public void OnGetScenePlayers(MsgBase msg)
    {
        MsgGetScenePlayers.Response resp_msg = (MsgGetScenePlayers.Response)msg;
        Int32 scene_id = resp_msg.resp.SceneId;
        Int32 scene_gid = resp_msg.resp.SceneGid;
        Int32 player_count = resp_msg.resp.PlayerCount;

        Debug.Log("OnGetScenePlayers, player_count:" + player_count.ToString());
    }
}
