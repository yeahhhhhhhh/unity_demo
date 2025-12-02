using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

public class MsgLogin: MsgBase
{
    public service.account.LoginRequest req = new service.account.LoginRequest();

    public class Response: MsgBase
    {
        public service.account.LoginRequest.Response resp = new service.account.LoginRequest.Response();
        public Response()
        {
            base.cmd_id_ = (short)MsgPbType.LoginRet;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.account.LoginRequest.Response)data;
        }
    }

    public MsgLogin() {
        base.cmd_id_ = (short)MsgPbType.Login;
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
