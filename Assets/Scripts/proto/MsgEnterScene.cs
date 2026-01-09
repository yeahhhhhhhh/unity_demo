using ProtoBuf;
using System;
using System.Collections.Generic;

public class MsgEnterScene: MsgBase
{
    public service.scene.RequestEnterDefaultScene req = new();

    public class Response: MsgBase
    {
        public service.scene.RequestEnterDefaultScene.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.ENTER_DEFAULT_SCENE_RESPONSE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestEnterDefaultScene.Response)data;
        }
    }

    public MsgEnterScene()
    {
        base.cmd_id_ = (short)MsgPbType.ENTER_DEFAULT_SCENE;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }

}


public class MsgPreEnterScene : MsgBase
{
    public service.scene.RequestPreEnterScene req = new();

    public class Response : MsgBase
    {
        public service.scene.RequestPreEnterScene.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.PRE_ENTER_DEFAULT_SCENE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestPreEnterScene.Response)data;
        }
    }

    public MsgPreEnterScene()
    {
        base.cmd_id_ = (short)MsgPbType.PRE_ENTER_DEFAULT_SCENE;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }

}
