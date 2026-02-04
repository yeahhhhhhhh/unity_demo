using ProtoBuf;
using System;
using System.Collections.Generic;

public class MsgResponseBuff: MsgBase
{
    public service.scene.ResponseUpdateBuff resp;
    public MsgResponseBuff()
    {
        base.cmd_id_ = (short)MsgRespPbType.BUFF_UPDATE;
    }
    public override void SetResponseData(IExtensible data)
    {
        resp = (service.scene.ResponseUpdateBuff)data;
    }
}
