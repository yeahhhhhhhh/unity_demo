using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

public class MsgLogin: MsgBase
{
    public service.account.LoginRequest req = new();

    public class Response: MsgBase
    {
        public service.account.LoginRequest.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.LOGIN_RESPONSE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.account.LoginRequest.Response)data;
        }
    }

    public MsgLogin() {
        base.cmd_id_ = (short)MsgPbType.LOGIN;
    }

    public void SetVisitorData(string device_id)
    {
        req.DeviceId = device_id;
    }
    public void SetSendData(string username, string password)
    {
        req.Username = username;
        req.Password = password;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }

}
