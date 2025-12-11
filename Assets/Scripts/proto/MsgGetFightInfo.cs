using ProtoBuf;
using System;
using System.Collections.Generic;
public class MsgGetFightInfo: MsgBase
{
    public service.scene.RequestFightInfo req = new();

    public class Response : MsgBase
    {
        public service.scene.RequestFightInfo.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.GET_FIGHT_INFO;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestFightInfo.Response)data;
        }
    }

    public MsgGetFightInfo()
    {
        base.cmd_id_ = (short)MsgPbType.GET_FIGHT_INFO;
    }

    public void SetSendData(Int64 uid)
    {
        req.Uid = uid;
    }

    public override IExtensible GetSendData()
    {
        return req;
    }
}
