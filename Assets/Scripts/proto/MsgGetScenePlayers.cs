using ProtoBuf;
using System;
using System.Collections.Generic;

public class MsgGetScenePlayers: MsgBase
{
    public service.scene.RequestGetScenePlayers req = new();

    public class Response: MsgBase
    {
        public service.scene.RequestGetScenePlayers.Response resp;
        public Response()
        {
            base.cmd_id_ = (short)MsgRespPbType.GET_SCENE_PLAYERS;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestGetScenePlayers.Response)data;
        }
    }

    public MsgGetScenePlayers()
    {
        base.cmd_id_ = (short)MsgPbType.GET_SCENE_PLAYERS;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }

}
