using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener((short)MsgPbType.LoginRet, OnLoginResp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLoginClick()
    {
        Debug.Log("click login");
        MsgLogin login_msg = new MsgLogin();
        login_msg.SetSendData("why", "123");
        NetManager.Send(login_msg);
    }

    public void OnLoginResp(MsgBase msg)
    {
        Debug.Log("OnLoginResp");
        MsgLogin.Response resp_msg = (MsgLogin.Response)msg;
        Debug.Log("errorcode:" + resp_msg.resp.ErrorCode + " uid:" + resp_msg.resp.PlayerBaseInfo.Uid + " username:" + resp_msg.resp.PlayerBaseInfo.Username);
    }
}
