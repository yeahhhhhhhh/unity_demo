using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;



public class MsgBase{
	public short cmd_id_ = 0;

    public virtual IExtensible GetSendData()
	{
		return null;
	}

    public virtual void SetResponseData(IExtensible data)
    {

    }

	public static byte[] Encode(MsgBase msg)
	{
        IExtensible data = msg.GetSendData();
		if (data == null)
		{
			return null;
		}

        byte[] bytes;
        using (var bodyMem = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(bodyMem, data);
            bytes = bodyMem.ToArray();
        }

        return bytes;
    }

    //解码
    public static MsgBase Decode(short cmd_id, byte[] bytes, int offset, int count){
        return DecodePb(cmd_id, bytes, offset, count);

        //return DecodeJson(cmd_id, bytes, offset, count);
    }

	public static MsgBase DecodePb(short cmd_id, byte[] bytes, int offset, int count)
	{
        try
        {
            using (MemoryStream memory = new MemoryStream(bytes, offset, count))
            {
                Type t = MsgTypeRegister.GetRespPbTypeByID(cmd_id);
                if (t == null)
                {
                    Debug.Log("type is null");
                    return null;
                }
                IExtensible resp = (IExtensible)ProtoBuf.Serializer.Deserialize(t, memory);

                Type ct = MsgTypeRegister.GetRespMsgClassTypeByID(cmd_id);
                if (ct == null)
                {
                    Debug.Log("class type is null");
                    return null;
                }

                MsgBase msg = (MsgBase)Activator.CreateInstance(ct);
                msg.SetResponseData(resp);

                return msg;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("反序列化失败:" + ex.Message);
            return null;
        }
    }

    public static MsgBase DecodeJson(short cmd_id, byte[] bytes, int offset, int count)
	{
		string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
		string type = MsgTypeRegister.GetStringTypeByID(cmd_id);
		if (type != null)
		{
			MsgBase msgBase = (MsgBase)JsonUtility.FromJson(s, Type.GetType(type));
			return msgBase;
		}
		else
		{
			return null;
		}
	}

    //解码协议cmdid（2字节长度+字符串）
    public static short DecodeCmdID(byte[] bytes, int offset){
		//必须大于2字节
		if(offset + 2 > bytes.Length){
			return 0;
		}
		
		Int16 cmd_id = (Int16)((bytes[offset] << 8 )| bytes[offset+1] );
		return cmd_id;
	}
}


