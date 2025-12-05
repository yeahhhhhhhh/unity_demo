using ProtoBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MsgNewMove: MsgBase
{
    public service.scene.RequestMove req = new();

    public class Response: MsgBase
    {
        public service.scene.RequestMove.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.MOVE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestMove.Response)data;
        }
    }

    public MsgNewMove()
    {
        base.cmd_id_ = (short)MsgPbType.MOVE;
    }

    public void SetSendData(Int32 direction, bool stop_move)
    {
        req.Direction = direction;
        req.StopMove = stop_move;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }
}
