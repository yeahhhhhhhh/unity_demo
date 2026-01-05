using ProtoBuf;
using System;
using System.Collections.Generic;
public class MsgReborn: MsgBase
{
    public service.scene.RequestReborn req = new();

    public class Response : MsgBase
    {
        public service.scene.RequestReborn.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.REBORN;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestReborn.Response)data;
        }
    }

    public MsgReborn()
    {
        base.cmd_id_ = (short)MsgPbType.REBORN;
    }

    public void SetSendData(Int32 type)
    {
        req.Type = type;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }
}
