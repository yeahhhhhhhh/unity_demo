using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public enum MsgType{
    None = 0,
    Move = 1,
    Login = 2,
}

public enum MsgPbType
{
    Login = 1,
    LoginRet = 2,
}

public static class MsgTypeRegister
{
    static Dictionary<short, string> msg_id2string_ = new Dictionary<short, string>();
    static Dictionary<short, Type> msgpb_id2MsgPbType_ = new Dictionary<short, Type>();
    static Dictionary<short, Type> msgpb_id2MsgClassType_ = new Dictionary<short, Type>();


    static public void InitRegister()
    {
        Debug.Log("InitRegister");
        RegisterMsg((short)MsgType.Move, "MsgMove");
    }

    static public void InitPbRegister()
    {
        Debug.Log("InitPbRegister");
        RegisterPbMsg((short)MsgPbType.Login, "service.account.LoginRequest", "MsgLogin");
        RegisterPbMsg((short)MsgPbType.LoginRet, "service.account.LoginRequest+Response", "MsgLogin+Response");
    }

    static public string GetStringTypeByID(short id)
    {
        if (msg_id2string_.ContainsKey(id))
        {
            return msg_id2string_[id];
        }

        return null;
    }

    static public bool RegisterMsg(short cmd_id, string type)
    {
        if (msg_id2string_.ContainsKey(cmd_id))
        {
            Debug.Log("cmd id already register:" + cmd_id.ToString());
            return false;
        }

        msg_id2string_[cmd_id] = type;
        return true;
    }

    static public bool RegisterPbMsg(short cmd_id, string type, string class_type)
    {
        if (msgpb_id2MsgPbType_.ContainsKey(cmd_id))
        {
            Debug.Log("pb cmd id already register:" + cmd_id.ToString());
            return false;
        }

        Type t = Type.GetType(type);
        if (t == null)
        {
            Debug.Log("RegisterPbMsg type is null");
            return false;
        }

        if (msgpb_id2MsgClassType_.ContainsKey(cmd_id))
        {
            Debug.Log("pb cmd id already register:" + cmd_id.ToString());
            return false;
        }

        Type ct = Type.GetType(class_type);
        if (t == null)
        {
            Debug.Log("RegisterPbMsg class_type is null");
            return false;
        }

        msgpb_id2MsgPbType_[cmd_id] = t;
        msgpb_id2MsgClassType_[cmd_id] = ct;
        return true;
    }

    static public Type GetMsgClassTypeByID(short id)
    {
        if (msgpb_id2MsgClassType_.ContainsKey(id))
        {
            return msgpb_id2MsgClassType_[id];
        }

        return null;
    }

    static public Type GetPbTypeByID(short id)
    {
        if (msgpb_id2MsgPbType_.ContainsKey(id))
        {
            return msgpb_id2MsgPbType_[id];
        }

        return null;
    }
}