using ProtoBuf;

public class MsgCommonResponse : MsgBase
{
    public service.common.CommonResponse resp;

    public MsgCommonResponse() {
        base.cmd_id_ = (short)MsgRespPbType.COMMON;
    }
    public override void SetResponseData(IExtensible data)
    {
        resp = (service.common.CommonResponse)data;
    }

}
