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
