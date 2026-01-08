using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

public class MsgRegister : MsgBase
{
    public service.account.RegisterRequest req = new();

    public class Response: MsgBase
    {
        public service.account.RegisterRequest.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.REGIST_RESPONSE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.account.RegisterRequest.Response)data;
        }
    }

    public MsgRegister() {
        base.cmd_id_ = (short)MsgPbType.REGIST;
    }

    public void SetVisitorData(string device_id)
    {
        req.IsVisitor = true;
        req.DeviceId = device_id;
    }
    public void SetSendData(string username, string password1, string password2)
    {
        req.IsVisitor = false;
        req.Username = username;
        req.Password1 = password1;
        req.Password2 = password2;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }

}
