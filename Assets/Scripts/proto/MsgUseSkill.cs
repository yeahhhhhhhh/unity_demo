using ProtoBuf;
using System;
using System.Collections.Generic;

public class MsgUseSkill: MsgBase
{
    public service.scene.RequestUseSkill req = new();

    public class Response: MsgBase
    {
        public service.scene.RequestUseSkill.Response resp;
        public Response()
        {
            cmd_id_ = (short)MsgRespPbType.USE_SKILL_RESPONSE;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestUseSkill.Response)data;
        }
    }

    public class ResponsePos : MsgBase
    {
        public service.scene.RequestUseSkill.ResponsePos resp;
        public ResponsePos()
        {
            cmd_id_ = (short)MsgRespPbType.SKILL_RESPONSE_POS;
        }
        public override void SetResponseData(IExtensible data)
        {
            resp = (service.scene.RequestUseSkill.ResponsePos)data;
        }
    }

    public MsgUseSkill() {
        cmd_id_ = (short)MsgPbType.USE_SKILL;
    }
    public void SetSendData(Int32 skill_id)
    {
        req.SkillId = skill_id;
    }
    public override IExtensible GetSendData()
    {
        return req;
    }
}
