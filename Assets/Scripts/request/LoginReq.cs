using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLoginClick()
    {
        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        NetManager.AddMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        Debug.Log("click login");
        MsgLogin login_msg = new();
        login_msg.SetSendData("why", "123");
        NetManager.Send(login_msg);
    }

    public void OnLogin2Click()
    {
        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        NetManager.AddMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        Debug.Log("click login");
        MsgLogin login_msg = new();
        login_msg.SetSendData("why2", "123");
        NetManager.Send(login_msg);
    }

    public void OnLoginResp(MsgBase msg)
    {
        Debug.Log("OnLoginResp");
        MsgLogin.Response resp_msg = (MsgLogin.Response)msg;
        Debug.Log("errorcode:" + resp_msg.resp.ErrorCode + " uid:" + resp_msg.resp.PlayerBaseInfo.Uid + " username:" + resp_msg.resp.PlayerBaseInfo.Username);

        Int64 uid = resp_msg.resp.PlayerBaseInfo.Uid;
        string username = resp_msg.resp.PlayerBaseInfo.Username;
        string nickname = resp_msg.resp.PlayerBaseInfo.Nickname;
        MainPlayer.SetPlayerBaseInfo(uid, username, nickname);

        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
    }
}
