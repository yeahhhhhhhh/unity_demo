using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ProtoBuf;

public class MsgMove: MsgBase
{
    public service.scene.RequestUpdatePos req = new();

    public class Response: MsgBase
    {
        public service.scene.RequestUpdatePos.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.MOVE_UPDATE_POS;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestUpdatePos.Response)data;
        }
    }

    public MsgMove() {
        cmd_id_ = (short)MsgPbType.MOVE_UPDATE_POS;
        req.Pos = new();
    }

    public void SetSendData(Vector3 pos, Int32 direction)
    {
        req.Pos.X = pos.x;
        req.Pos.Y = pos.y;
        req.Pos.Z = pos.z;
        req.Pos.Direction = direction;
        Debug.Log("move, x:" + req.Pos.X + " y:" + req.Pos.Y + " z:" + req.Pos.Z);
    }
    public override IExtensible GetSendData()
    {
        return req;
    }
}
