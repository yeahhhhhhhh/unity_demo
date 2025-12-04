using ProtoBuf;
using System;
using System.Collections.Generic;
public class MsgLeaveScene: MsgBase
{
    public service.scene.RequestLeaveScene req = new();
    public MsgLeaveScene()
    {
        base.cmd_id_ = (short)MsgPbType.LEAVE_SCENE;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }


    public class Response: MsgBase
    {
        public service.scene.RequestLeaveScene.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.LEAVE_SCENE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestLeaveScene.Response)data;
        }
    }
}
