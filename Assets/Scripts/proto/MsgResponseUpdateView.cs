using ProtoBuf;
using System;
using System.Collections.Generic;


public class MsgResponseUpdateView: MsgBase
{
    public service.scene.ResponseUpdateView resp;
    public MsgResponseUpdateView()
    {
        base.cmd_id_ = (short)MsgRespPbType.UPDATE_VIEW;
    }
    public override void SetResponseData(IExtensible data)
    {
        resp = (service.scene.ResponseUpdateView)data;
    }
}

public class MsgResponseLeaveView : MsgBase
{
    public service.scene.ResponseLeaveView resp;
    public MsgResponseLeaveView()
    {
        base.cmd_id_ = (short)MsgRespPbType.LEAVE_VIEW;
    }
    public override void SetResponseData(IExtensible data)
    {
        resp = (service.scene.ResponseLeaveView)data;
    }
}

public class MsgResponseEnterView : MsgBase
{
    public service.scene.ResponseEnterView resp;
    public MsgResponseEnterView()
    {
        base.cmd_id_ = (short)MsgRespPbType.ENTER_VIEW;
    }
    public override void SetResponseData(IExtensible data)
    {
        resp = (service.scene.ResponseEnterView)data;
    }
}
