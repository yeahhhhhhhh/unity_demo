using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ProtoBuf;

public class MsgMove: MsgBase
{
    public service.scene.RequestUpdatePos req = new();

    public class MsgMoveResp: MsgBase
    {
        public service.scene.RequestUpdatePos.Response resp = new();
    }

    public MsgMove() {
        cmd_id_ = (short)MsgType.Move;
    }

    public void SetSendData(int x, int y, int z)
    {
        req.X = x;
        req.Y = y;
        req.Z = z;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }
}
