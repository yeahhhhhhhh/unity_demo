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
    }

    public void SetSendData(Vector3 pos)
    {
        req.X = pos.x;
        req.Y = pos.y;
        req.Z = pos.z;
        Debug.Log("move, x:" + req.X + " y:" + req.Y + " z:" + req.Z);
    }
    public override IExtensible GetSendData()
    {
        return req;
    }
}
